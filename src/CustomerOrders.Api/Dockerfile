FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CustomerOrders.Api/CustomerOrders.Api.csproj", "src/CustomerOrders.Api/"]
COPY ["src/CustomerOrders.Business/CustomerOrders.Business.csproj", "src/CustomerOrders.Business/"]
COPY ["src/CustomerOrders.Data/CustomerOrders.Data.csproj", "src/CustomerOrders.Data/"]
RUN dotnet restore "src/CustomerOrders.Api/CustomerOrders.Api.csproj"
COPY . .
WORKDIR "/src/src/CustomerOrders.Api"
RUN dotnet build "./CustomerOrders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CustomerOrders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerOrders.Api.dll"]
