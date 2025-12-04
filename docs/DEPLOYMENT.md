# Deployment Guide

This guide covers deploying your portfolio to production.

## Architecture

- **Frontend**: Nuxt.js → Vercel
- **Backend**: .NET 10.0 API → [Choose platform below]
- **Database**: PostgreSQL → [Choose provider below]

---

## Free Deployment Options for .NET API

### 1. **Railway** (Recommended) ⭐
- **Free Tier**: $5 credit/month (usually enough for small apps)
- **Pros**: 
  - Easy PostgreSQL setup
  - Automatic HTTPS
  - Environment variables UI
  - GitHub integration
  - Good documentation
- **Cons**: Credit-based, may need to upgrade for high traffic
- **Best for**: Quick deployment with database included

### 2. **Render**
- **Free Tier**: 
  - Web services: Free (spins down after 15min inactivity)
  - PostgreSQL: Free (90 days, then $7/month)
- **Pros**: 
  - Free PostgreSQL for 90 days
  - Automatic HTTPS
  - GitHub integration
- **Cons**: 
  - Cold starts on free tier
  - Database expires after 90 days
- **Best for**: Testing/development

### 3. **Fly.io**
- **Free Tier**: 3 shared-cpu VMs, 3GB persistent volumes
- **Pros**: 
  - Generous free tier
  - Global edge deployment
  - Good for production
- **Cons**: 
  - More complex setup
  - Need separate PostgreSQL (can use Supabase free tier)
- **Best for**: Production apps with global reach

### 4. **Azure App Service** (Free Tier)
- **Free Tier**: F1 tier (limited resources)
- **Pros**: 
  - Microsoft ecosystem
  - Good integration
- **Cons**: 
  - Very limited resources
  - May need upgrade quickly
- **Best for**: If already using Azure

### 5. **DigitalOcean App Platform**
- **Free Tier**: $200 credit for 60 days (trial)
- **Pros**: 
  - Good performance
  - Easy setup
- **Cons**: 
  - Not permanently free
  - Need credit card
- **Best for**: Short-term testing

---

## Free PostgreSQL Database Options

### 1. **Supabase** (Recommended) ⭐
- **Free Tier**: 500MB database, 2GB bandwidth
- **Pros**: 
  - Generous free tier
  - Great dashboard
  - Built-in auth (if needed later)
- **Best for**: Production-ready free tier

### 2. **Neon**
- **Free Tier**: 0.5GB storage, auto-suspend
- **Pros**: 
  - Serverless PostgreSQL
  - Good performance
- **Best for**: Serverless architectures

### 3. **Railway PostgreSQL**
- **Free Tier**: Included with Railway deployment
- **Pros**: 
  - Integrated with Railway
  - Easy setup
- **Best for**: If using Railway for API

### 4. **ElephantSQL**
- **Free Tier**: 20MB database
- **Pros**: 
  - Simple setup
- **Cons**: 
  - Very limited storage
- **Best for**: Testing only

---

## Recommended Setup

**For Production**: Railway (API) + Supabase (Database) or Railway (Both)
**For Testing**: Render (API + Database) or Fly.io + Supabase

---

## Pre-Deployment Checklist

### 1. Environment Variables Required

Create a `.env.example` file with all required variables:

```bash
# Database
DB_HOST=your-db-host
DB_PORT=5432
DB_NAME=your-db-name
DB_USER=your-db-user
DB_PASSWORD=your-db-password

# JWT
JWT_KEY=your-super-secret-key-at-least-32-characters-long
JWT_ISSUER=MyPortfolio
JWT_AUDIENCE=MyPortfolioUsers
JWT_EXPIRE_MINUTES=60

# CORS (comma-separated)
ALLOWED_ORIGINS=https://yourdomain.com,https://www.yourdomain.com

# Email (Resend)
RESEND_API_KEY=your-resend-api-key
RESEND_FROM_EMAIL=noreply@yourdomain.com
RESEND_FROM_NAME=Judah Sullivan
FRONTEND_URL=https://yourdomain.com

# Contact Email
CONTACT_EMAIL=hello@judahsullivan.com
```

### 2. Generate Secure JWT Key

```bash
# Generate a secure random key (32+ characters)
openssl rand -base64 32
```

### 3. Update Frontend Configuration

Update `web/nuxt.config.ts`:
- Set `NUXT_PUBLIC_API_BASE` to your production API URL

### 4. Database Migration

