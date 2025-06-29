FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InstrumentManagement/src/Wealth.InstrumentManagement.API/Wealth.InstrumentManagement.API.csproj", "InstrumentManagement/src/Wealth.InstrumentManagement.API/"]
RUN dotnet restore "InstrumentManagement/src/Wealth.InstrumentManagement.API/Wealth.InstrumentManagement.API.csproj"
COPY . .
WORKDIR "/src/InstrumentManagement/src/Wealth.InstrumentManagement.API"
RUN dotnet build "./Wealth.InstrumentManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wealth.InstrumentManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wealth.InstrumentManagement.API.dll"]
