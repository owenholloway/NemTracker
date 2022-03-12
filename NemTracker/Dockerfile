FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NemTracker/NemTracker.csproj", "NemTracker/"]
RUN dotnet restore "NemTracker/NemTracker.csproj"
COPY . .
WORKDIR "/src/NemTracker"
RUN dotnet build "NemTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NemTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NemTracker.dll"]
