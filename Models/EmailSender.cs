using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
/*
 * Caleb Edwards
 * u0829971
 * Used to verify the email address from a new registered user
 */

using Microsoft.AspNetCore.Identity.UI.Services;

namespace Learning_Outcomes.Models
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var task = new Task(() =>
            {
                SmtpClient smtpClient = new SmtpClient()
                {
                    Host = "smtp.utah.edu",
                    Port = 25,
                    EnableSsl = true,
                    //DeliveryMethod = SmtpDeliveryMethod.Network,
                    //UseDefaultCredentials = false
                    // For credentials need umail and password for this to work
                    Credentials = new NetworkCredential("umail.utah.edu", "password")
            };


                MailMessage mail = new MailMessage
                {
                    // WARNING: what email are you going to send to? germain@cs.utah.edu?
                    From = new MailAddress("u0829971@utah.edu", "LOT")
                };
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject; mail.Body = message; mail.IsBodyHtml = true;
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                                ex.ToString());
                }
            }); task.Start(); return task;
        }
    }
}
