#API for BessPress Portal
# Use ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Paths relative to solution root
COPY BessPressPortal.Api/BessPressPortal.Api.csproj BessPressPortal.Api/
COPY BessPressPortal.Shared/BessPressPortal.Shared.csproj BessPressPortal.Shared/
RUN dotnet restore BessPressPortal.Api/BessPressPortal.Api.csproj

# Copy everything
COPY . .

WORKDIR /src/BessPressPortal.Api
RUN dotnet build BessPressPortal.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BessPressPortal.Api.csproj -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BessPressPortal.Api.dll"]
