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
# 1. Install Certbot
sudo apt update
sudo apt install -y certbot python3-certbot-nginx

# 2. Copy SSL params
sudo mkdir -p /etc/nginx/snippets
sudo cp ssl-params.conf /etc/nginx/snippets/

# 3. Copy config to nginx
sudo cp peerdrop.conf /etc/nginx/sites-available/peerdrop

# 4. Enable site
sudo ln -sf /etc/nginx/sites-available/peerdrop /etc/nginx/sites-enabled/

# 5. Create directory for Let's Encrypt challenges
sudo mkdir -p /var/www/certbot

# 6. Obtain SSL certificate
sudo certbot certonly --webroot -w /var/www/certbot \
  -d peerdrop.cloud.hnagnurtme.id.vn \
  --email your-email@example.com \
  --agree-tos \
  --no-eff-email

# 7. Test and reload nginx
sudo nginx -t
sudo systemctl reload nginx

# 8. Auto-renewal (certbot creates this automatically)
sudo certbot renew --dry-run
```

## Docker Compose

```bash
docker-compose -f docker-compose.prod.yml up -d
# Backend: 8080, Frontend: 3000
```
