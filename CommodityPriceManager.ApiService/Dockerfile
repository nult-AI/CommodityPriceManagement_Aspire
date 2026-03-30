# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["CommodityPriceManager.sln", "."]
COPY ["CommodityPriceManager.ApiService/CommodityPriceManager.ApiService.csproj", "CommodityPriceManager.ApiService/"]
COPY ["CommodityPriceManager.ServiceDefaults/CommodityPriceManager.ServiceDefaults.csproj", "CommodityPriceManager.ServiceDefaults/"]

# Restore
RUN dotnet restore "CommodityPriceManager.ApiService/CommodityPriceManager.ApiService.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/CommodityPriceManager.ApiService"
RUN dotnet publish "CommodityPriceManager.ApiService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CommodityPriceManager.ApiService.dll"]
