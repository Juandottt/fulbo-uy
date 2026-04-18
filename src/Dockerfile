# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos primero (para cache de capas)
COPY FulboUY.sln ./
COPY FulboUY.API/FulboUY.API.csproj FulboUY.API/
COPY FulboUY.Application/FulboUY.Application.csproj FulboUY.Application/
COPY FulboUY.Domain/FulboUY.Domain.csproj FulboUY.Domain/
COPY FulboUY.Infrastructure/FulboUY.Infrastructure.csproj FulboUY.Infrastructure/

RUN dotnet restore

# Copiar todo el código fuente
COPY . .

# Publicar en modo Release
RUN dotnet publish FulboUY.API/FulboUY.API.csproj -c Release -o /app/publish

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FulboUY.API.dll"]
