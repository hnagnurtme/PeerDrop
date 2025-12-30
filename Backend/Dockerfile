# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["PeerDrop.API/PeerDrop.API.csproj", "PeerDrop.API/"]
COPY ["PeerDrop.BLL/PeerDrop.BLL.csproj", "PeerDrop.BLL/"]
COPY ["PeerDrop.DAL/PeerDrop.DAL.csproj", "PeerDrop.DAL/"]
COPY ["PeerDrop.Shared/PeerDrop.Shared.csproj", "PeerDrop.Shared/"]

RUN dotnet restore "PeerDrop.API/PeerDrop.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/PeerDrop.API"
RUN dotnet build "PeerDrop.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PeerDrop.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PeerDrop.API.dll"]
