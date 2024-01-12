using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Notification
{
    public static class EmailSender
    {
        public static async Task<bool> SendEmail(string Subject, string Body, string recipientEmail)
        {
            string senderMail = "TrainingAdmin@ceridian.com";

            var smtpClient = new SmtpClient("relay.ceridian.com")
            {
                Port = 25,
                EnableSsl = true,
                UseDefaultCredentials = true
            };

            var mailMessage = new MailMessage(senderMail, recipientEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            };

            try
            {
                Task.Run(async () => { smtpClient.SendMailAsync(mailMessage); });
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
    }
}
