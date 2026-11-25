using Npgsql;

public sealed class BenchmarkDataSeeder
{
    private readonly string _connectionString;

    public BenchmarkDataSeeder(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<Guid>> SeedPatientsAsync(
        int count,
        CancellationToken ct)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);

        await using var connection = await dataSource.OpenConnectionAsync(ct);

        await using var command = connection.CreateCommand();

        command.CommandText = """
                              TRUNCATE TABLE "Patients" CASCADE;

                              INSERT INTO "Patients"
                                  ("Id", "TelephoneNumber", "Address", "Age", "Name")
                              SELECT
                                  gen_random_uuid(),
                                  '+380501234567',
                                  'Benchmark Address ' || n,
                                  20 + (n % 60),
                                  'Patient_' || n
                              FROM generate_series(1, @count) AS n;

                              SELECT "Id"
                              FROM "Patients"
                              ORDER BY "Name";
                              """;

        command.Parameters.AddWithValue("count", count);

        var patientIds = new List<Guid>(count);

        await using var reader = await command.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            patientIds.Add(reader.GetGuid(0));
        }

        return patientIds;
    }
}