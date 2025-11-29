FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/Directory.Build.props src/
COPY src/Wealth.DepositManagement.API/Wealth.DepositManagement.API.csproj src/Wealth.DepositManagement.API/
RUN dotnet restore src/Wealth.DepositManagement.API
COPY . .
WORKDIR "/src/src/Wealth.DepositManagement.API"
RUN dotnet build "./Wealth.DepositManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wealth.DepositManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.DepositManagement.API.dll"]
