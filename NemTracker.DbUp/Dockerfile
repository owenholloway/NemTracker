﻿FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NemTracker.DbUp.csproj", "./"]
RUN dotnet restore "./NemTracker.DbUp.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "NemTracker.DbUp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NemTracker.DbUp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NemTracker.DbUp.dll"]
