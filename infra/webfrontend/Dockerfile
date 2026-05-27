# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore
COPY ["CommodityPriceManager.Web.csproj", "./"]
RUN dotnet restore "CommodityPriceManager.Web.csproj"

# Copy source and publish static assets
COPY . .
RUN dotnet publish "CommodityPriceManager.Web.csproj" -c Release -o /app/publish

# Static web stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]