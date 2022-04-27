FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NemTracker/", "NemTracker/"]
COPY ["NemTracker.Domain/", "NemTracker.Domain"]
COPY ["NemTracker.Dtos/", "NemTracker.Dtos/"]
COPY ["NemTracker.Persistence/", "NemTracker.Persistence/"]
COPY ["NemTracker.Tools/", "NemTracker.Tools/"]
RUN --mount=type=secret,id=api-login cat /run/secrets/api-login > /api-login
RUN dotnet nuget add source https://vulcan.lanceolata.com.au/api/v4/projects/42/packages/nuget/index.json --name oxygen-packages --username owen.holloway --password $(cat /api-login) --store-password-in-clear-text
RUN dotnet restore "NemTracker/NemTracker.csproj"
COPY . .
WORKDIR "/src/NemTracker"
RUN dotnet build "NemTracker.csproj" -c Release -o /app/build
RUN ls -la /app/build/

FROM build AS publish
RUN dotnet publish "NemTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NemTracker.dll"]
