using MimeKit.Text;
using MimeKit;
using ModelLayer;
using System;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace RepositoryLayer.Utility
{
    public class EmailService
    {
        public static void SendEmail(EmailML model, IConfiguration config)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(config.GetSection("EmailSettings:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(model.To));
            email.Subject = model.Subject;
            //email.Body = new TextPart(TextFormat.Html)
            //{
            //    Text = model.Body
            //};
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                        <html>
                        <head>
                          <style>
                            .container {{
                              font-family: Arial, sans-serif;
                              margin: 0 auto;
                              padding: 20px;
                              max-width: 600px;
                              border: 1px solid #ddd;
                              border-radius: 5px;
                              background-color: #f9f9f9;
                            }}
                            .header {{
                              text-align: center;
                              padding-bottom: 20px;
                            }}
                            .content {{
                              text-align: center;
                            }}
                            .button {{
                              display: inline-block;
                              margin-top: 20px;
                              padding: 10px 20px;
                              background-color: #4CAF50;
                              color: white;
                              text-decoration: none;
                              border-radius: 5px;
                            }}
                          </style>
                        </head>
                        <body>
                          <div class='container'>
                            <div class='header'>
                              <h2>Password Reset Request</h2>
                            </div>
                            <div class='content'>
                              <p>Hello,</p>
                              <p>We received a request to reset the password for your account. If you did not make this request, you can ignore this email.</p>
                              <p>To reset your password, please click the button below:</p>
                              <a href='{model.Body}' class='button'>Reset Password</a>
                              <p>If the button above does not work, copy and paste the following link into your browser:</p>
                              <p><a href='{model.Body}'>{model.Body}</a></p>
                              <p>Thank you,<br/>The FundooNotes Team</p>
                            </div>
                          </div>
                        </body>
                        </html>"
                };


            using var smtp = new SmtpClient();
            smtp.Connect(config.GetSection("EmailSettings:EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("EmailSettings:EmailUsername").Value, config.GetSection("EmailSettings:EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
