#!/bin/bash

# PeerDrop Nginx Setup Script for GCP VM
# Run this script on your GCP VM to configure Nginx

set -e

echo "=== PeerDrop Nginx Setup ==="

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    echo "Please run as root (sudo)"
    exit 1
fi

# Variables
DOMAIN=${1:-"your-domain.com"}
NGINX_CONF_DIR="/etc/nginx"
SITES_AVAILABLE="$NGINX_CONF_DIR/sites-available"
SITES_ENABLED="$NGINX_CONF_DIR/sites-enabled"
SNIPPETS_DIR="$NGINX_CONF_DIR/snippets"

echo "Domain: $DOMAIN"

# Install nginx if not installed
if ! command -v nginx &> /dev/null; then
    echo "Installing Nginx..."
    apt update
    apt install -y nginx
fi

# Create directories if not exist
mkdir -p "$SNIPPETS_DIR"

# Copy configuration files
echo "Copying configuration files..."
cp peerdrop.conf "$SITES_AVAILABLE/peerdrop"
cp ssl-params.conf "$SNIPPETS_DIR/ssl-params.conf"

# Replace domain placeholder
sed -i "s/your-domain.com/$DOMAIN/g" "$SITES_AVAILABLE/peerdrop"

# Enable site
echo "Enabling site..."
ln -sf "$SITES_AVAILABLE/peerdrop" "$SITES_ENABLED/"

# Remove default site
rm -f "$SITES_ENABLED/default"

# Test configuration
echo "Testing Nginx configuration..."
nginx -t

# Reload nginx
echo "Reloading Nginx..."
systemctl reload nginx

echo ""
echo "=== Setup Complete ==="
echo ""
echo "Next steps:"
echo "1. Update your DNS to point $DOMAIN to this server's IP"
echo "2. For SSL, run: sudo certbot --nginx -d $DOMAIN"
echo "3. Start your Docker containers with docker-compose"
echo ""
