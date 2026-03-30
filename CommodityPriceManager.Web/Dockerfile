# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["CommodityPriceManager.sln", "."]
COPY ["CommodityPriceManager.Web/CommodityPriceManager.Web.csproj", "CommodityPriceManager.Web/"]

# Restore
RUN dotnet restore "CommodityPriceManager.Web/CommodityPriceManager.Web.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/CommodityPriceManager.Web"
RUN dotnet publish "CommodityPriceManager.Web.csproj" -c Release -o /app/publish

# Static Web Stage - Using Nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
# Copy publish output to nginx
COPY --from=build /app/publish/wwwroot .
# Default Nginx config usually enough for WASM. For SPA routing, custom config is better.
# For simplicity, we just provide the binary.
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
