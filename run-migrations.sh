#!/bin/bash
# Script to run database migrations from host machine to Docker PostgreSQL

echo "Running database migrations..."

# Set connection string for Docker PostgreSQL
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=peerdrop_db;Username=peerdrop;Password=peerdrop_password"

cd Backend/PeerDrop.API

# Run migrations
dotnet ef database update --connection "Host=localhost;Port=5432;Database=peerdrop_db;Username=peerdrop;Password=peerdrop_password"

echo "Migrations completed!"
