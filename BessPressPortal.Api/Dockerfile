# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY BessPressPortal.Api.csproj ./
COPY ../BessPressPortal.Shared/BessPressPortal.Shared.csproj ../BessPressPortal.Shared/
RUN dotnet restore "./BessPressPortal.Api.csproj"
COPY . .
COPY ../BessPressPortal.Shared ../BessPressPortal.Shared
RUN dotnet build "BessPressPortal.Api.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "BessPressPortal.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BessPressPortal.Api.dll"]
