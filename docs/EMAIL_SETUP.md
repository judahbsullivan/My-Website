# Email Setup for Password Reset

## Current Status

The password reset functionality is implemented but currently logs reset links to the console in development mode. To enable actual email sending, you need to configure SMTP settings.

## Quick Setup

### Option 1: Gmail SMTP (Easiest for Testing)

1. Enable 2-Factor Authentication on your Gmail account
2. Generate an App Password:
   - Go to Google Account → Security → 2-Step Verification → App passwords
   - Create an app password for "Mail"
3. Add these to your `.env` file:

```env
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=your-email@gmail.com
SMTP_PASSWORD=your-16-char-app-password
SMTP_FROM_EMAIL=your-email@gmail.com
SMTP_FROM_NAME=Your Portfolio Name
FRONTEND_URL=http://localhost:3000
```

### Option 2: SendGrid (Recommended for Production)

1. Sign up at [SendGrid](https://sendgrid.com)
2. Create an API key
3. Add to `.env`:

```env
SENDGRID_API_KEY=your-api-key
SENDGRID_FROM_EMAIL=noreply@yourdomain.com
SENDGRID_FROM_NAME=Your Portfolio Name
FRONTEND_URL=https://yourdomain.com
```

Then update `EmailService.cs` to use SendGrid SDK instead of SMTP.

### Option 3: Other Email Services

- **Mailgun**: Similar to SendGrid, API-based
- **AWS SES**: Good for high volume, requires AWS account
- **SMTP Server**: Any SMTP server (Outlook, custom, etc.)

## Environment Variables

Add these to your `.env` file in the project root:

```env
# Frontend URL (for reset links)
FRONTEND_URL=http://localhost:3000

# SMTP Configuration (for basic SMTP)
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=your-email@example.com
SMTP_PASSWORD=your-password
SMTP_FROM_EMAIL=noreply@yourdomain.com
SMTP_FROM_NAME=My Portfolio
```

## Testing

1. Start your API server
2. Request a password reset
3. Check:
   - **Development**: Console output shows the reset link
   - **Production**: Email inbox (and spam folder) for the reset email

## Security Notes

- Never commit `.env` file to git
- Use App Passwords for Gmail (not your main password)
- In production, use a dedicated email service (SendGrid, Mailgun, etc.)
- Reset tokens expire after 1 hour (configurable in ASP.NET Identity)

