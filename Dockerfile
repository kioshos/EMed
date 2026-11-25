FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY EMed.sln ./
COPY Directory.Packages.props ./
COPY Directory.Build.props ./

COPY src/EMed.Domain/EMed.Domain.csproj src/EMed.Domain/
COPY src/EMed.Application/EMed.Application.csproj src/EMed.Application/
COPY src/EMed.Infrastructure.Postgres/EMed.Infrastructure.Postgres.csproj src/EMed.Infrastructure.Postgres/
COPY src/EMed.Api/EMed.Api.csproj src/EMed.Api/
COPY src/EMed.BackgroundServices/EMed.BackgroundServices.csproj src/EMed.BackgroundServices/

RUN dotnet restore src/EMed.Api/EMed.Api.csproj

COPY . .

RUN dotnet publish src/EMed.Api/EMed.Api.csproj \
    -c Release \
    -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /publish/ .

ENTRYPOINT ["dotnet", "EMed.Api.dll"]