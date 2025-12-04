# Fly.io Setup Instructions

The web interface may have issues with monorepos. Use the CLI instead:

## Step 1: Install Fly CLI

```bash
# macOS
curl -L https://fly.io/install.sh | sh

# Or with Homebrew
brew install flyctl
```

## Step 2: Login

```bash
fly auth login
```

## Step 3: Initialize from the api directory

```bash
cd api
fly launch
```

When prompted:
- **App name**: Choose a name (e.g., `portfolio-api` or `my-website-bcxqiw`)
- **Region**: Choose closest to you (e.g., `iad` for US East)
- **PostgreSQL**: Skip (use external database)
- **Redis**: Skip

This will detect your existing `fly.toml` and `Dockerfile`.

## Step 4: Set Environment Variables

```bash
# Database (from Supabase/Neon)
fly secrets set DB_HOST=your-host
fly secrets set DB_NAME=your-db-name
fly secrets set DB_USER=your-user
fly secrets set DB_PASSWORD=your-password

# JWT (generate with: openssl rand -base64 32)
fly secrets set JWT_KEY=your-generated-key
fly secrets set JWT_ISSUER=MyPortfolio
fly secrets set JWT_AUDIENCE=MyPortfolioUsers
fly secrets set JWT_EXPIRE_MINUTES=60

# CORS
fly secrets set ALLOWED_ORIGINS=https://your-vercel-app.vercel.app

# Optional: Email
fly secrets set RESEND_API_KEY=your-key
fly secrets set RESEND_FROM_EMAIL=noreply@yourdomain.com
fly secrets set RESEND_FROM_NAME="Judah Sullivan"
fly secrets set FRONTEND_URL=https://yourdomain.com
fly secrets set CONTACT_EMAIL=hello@judahsullivan.com
```

## Step 5: Deploy

```bash
fly deploy
```

## Alternative: If Web Interface is Required

If you must use the web interface, try:

1. **Current Working Directory**: `api`
2. **Config path**: `api/fly.toml` (try this if just `api` doesn't work)

But CLI is recommended for monorepos!

