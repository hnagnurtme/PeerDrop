#!/bin/bash
set -e

echo "Running database migrations..."

# Run migrations
dotnet PeerDrop.API.dll ef database update

echo "Migrations completed successfully!"

# Start the application
exec "$@"
