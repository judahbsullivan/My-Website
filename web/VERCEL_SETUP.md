# Vercel Frontend Setup

## Environment Variables

Your frontend needs to know where the API is located. Set this in Vercel:

### Required Environment Variable

1. Go to your Vercel project settings
2. Navigate to **Settings** → **Environment Variables**
3. Add the following:

```
NUXT_PUBLIC_API_BASE=https://judahsullivan-dev-api.fly.dev
```

### Setting in Vercel Dashboard

1. **Project Settings** → **Environment Variables**
2. **Add New**:
   - **Name**: `NUXT_PUBLIC_API_BASE`
   - **Value**: `https://judahsullivan-dev-api.fly.dev`
   - **Environment**: Select all (Production, Preview, Development)

3. **Redeploy** your application after adding the variable

### Alternative: Using Vercel CLI

```bash
vercel env add NUXT_PUBLIC_API_BASE
# Enter: https://judahsullivan-dev-api.fly.dev
# Select environments: Production, Preview, Development
```

Then redeploy:
```bash
vercel --prod
```

## Verify Configuration

After deploying, check that the frontend is using the correct API:

1. Open browser DevTools (F12)
2. Go to **Network** tab
3. Navigate to a page that loads data (e.g., Projects, Blog)
4. Look for requests to `judahsullivan-dev-api.fly.dev`
5. Verify responses are coming back successfully

## CORS Configuration

The API is configured to allow requests from:
- `https://judahsullivan.dev` (default)
- Any domain set in the `ALLOWED_ORIGINS` environment variable on Fly.io

If your Vercel domain is different (e.g., `*.vercel.app`), update the Fly.io secret:

```bash
fly secrets set ALLOWED_ORIGINS=https://judahsullivan.dev,https://your-app.vercel.app
fly apps restart your-portfolio-api-dry-feather-6260
```

## Current API Status

✅ API is live at: `https://judahsullivan-dev-api.fly.dev`
✅ Health endpoint: `/weatherforecast` (working)
✅ Projects endpoint: `/api/projects` (returning data)
✅ CORS configured for: `https://judahsullivan.dev`
