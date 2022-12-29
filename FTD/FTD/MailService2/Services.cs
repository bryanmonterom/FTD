using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;


namespace MailService2
{
    public static class Services
    {
        internal const string username = "********.com";
        internal const string password = "*************@";

        public static Result  MailSender(string subject, string body, string to)
        {

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
                    Timeout = Int32.MaxValue,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(username, password)
                };
                smtpClient.Send(mail);
                return new Result()
                {
                    IsSuccess = true,
                    Message = "Mail Sent"
                };
            }
            catch (Exception ex)
            {
                var msg = "";
                if (ex.InnerException != null)
                {
                    msg = ex.InnerException.Message;
                }
                return new Result()
                {
                    IsSuccess = false,
                    Message = ex.Message + " "+msg
                };
            }
        }
    }
}