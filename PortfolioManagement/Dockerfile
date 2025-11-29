FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/Directory.Build.props src/
COPY src/Wealth.PortfolioManagement.API/Wealth.PortfolioManagement.API.csproj src/Wealth.PortfolioManagement.API/
RUN dotnet restore src/Wealth.PortfolioManagement.API
COPY . .
WORKDIR "/src/src/Wealth.PortfolioManagement.API"
RUN dotnet build "./Wealth.PortfolioManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wealth.PortfolioManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.PortfolioManagement.API.dll"]
