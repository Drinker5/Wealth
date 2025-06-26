FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/Directory.Build.props src/
COPY src/API/Wealth.CurrencyManagement.API/Wealth.CurrencyManagement.API.csproj src/API/Wealth.CurrencyManagement.API/
COPY src/Infrastructure/Wealth.CurrencyManagement.Infrastructure/Wealth.CurrencyManagement.Infrastructure.csproj src/Infrastructure/Wealth.CurrencyManagement.Infrastructure/
COPY src/Application/Wealth.CurrencyManagement.Application/Wealth.CurrencyManagement.Application.csproj src/Application/Wealth.CurrencyManagement.Application/
COPY src/Domain/Wealth.CurrencyManagement.Domain/Wealth.CurrencyManagement.Domain.csproj src/Domain/Wealth.CurrencyManagement.Domain/
RUN dotnet restore src/API/Wealth.CurrencyManagement.API
COPY . .
WORKDIR "/src/src/API/Wealth.CurrencyManagement.API"
RUN dotnet build "./Wealth.CurrencyManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wealth.CurrencyManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.CurrencyManagement.API.dll"]
