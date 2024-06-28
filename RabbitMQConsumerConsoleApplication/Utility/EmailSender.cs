using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumerConsoleApplication.Utility
{
    public class EmailSender
    {
        public static void SendEmail(EmailML model)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("vikrantgawale123@gmail.com"));
            email.To.Add(MailboxAddress.Parse(model.To));
            email.Subject = model.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = model.Body
            };
 
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("vikrantgawale123@gmail.com", "uzmi anqp fweq uxuf");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
