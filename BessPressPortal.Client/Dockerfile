# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BessPressPortal.Client/BessPressPortal.Client.csproj", "BessPressPortal.Client/"]
COPY ["BessPressPortal.Shared/BessPressPortal.Shared.csproj", "BessPressPortal.Shared/"]
RUN dotnet restore "BessPressPortal.Client/BessPressPortal.Client.csproj"
COPY . .
WORKDIR "/src/BessPressPortal.Client"
RUN dotnet publish "BessPressPortal.Client.csproj" -c Release -o /app/publish

# Final stage
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
