# Fly.io Deployment Guide

Quick guide to deploy backend changes to Fly.io.

## Quick Deploy

### Option 1: Use the deployment script (Recommended)

```bash
cd api
./deploy.sh
```

This script will:
- Check if Fly CLI is installed and you're logged in
- Show current app status
- Build and deploy your changes
- Optionally run database migrations
- Show logs and app URL

### Option 2: Manual deployment

```bash
cd api
fly deploy
```

## Step-by-Step Process

### 1. Make your changes
Edit your code in the `api/` directory.

### 2. Test locally (optional but recommended)
```bash
cd api
dotnet run
```

### 3. Deploy to Fly.io
```bash
cd api
fly deploy
```

This will:
- Build your Docker image
- Push to Fly.io registry
- Deploy to your app
- Restart the app with new changes

### 4. Verify deployment
```bash
# Check app status
fly status

# View logs
fly logs

# Open app in browser
fly open
```

## Running Database Migrations

After deploying code changes, you may need to run database migrations:

### Option 1: Run migrations via SSH (Recommended)
```bash
fly ssh console -C "cd /app && dotnet ef database update"
```

### Option 2: Run migrations locally (pointing to production DB)
1. Temporarily set production database connection in your local environment
2. Run:
```bash
cd api
dotnet ef database update
```

## Common Commands

### View logs
```bash
fly logs
fly logs --limit 50  # Last 50 lines
```

### Check app status
```bash
fly status
```

### Restart app
```bash
fly apps restart your-portfolio-api-dry-feather-6260
```

### Update environment variables
```bash
fly secrets set KEY=value
```

### View all secrets
```bash
fly secrets list
```

### SSH into the machine
```bash
fly ssh console
```

### Scale your app
```bash
fly scale count 2  # Scale to 2 instances
fly scale vm shared-cpu-1x --memory 512  # Change memory
```

## Troubleshooting

### Deployment fails
1. Check logs: `fly logs`
2. Verify Dockerfile is correct
3. Check if all dependencies are in `api.csproj`
4. Ensure `fly.toml` is configured correctly

### App not starting
1. Check logs: `fly logs`
2. Verify environment variables: `fly secrets list`
3. Check database connection
4. Verify port 8080 is exposed correctly

### Database connection issues
1. Verify secrets are set: `fly secrets list`
2. Check database is accessible from Fly.io
3. Verify connection string format

## CI/CD Integration (Optional)

You can set up GitHub Actions to auto-deploy on push:

```yaml
# .github/workflows/deploy-fly.yml
name: Deploy to Fly.io

on:
  push:
    branches: [main]
    paths:
      - 'api/**'

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: superfly/flyctl-actions/setup-flyctl@master
      - run: flyctl deploy --remote-only
        working-directory: ./api
        env:
          FLY_API_TOKEN: ${{ secrets.FLY_API_TOKEN }}
```

## Notes

- **Build time**: First deployment takes longer (~5-10 min), subsequent deployments are faster
- **Zero downtime**: Fly.io performs rolling deployments by default
- **Auto-restart**: App automatically restarts when secrets are updated
- **Logs**: Logs are available for 3 days on free tier
