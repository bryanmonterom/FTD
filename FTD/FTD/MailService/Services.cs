using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;


namespace MailService
{
    public class Services
    {
        internal const string username = "ftdmailservices1@gmail.com";
        internal const string password = "Ftd2019@";

        public static void MailSender(string subject, string body, string to )
        {
            string htmlBody = "<body><table><th>Prueba<th><tr><td>1<td></tr><tr><td>2</td></tr></table></body>";

            try
            {
             
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body
                };
                mail.To.Add(to);
                SmtpClient smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 20000,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(username, password)
                };
                smtpClient.Send(mail);
                Console.WriteLine("Mail sent");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}




