using System.Net;
using System.Net.Mail;

namespace OnlineEducationWebApp.Data.Services
{
    public class MailService
    {
        private static string fromEmail = "";
        private static string password = "";

        public static void SendMail(List<string> toEmails, string from, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(from);
                    foreach (var toEmail in toEmails)
                    {
                        mail.To.Add(toEmail);
                    }
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.google.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(fromEmail, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}