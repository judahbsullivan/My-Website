#!/bin/bash

# Fly.io Deployment Script
# Run this from the api directory: ./deploy.sh

set -e  # Exit on error

echo "🚀 Deploying to Fly.io..."
echo ""

# Check if fly CLI is installed
if ! command -v fly &> /dev/null; then
    echo "❌ Fly CLI not found. Install it with:"
    echo "   curl -L https://fly.io/install.sh | sh"
    exit 1
fi

# Check if we're in the api directory
if [ ! -f "fly.toml" ]; then
    echo "❌ Error: fly.toml not found. Make sure you're in the api directory."
    exit 1
fi

# Check if logged in
if ! fly auth whoami &> /dev/null; then
    echo "❌ Not logged in to Fly.io. Run: fly auth login"
    exit 1
fi

echo "✅ Fly CLI found and authenticated"
echo ""

# Show current app status
echo "📊 Current app status:"
fly status
echo ""

# Ask if user wants to run migrations
read -p "Do you want to run database migrations after deployment? (y/n): " RUN_MIGRATIONS
echo ""

# Deploy
echo "🔨 Building and deploying..."
fly deploy

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Deployment successful!"
    echo ""
    
    # Show app info
    echo "📊 App information:"
    fly status
    echo ""
    
    # Show logs
    echo "📋 Recent logs (last 20 lines):"
    fly logs --limit 20
    echo ""
    
    # Run migrations if requested
    if [[ $RUN_MIGRATIONS == "y" || $RUN_MIGRATIONS == "Y" ]]; then
        echo "🔄 Running database migrations..."
        echo "Note: Make sure your production database connection is configured in Fly.io secrets"
        read -p "Continue with migrations? (y/n): " CONFIRM_MIGRATE
        
        if [[ $CONFIRM_MIGRATE == "y" || $CONFIRM_MIGRATE == "Y" ]]; then
            # Option 1: Run migrations via SSH
            echo "Running migrations on Fly.io machine..."
            fly ssh console -C "cd /app && dotnet ef database update" || {
                echo "⚠️  Could not run migrations via SSH."
                echo "💡 Alternative: Run migrations locally pointing to production DB:"
                echo "   1. Temporarily set production DB connection in your local environment"
                echo "   2. Run: dotnet ef database update"
            }
        fi
    fi
    
    echo ""
    echo "🌐 Your app is live at:"
    fly info | grep "Hostname" || fly status | grep "Hostname"
    echo ""
    echo "✨ Done! Your changes are now live on Fly.io"
else
    echo ""
    echo "❌ Deployment failed. Check the logs above for errors."
    exit 1
fi
