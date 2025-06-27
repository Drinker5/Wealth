FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/Directory.Build.props src/
COPY src/Wealth.CurrencyManagement.API/Wealth.CurrencyManagement.API.csproj src/Wealth.CurrencyManagement.API/
COPY src/Wealth.CurrencyManagement.Infrastructure/Wealth.CurrencyManagement.Infrastructure.csproj src/Wealth.CurrencyManagement.Infrastructure/
COPY src/Wealth.CurrencyManagement.Application/Wealth.CurrencyManagement.Application.csproj src/Wealth.CurrencyManagement.Application/
COPY src/Wealth.CurrencyManagement.Domain/Wealth.CurrencyManagement.Domain.csproj src/Wealth.CurrencyManagement.Domain/
RUN dotnet restore src/Wealth.CurrencyManagement.API
COPY . .
WORKDIR "/src/src/Wealth.CurrencyManagement.API"
RUN dotnet build "./Wealth.CurrencyManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wealth.CurrencyManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.CurrencyManagement.API.dll"]
