using Academics.Models;
using System.Net.Mail;
using System.Net;

public class EmailSender : IMessage
{
    public bool SendMessage(string to, string subject, string message)
    {
        var fromAddress = new MailAddress("zhumanovahat00@gmail.com", "From Name");
        var toAddress = new MailAddress(to, "To Name");
        const string fromPassword = "tzvl bomn yhrh zjww";

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };
        using (var result = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = message
        })
        {
            try
            {
                smtp.Send(result);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}