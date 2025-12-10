using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace MyPortfolio.Api.Services
{
    public class EmailService
    {
        private readonly string _frontendUrl;
        private readonly string? _resendApiKey;
        private readonly string? _resendFromEmail;
        private readonly string _resendFromName;
        private readonly HttpClient _httpClient;

        public EmailService()
        {
            // Get frontend URL from environment or default to localhost
            _frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") 
                ?? "http://localhost:3000";

            // Resend API Configuration
            _resendApiKey = Environment.GetEnvironmentVariable("RESEND_API_KEY");
            _resendFromEmail = Environment.GetEnvironmentVariable("RESEND_FROM_EMAIL");
            _resendFromName = Environment.GetEnvironmentVariable("RESEND_FROM_NAME") ?? "My Portfolio";

            // Setup HttpClient for Resend API
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.resend.com/")
            };

            if (!string.IsNullOrWhiteSpace(_resendApiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_resendApiKey}");
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, string userId, string resetToken)
        {
            // Generate reset link
            var resetLink = $"{_frontendUrl}/auth/reset-password?userId={Uri.EscapeDataString(userId)}&token={Uri.EscapeDataString(resetToken)}";

            // If Resend is configured, send actual email
            if (!string.IsNullOrWhiteSpace(_resendApiKey) && !string.IsNullOrWhiteSpace(_resendFromEmail))
            {
                await SendEmailViaResendAsync(email, resetLink);
            }
            // In development mode without Resend, email is silently skipped
        }

        private async Task SendEmailViaResendAsync(string toEmail, string resetLink)
        {
            try
            {
                var emailRequest = new
                {
                    from = $"{_resendFromName} <{_resendFromEmail}>",
                    to = new[] { toEmail },
                    subject = "Reset Your Password",
                    html = BuildPasswordResetEmailBody(resetLink)
                };

                var response = await _httpClient.PostAsJsonAsync("emails", emailRequest);

                if (!response.IsSuccessStatusCode)
                {
                    // Don't throw - we don't want to reveal email sending failures to users
                }
            }
            catch (Exception ex)
            {
                // Don't throw - we don't want to reveal email sending failures to users
            }
        }

        public async Task SendContactFormEmailAsync(string fromName, string fromEmail, string subject, string message)
        {
            var recipientEmail = Environment.GetEnvironmentVariable("CONTACT_EMAIL") 
                ?? "hello@judahsullivan.com";

            // If Resend is configured, send actual email
            if (!string.IsNullOrWhiteSpace(_resendApiKey) && !string.IsNullOrWhiteSpace(_resendFromEmail))
            {
                await SendContactEmailViaResendAsync(recipientEmail, fromName, fromEmail, subject, message);
            }
            // In development mode without Resend, email is silently skipped
        }

        private async Task SendContactEmailViaResendAsync(string recipientEmail, string fromName, string fromEmail, string subject, string message)
        {
            try
            {
                var emailRequest = new
                {
                    from = $"{_resendFromName} <{_resendFromEmail}>",
                    to = new[] { recipientEmail },
                    replyTo = fromEmail,
                    subject = $"New Contact Form Message: {subject}",
                    html = BuildContactFormEmailBody(fromName, fromEmail, subject, message)
                };

                var response = await _httpClient.PostAsJsonAsync("emails", emailRequest);

                if (!response.IsSuccessStatusCode)
                {
                    // Don't throw - we don't want to reveal email sending failures to users
                }
            }
            catch (Exception ex)
            {
                // Don't throw - we don't want to reveal email sending failures to users
            }
        }

        private string BuildPasswordResetEmailBody(string resetLink)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head><meta charset='utf-8'></head>");
            html.AppendLine("<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>");
            html.AppendLine("<div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; border-radius: 10px 10px 0 0; text-align: center;'>");
            html.AppendLine("<h1 style='color: white; margin: 0; font-size: 28px;'>Reset Your Password</h1>");
            html.AppendLine("</div>");
            html.AppendLine("<div style='background: #ffffff; padding: 30px; border-radius: 0 0 10px 10px; border: 1px solid #e5e7eb;'>");
            html.AppendLine("<p style='font-size: 16px; margin-bottom: 20px;'>You requested to reset your password. Click the button below to create a new password:</p>");
            html.AppendLine($"<div style='text-align: center; margin: 30px 0;'>");
            html.AppendLine($"<a href='{resetLink}' style='display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 14px 28px; text-decoration: none; border-radius: 6px; font-weight: 600; font-size: 16px;'>Reset Password</a>");
            html.AppendLine("</div>");
            html.AppendLine($"<p style='font-size: 14px; color: #6b7280; margin-top: 30px;'>Or copy and paste this link into your browser:</p>");
            html.AppendLine($"<p style='font-size: 12px; color: #9ca3af; word-break: break-all; background: #f9fafb; padding: 12px; border-radius: 6px;'>{resetLink}</p>");
            html.AppendLine("<p style='font-size: 12px; color: #6b7280; margin-top: 20px; padding-top: 20px; border-top: 1px solid #e5e7eb;'>This link will expire in 1 hour. If you didn't request this, please ignore this email.</p>");
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }

        private string BuildContactFormEmailBody(string fromName, string fromEmail, string subject, string message)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head><meta charset='utf-8'></head>");
            html.AppendLine("<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>");
            html.AppendLine("<div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; border-radius: 10px 10px 0 0; text-align: center;'>");
            html.AppendLine("<h1 style='color: white; margin: 0; font-size: 28px;'>New Contact Form Message</h1>");
            html.AppendLine("</div>");
            html.AppendLine("<div style='background: #ffffff; padding: 30px; border-radius: 0 0 10px 10px; border: 1px solid #e5e7eb;'>");
            html.AppendLine("<div style='margin-bottom: 20px;'>");
            html.AppendLine($"<p style='font-size: 14px; color: #6b7280; margin-bottom: 5px;'>From:</p>");
            html.AppendLine($"<p style='font-size: 16px; font-weight: 600; margin: 0;'>{System.Net.WebUtility.HtmlEncode(fromName)}</p>");
            html.AppendLine($"<p style='font-size: 14px; color: #6b7280; margin: 0;'>{System.Net.WebUtility.HtmlEncode(fromEmail)}</p>");
            html.AppendLine("</div>");
            html.AppendLine("<div style='margin-bottom: 20px; padding-top: 20px; border-top: 1px solid #e5e7eb;'>");
            html.AppendLine($"<p style='font-size: 14px; color: #6b7280; margin-bottom: 5px;'>Subject:</p>");
            html.AppendLine($"<p style='font-size: 16px; font-weight: 600; margin: 0;'>{System.Net.WebUtility.HtmlEncode(subject)}</p>");
            html.AppendLine("</div>");
            html.AppendLine("<div style='margin-bottom: 20px; padding-top: 20px; border-top: 1px solid #e5e7eb;'>");
            html.AppendLine($"<p style='font-size: 14px; color: #6b7280; margin-bottom: 10px;'>Message:</p>");
            html.AppendLine($"<div style='background: #f9fafb; padding: 16px; border-radius: 6px; border-left: 4px solid #667eea;'>");
            html.AppendLine($"<p style='font-size: 15px; line-height: 1.8; margin: 0; white-space: pre-wrap;'>{System.Net.WebUtility.HtmlEncode(message)}</p>");
            html.AppendLine("</div>");
            html.AppendLine("</div>");
            html.AppendLine($"<div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e5e7eb; text-align: center;'>");
            html.AppendLine($"<a href='mailto:{System.Net.WebUtility.HtmlEncode(fromEmail)}' style='display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; font-weight: 600; font-size: 14px;'>Reply to {System.Net.WebUtility.HtmlEncode(fromName)}</a>");
            html.AppendLine("</div>");
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }
    }
}
