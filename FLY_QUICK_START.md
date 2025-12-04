# Fly.io Quick Start

## Prerequisites

1. Install Fly CLI: `curl -L https://fly.io/install.sh | sh`
2. Login: `fly auth login`
3. Set up PostgreSQL database (Supabase/Neon recommended)

## Deployment Steps

### 1. Initialize (First Time Only)

```bash
cd api
fly launch
# Choose app name, region (e.g., iad)
```

### 2. Set Environment Variables

```bash
# Database
fly secrets set DB_HOST=your-host DB_NAME=your-db DB_USER=your-user DB_PASSWORD=your-password

# JWT (generate: openssl rand -base64 32)
fly secrets set JWT_KEY=your-key JWT_ISSUER=MyPortfolio JWT_AUDIENCE=MyPortfolioUsers

# CORS
fly secrets set ALLOWED_ORIGINS=https://your-app.vercel.app

# Optional: Email
fly secrets set RESEND_API_KEY=key RESEND_FROM_EMAIL=email FRONTEND_URL=https://yourdomain.com
```

### 3. Deploy

```bash
fly deploy
```

### 4. Run Migrations

```bash
# Option 1: Local (point .env to production DB temporarily)
cd api && dotnet ef database update

# Option 2: Remote
fly ssh console -C "cd /app && dotnet ef database update"
```

### 5. Verify

```bash
fly open
# Should show weather forecast JSON
```

## Your App URL

After deployment: `https://your-app-name.fly.dev`

Update Vercel environment variable:
```
NUXT_PUBLIC_API_BASE=https://your-app-name.fly.dev
```

## Useful Commands

```bash
fly logs          # View logs
fly status        # Check status
fly secrets list  # List secrets
fly ssh console   # SSH into machine
```

## Full Guide

See `docs/FLY_DEPLOYMENT.md` for complete documentation.

