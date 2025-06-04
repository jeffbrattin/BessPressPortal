# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy .csproj files with correct relative paths
COPY BessPressPortal.Api/BessPressPortal.Api.csproj BessPressPortal.Api/
COPY BessPressPortal.Shared/BessPressPortal.Shared.csproj BessPressPortal.Shared/

# Restore
WORKDIR /src/BessPressPortal.Api
RUN dotnet restore

# Copy the full source code
WORKDIR /src
COPY BessPressPortal.Api/ BessPressPortal.Api/
COPY BessPressPortal.Shared/ BessPressPortal.Shared/

WORKDIR /src/BessPressPortal.Api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /src/BessPressPortal.Api
RUN dotnet publish -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BessPressPortal.Api.dll"]