Run migrations on production database:
```bash
cd api
dotnet ef database update
```

### 5. Remove .env Loading in Production

The `Env.Load("../.env")` in `Program.cs` should only work in development.

---

## Deployment Steps by Platform

### Railway Deployment

1. **Sign up**: https://railway.app
2. **Create New Project** → "Deploy from GitHub repo"
3. **Add PostgreSQL** service
4. **Add .NET** service:
   - Root Directory: `api`
   - Build Command: `dotnet publish -c Release -o /app`
   - Start Command: `dotnet /app/api.dll`
5. **Set Environment Variables** (from checklist above)
6. **Deploy**

### Render Deployment

1. **Sign up**: https://render.com
2. **Create New Web Service**:
   - Build Command: `cd api && dotnet publish -c Release -o ./publish`
   - Start Command: `cd api && dotnet ./publish/api.dll`
3. **Create PostgreSQL Database**
4. **Set Environment Variables**
5. **Deploy**

### Fly.io Deployment

1. **Install Fly CLI**: `curl -L https://fly.io/install.sh | sh`
2. **Login**: `fly auth login`
3. **Initialize**: `cd api && fly launch`
4. **Create `fly.toml`** (see example below)
5. **Set Secrets**: `fly secrets set KEY=value`
6. **Deploy**: `fly deploy`

---

## Configuration Files

### Railway (railway.json - optional)
```json
{
  "$schema": "https://railway.app/railway.schema.json",
  "build": {
    "builder": "NIXPACKS"
  },
  "deploy": {
    "startCommand": "dotnet api.dll",
    "restartPolicyType": "ON_FAILURE",
    "restartPolicyMaxRetries": 10
  }
}
```

### Fly.io (fly.toml)
```toml
app = "your-app-name"
primary_region = "iad"

[build]

[env]
  ASPNETCORE_ENVIRONMENT = "Production"

[http_service]
  internal_port = 8080
  force_https = true
  auto_stop_machines = false
  auto_start_machines = true
  min_machines_running = 1

[[services]]
  protocol = "tcp"
  internal_port = 8080
```

### Dockerfile (Optional - for containerized deployments)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["api.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
```

---

## Vercel Frontend Deployment

1. **Connect GitHub repo** to Vercel
2. **Set Root Directory**: `web`
3. **Build Settings**:
   - Framework Preset: Nuxt.js
   - Build Command: `npm run build` (or `bun run build`)
   - Output Directory: `.output/public`
4. **Environment Variables**:
   ```
   NUXT_PUBLIC_API_BASE=https://your-api-url.com
   ```
5. **Deploy**

---

## Post-Deployment

### 1. Run Database Migrations
```bash
# Connect to your production database and run:
dotnet ef database update
```

### 2. Test Endpoints
- Health check: `GET https://your-api.com/weatherforecast`
- Contact form: `POST https://your-api.com/api/contact`
- Auth: `POST https://your-api.com/api/auth/login`

### 3. Verify CORS
- Test from your Vercel frontend
- Check browser console for CORS errors

### 4. Monitor Logs
- Check platform logs for errors
- Verify rate limiting is working

---

## Troubleshooting

### Common Issues

1. **CORS Errors**
   - Verify `ALLOWED_ORIGINS` includes your Vercel domain
   - Check for trailing slashes

2. **Database Connection**
   - Verify connection string format
   - Check firewall/network access
   - Ensure SSL is enabled

3. **Environment Variables**
   - Double-check all required vars are set
   - Verify no typos in variable names

4. **Build Failures**
   - Check .NET version compatibility
   - Verify all NuGet packages are compatible

---

## Security Checklist

- [ ] JWT_KEY is strong and secure (32+ chars)
- [ ] Database password is strong
- [ ] CORS origins are restricted to your domains
- [ ] HTTPS is enforced
- [ ] Rate limiting is active
- [ ] Debug logging is disabled in production
- [ ] Environment variables are not committed to git
- [ ] Database backups are configured

---

## Cost Estimate (Free Tier)

- **Vercel**: Free (Hobby plan)
- **Railway**: $5/month credit (usually free for small apps)
- **Supabase**: Free (500MB)
- **Total**: ~$0-5/month

---

## Next Steps

1. Choose your deployment platform
2. Set up PostgreSQL database
3. Configure environment variables
4. Deploy API
5. Deploy frontend to Vercel
6. Test everything
7. Set up monitoring (optional)

