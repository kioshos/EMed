using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using NBomber.Contracts;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};
    
var ct = cts.Token;

var network = new NetworkBuilder()
    .WithName($"emed-performance-{Guid.NewGuid():N}")
    .Build();

PostgreSqlContainer? postgres = null;
RedisContainer? redis = null;
IContainer? api = null;

try
{
    await network.CreateAsync(ct);

    postgres = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("EMed")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithNetwork(network)
        .WithNetworkAliases("postgres")
        .WithPortBinding(5432, true)
        .Build();

    redis = new RedisBuilder()
        .WithImage("redis:8.6.8")
        .WithNetwork(network)
        .WithNetworkAliases("redis")
        .Build();

    await postgres.StartAsync(ct);
    await redis.StartAsync(ct);

    var sqlScript = await File.ReadAllTextAsync("migration.sql", ct);

    await postgres.ExecScriptAsync(sqlScript, ct);

    api = new ContainerBuilder()
        .WithImage("emed:dev")
        .WithImagePullPolicy(PullPolicy.Never)
        .WithNetwork(network)
        .WithPortBinding(5265, 8080)
        .WithEnvironment(
            "ASPNETCORE_ENVIRONMENT", "Performance")
        .WithEnvironment(
            "ConnectionStrings__Postgres",
            "Host=postgres;Port=5432;Database=EMed;Username=postgres;Password=postgres")
        .WithEnvironment(
            "ConnectionStrings__Redis",
            "redis:6379")
        .Build();

    await api.StartAsync(ct);

    int[] dataSizes = new[] { 100, 1_000, 10_000 };

    BenchmarkDataSeeder seeder = new BenchmarkDataSeeder(postgres.GetConnectionString());

    const int RequestsPerExperiment = 100;
    const int RequestsPerSecond = 10;

    const string BaseUrl = "http://localhost:5265";

    foreach (int size in dataSizes)
    {
        ct.ThrowIfCancellationRequested();
        
        Console.WriteLine($"Preparing {size} users...");
        
        var patientIds = await seeder.SeedPatientsAsync(size, ct);
        
        var hitScenario = CreateUserScenario(
            $"CacheHit_{size}",
            BaseUrl,
            patientIds,
            RequestsPerExperiment,
            RequestsPerSecond,
            cacheHit: true);

        var missScenario = CreateUserScenario(
            $"CacheMiss_{size}",
            BaseUrl,
            patientIds,
            RequestsPerExperiment,
            RequestsPerSecond,
            cacheHit: false);
        
        RunBenchmark(hitScenario, $"CacheHit_{size}");
        RunBenchmark(missScenario, $"CacheMiss_{size}");
        
        Console.WriteLine($"Finished {size} users.");
    }
}
finally
{
    if(api is not null)
        await api.DisposeAsync();
    
    if(redis is not null)
        await redis.DisposeAsync();
    
    if(postgres is not null)
        await postgres.DisposeAsync();
    
    await network.DisposeAsync();
}


static ScenarioProps CreateUserScenario(
    string scenarioName,
    string baseUrl,
    IReadOnlyList<Guid> userIds,
    int requestCount,
    int requestsPerSecond,
    bool cacheHit)
{
    if (requestCount <= 0)
        throw new ArgumentOutOfRangeException(nameof(requestCount));

    if (userIds.Count == 0)
        throw new ArgumentException("User IDs cannot be empty.");

    if (!cacheHit && requestCount > userIds.Count)
        throw new ArgumentException(
            "Not enough unique IDs for Cache Miss.");

    var index = -1;
    var httpClient = Http.CreateDefaultClient();

    return Scenario.Create(scenarioName, async context =>
        {
            var currentIndex = Interlocked.Increment(ref index);
            
            var patientId = cacheHit
                ? userIds[0]
                : userIds[currentIndex];

            var request = Http.CreateRequest(
                "GET",
                $"{baseUrl}/patients/{patientId}");

            return await Http.Send(httpClient, request);
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.IterationsForInject(
                rate: requestsPerSecond,
                interval: TimeSpan.FromSeconds(1),
                iterations: requestCount));
}


static void RunBenchmark(ScenarioProps scenario, string reportName)
{
    NBomberRunner
        .RegisterScenarios(scenario)
        .WithTestSuite("Patients API")
        .WithTestName(reportName)
        .WithReportFolder($"reports/{reportName}")
        .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
        .Run();
}