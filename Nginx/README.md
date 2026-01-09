# Nginx Reverse Proxy for GCP VM

**Domain**: `peerdrop.cloud.hnagnurtme.id.vn`

## URLs

| Path       | Service            |
| ---------- | ------------------ |
| `/`        | Frontend (Angular) |
| `/api/`    | Backend API (.NET) |
| `/health`  | Health check       |
| `/swagger` | API docs           |

## Installation on GCP VM

```bash
# 1. Copy config to nginx
sudo cp peerdrop.conf /etc/nginx/sites-available/peerdrop

# 2. Enable site
sudo ln -sf /etc/nginx/sites-available/peerdrop /etc/nginx/sites-enabled/

# 3. Test and reload
sudo nginx -t
sudo systemctl reload nginx

# 4. SSL with Certbot
sudo certbot --nginx -d peerdrop.cloud.hnagnurtme.id.vn
```

## Docker Compose

```bash
docker-compose -f docker-compose.prod.yml up -d
# Backend: 8080, Frontend: 3000
```
