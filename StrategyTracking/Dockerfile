﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/Directory.Build.props src/
COPY src/Wealth.StrategyTracking.API/Wealth.StrategyTracking.API.csproj src/Wealth.StrategyTracking.API/
RUN dotnet restore src/Wealth.StrategyTracking.API
COPY . .
WORKDIR "/src/src/Wealth.StrategyTracking.API"
RUN dotnet build "./Wealth.StrategyTracking.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wealth.StrategyTracking.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.StrategyTracking.API.dll"]
