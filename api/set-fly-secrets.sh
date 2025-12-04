#!/bin/bash

# Script to set all required Fly.io secrets
# Run this from the api directory: ./set-fly-secrets.sh

echo "Setting Fly.io secrets..."
echo "Make sure you have your database credentials and JWT key ready!"
echo ""

# Database secrets (REQUIRED)
read -p "Enter DB_HOST: " DB_HOST
read -p "Enter DB_NAME: " DB_NAME
read -p "Enter DB_USER: " DB_USER
read -sp "Enter DB_PASSWORD: " DB_PASSWORD
echo ""
read -p "Enter DB_PORT (default: 5432): " DB_PORT
DB_PORT=${DB_PORT:-5432}

# JWT secrets (REQUIRED)
read -sp "Enter JWT_KEY (generate with: openssl rand -base64 32): " JWT_KEY
echo ""
read -p "Enter JWT_ISSUER (default: MyPortfolio): " JWT_ISSUER
JWT_ISSUER=${JWT_ISSUER:-MyPortfolio}
read -p "Enter JWT_AUDIENCE (default: MyPortfolioUsers): " JWT_AUDIENCE
JWT_AUDIENCE=${JWT_AUDIENCE:-MyPortfolioUsers}
read -p "Enter JWT_EXPIRE_MINUTES (default: 60): " JWT_EXPIRE_MINUTES
JWT_EXPIRE_MINUTES=${JWT_EXPIRE_MINUTES:-60}

# CORS (REQUIRED for production)
read -p "Enter ALLOWED_ORIGINS (comma-separated, e.g., https://your-app.vercel.app): " ALLOWED_ORIGINS

# Email (OPTIONAL)
read -p "Enter RESEND_API_KEY (optional, press Enter to skip): " RESEND_API_KEY
read -p "Enter RESEND_FROM_EMAIL (optional): " RESEND_FROM_EMAIL
read -p "Enter RESEND_FROM_NAME (optional, default: Judah Sullivan): " RESEND_FROM_NAME
RESEND_FROM_NAME=${RESEND_FROM_NAME:-Judah Sullivan}
read -p "Enter FRONTEND_URL (optional): " FRONTEND_URL
read -p "Enter CONTACT_EMAIL (optional, default: hello@judahsullivan.com): " CONTACT_EMAIL
CONTACT_EMAIL=${CONTACT_EMAIL:-hello@judahsullivan.com}

echo ""
echo "Setting secrets..."

# Set all secrets
fly secrets set \
  DB_HOST="$DB_HOST" \
  DB_PORT="$DB_PORT" \
  DB_NAME="$DB_NAME" \
  DB_USER="$DB_USER" \
  DB_PASSWORD="$DB_PASSWORD" \
  JWT_KEY="$JWT_KEY" \
  JWT_ISSUER="$JWT_ISSUER" \
  JWT_AUDIENCE="$JWT_AUDIENCE" \
  JWT_EXPIRE_MINUTES="$JWT_EXPIRE_MINUTES" \
  ALLOWED_ORIGINS="$ALLOWED_ORIGINS"

# Set optional email secrets if provided
if [ -n "$RESEND_API_KEY" ]; then
  fly secrets set RESEND_API_KEY="$RESEND_API_KEY"
fi

if [ -n "$RESEND_FROM_EMAIL" ]; then
  fly secrets set RESEND_FROM_EMAIL="$RESEND_FROM_EMAIL"
fi

if [ -n "$RESEND_FROM_NAME" ]; then
  fly secrets set RESEND_FROM_NAME="$RESEND_FROM_NAME"
fi

if [ -n "$FRONTEND_URL" ]; then
  fly secrets set FRONTEND_URL="$FRONTEND_URL"
fi

if [ -n "$CONTACT_EMAIL" ]; then
  fly secrets set CONTACT_EMAIL="$CONTACT_EMAIL"
fi

echo ""
echo "✅ Secrets set! The app will restart automatically."
echo "Check logs with: fly logs"

