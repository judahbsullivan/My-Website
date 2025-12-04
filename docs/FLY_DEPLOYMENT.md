# Fly.io Deployment Guide

Complete guide for deploying your Portfolio API to Fly.io.

## Prerequisites

1. **Fly.io Account**: Sign up at https://fly.io
2. **Fly CLI**: Install from https://fly.io/docs/getting-started/installing-flyctl/
3. **PostgreSQL Database**: Set up on Supabase, Neon, or Railway (Fly.io doesn't provide free PostgreSQL)

## Quick Start

### 1. Install Fly CLI

```bash
# macOS
curl -L https://fly.io/install.sh | sh

# Or with Homebrew
brew install flyctl
```

### 2. Login to Fly.io

```bash
fly auth login
```

### 3. Initialize Your App

```bash
cd api
fly launch
```

This will:
- Ask for app name (or use existing `fly.toml`)
- Ask for region (choose closest to you, e.g., `iad` for US East)
- Create `fly.toml` if it doesn't exist
- Optionally set up a database (skip this, use external PostgreSQL)

### 4. Set Environment Variables

Set all required environment variables:

```bash
# Database (from your PostgreSQL provider)
fly secrets set DB_HOST=your-db-host
fly secrets set DB_PORT=5432
fly secrets set DB_NAME=your-db-name
fly secrets set DB_USER=your-db-user
fly secrets set DB_PASSWORD=your-db-password

# JWT (generate with: openssl rand -base64 32)
fly secrets set JWT_KEY=your-generated-secret-key
fly secrets set JWT_ISSUER=MyPortfolio
fly secrets set JWT_AUDIENCE=MyPortfolioUsers
fly secrets set JWT_EXPIRE_MINUTES=60

# CORS (your Vercel domain)
fly secrets set ALLOWED_ORIGINS=https://your-app.vercel.app,https://yourdomain.com

# Email (optional)
fly secrets set RESEND_API_KEY=your-resend-key
fly secrets set RESEND_FROM_EMAIL=noreply@yourdomain.com
fly secrets set RESEND_FROM_NAME="Judah Sullivan"
fly secrets set FRONTEND_URL=https://yourdomain.com
fly secrets set CONTACT_EMAIL=hello@judahsullivan.com
```

**Or set multiple at once:**
```bash
fly secrets set \
  DB_HOST=your-host \
  DB_NAME=your-db \
  DB_USER=your-user \
  DB_PASSWORD=your-password \
  JWT_KEY=your-jwt-key
```

### 5. Deploy

```bash
fly deploy
```

This will:
- Build your Docker image
- Push to Fly.io
- Deploy your app

### 6. Run Database Migrations

After deployment, run migrations:

```bash
# Option 1: SSH into the machine and run
fly ssh console
dotnet ef database update

# Option 2: Use flyctl to run command
fly ssh console -C "cd /app && dotnet ef database update"
```

**Better approach**: Run migrations locally pointing to production DB:

```bash
# Temporarily set production DB connection in your local .env
# Then run:
cd api
dotnet ef database update
```

### 7. Verify Deployment

```bash
# Check app status
fly status

# View logs
fly logs

# Open app in browser
fly open
```

Test the health endpoint:
```bash
curl https://your-app.fly.dev/weatherforecast
```

## Configuration Files

### fly.toml

Located in `api/fly.toml`. Key settings:

- `app`: Your app name (change from "your-portfolio-api")
- `primary_region`: Deployment region
- `internal_port`: 8080 (matches Dockerfile)
- `min_machines_running`: 1 (always running, no cold starts)

### Dockerfile

Located in `api/Dockerfile`. Already configured for:
- Multi-stage build
- .NET 10.0
- Port 8080
- Production environment

## Managing Your App

### View Logs

```bash
fly logs
```

### Scale Your App

```bash
# Scale to 2 instances
fly scale count 2

# Scale memory
fly scale vm shared-cpu-1x --memory 256
```

### Update Environment Variables

```bash
fly secrets set NEW_VAR=value
```

### Restart App

```bash
fly apps restart your-app-name
```

## Database Setup

Fly.io doesn't provide free PostgreSQL. Use one of these:

### Option 1: Supabase (Recommended)

1. Sign up at https://supabase.com
2. Create new project
3. Get connection string from Settings → Database
4. Set `DB_HOST`, `DB_NAME`, `DB_USER`, `DB_PASSWORD` in Fly.io secrets

### Option 2: Neon

1. Sign up at https://neon.tech
2. Create database
3. Get connection string
4. Set database variables in Fly.io

### Option 3: Railway PostgreSQL

1. Create Railway account
2. Add PostgreSQL service
3. Get connection details
4. Set in Fly.io secrets

## Troubleshooting

### Build Fails

```bash
# Check build logs
fly logs

# Build locally to test
docker build -t test-build .
```

### Database Connection Issues

- Verify connection string format
- Check firewall/network access
- Ensure SSL is enabled in connection string
- Test connection locally with production credentials

### CORS Errors

- Verify `ALLOWED_ORIGINS` includes your exact Vercel URL
- Check for trailing slashes
- Ensure no typos in origins

### App Won't Start

```bash
# Check logs
fly logs

# SSH into machine
fly ssh console

# Check environment variables
fly ssh console -C "env | grep DB_"
```

### Port Issues

Fly.io automatically sets `PORT` env var. Our app uses port 8080 by default, which matches `fly.toml`.

## Cost

**Free Tier Includes:**
- 3 shared-cpu VMs
- 3GB persistent volumes
- 160GB outbound data transfer

**For this app:**
- 1 VM running 24/7: ~$0-5/month (depending on usage)
- Database: Free with Supabase/Neon
- **Total: ~$0-5/month**

## Next Steps

1. Set up PostgreSQL database (Supabase recommended)
2. Configure environment variables
3. Deploy: `fly deploy`
4. Run migrations
5. Update Vercel frontend with API URL
6. Test everything!

## Useful Commands

```bash
# View app info
fly info

# View secrets (names only, not values)
fly secrets list

# SSH into machine
fly ssh console

# View metrics
fly metrics

# Open app
fly open

# Deploy
fly deploy

# View status
fly status
```

