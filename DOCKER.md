# PeerDrop Docker Setup

## Quick Start

```bash
# Build and start all services
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop all services
docker-compose down

# Stop and remove volumes (WARNING: deletes database data)
docker-compose down -v
```

## Services

### PostgreSQL Database
- **Port**: 5432
- **Username**: peerdrop
- **Password**: peerdrop_password
- **Database**: peerdrop_db
- **Data**: Persisted in `postgres_data` volume

### Backend API (.NET 8)
- **Port**: 8080
- **URL**: http://localhost:8080
- **Swagger**: http://localhost:8080/swagger
- **Health Check**: http://localhost:8080/health

### Frontend (React + Nginx)
- **Port**: 3000
- **URL**: http://localhost:3000

## Environment Variables

### Backend
Edit `docker-compose.yml` to configure:
- `ConnectionStrings__DefaultConnection` - PostgreSQL connection
- `JwtSettings__SecretKey` - JWT secret (min 32 chars)
- `JwtSettings__Issuer` - JWT issuer
- `JwtSettings__Audience` - JWT audience
- `JwtSettings__ExpiryInMinutes` - Token expiry time

### Frontend
- `VITE_API_URL` - Backend API URL (default: http://localhost:8080/api)

## Database Migrations

**Automated Migrations** (Recommended):

After starting services for the first time, run migrations automatically:

```bash
# Generate migration SQL script
cd Backend/PeerDrop.API
dotnet ef migrations script --output /tmp/init.sql --idempotent

# Copy and execute in PostgreSQL container
docker cp /tmp/init.sql peerdrop-postgres:/tmp/init.sql
docker exec peerdrop-postgres sh -c 'PGPASSWORD=peerdrop_password psql -U peerdrop -d peerdrop_db -f /tmp/init.sql'
```

Or use the provided script:

```bash
./run-migrations.sh
```

**Manual Verification:**

Check if tables were created:

```bash
docker exec peerdrop-postgres sh -c 'PGPASSWORD=peerdrop_password psql -U peerdrop -d peerdrop_db -c "\dt"'
```

Expected output:
```
Schema |         Name          | Type  |  Owner   
--------+-----------------------+-------+----------
 public | Users                 | table | peerdrop
 public | __EFMigrationsHistory | table | peerdrop
```

## Development

For development, use docker-compose with live reload:

```bash
# Start only database
docker-compose up -d postgres

# Run backend and frontend locally
cd Backend/PeerDrop.API && dotnet run
cd Frontend && npm run dev
```

## Production Deployment

**IMPORTANT**: Before deploying to production:

1. **Change default passwords** in `docker-compose.yml`
2. **Update JWT secret** to a strong random value
3. **Set proper CORS** origins in Backend
4. **Use environment files** instead of hardcoded values
5. **Enable HTTPS** with reverse proxy (nginx/traefik)
6. **Configure backups** for PostgreSQL volume

## Troubleshooting

### Backend won't start
```bash
# Check logs
docker-compose logs backend

# Verify database is ready
docker-compose ps postgres
```

### Frontend can't connect to backend
- Check `VITE_API_URL` in docker-compose.yml
- Verify backend is running: `curl http://localhost:8080/health`
- Check CORS settings in Backend

### Database connection failed
```bash
# Check PostgreSQL logs
docker-compose logs postgres

# Verify connection string
docker exec -it peerdrop-postgres psql -U peerdrop -d peerdrop_db
```

## Architecture

```
┌─────────────┐      ┌─────────────┐      ┌─────────────┐
│  Frontend   │─────▶│   Backend   │─────▶│  PostgreSQL │
│  (React)    │      │   (.NET 8)  │      │             │
│  Port: 3000 │      │  Port: 8080 │      │  Port: 5432 │
└─────────────┘      └─────────────┘      └─────────────┘
     Nginx              ASP.NET Core         PostgreSQL 16
```

## Network

All services communicate via the `peerdrop-network` bridge network:
- Frontend → Backend: http://backend:8080
- Backend → Database: postgres:5432
