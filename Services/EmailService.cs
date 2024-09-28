using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;

    public EmailService(string smtpHost, int smtpPort, string smtpUser, string smtpPass)
    {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _smtpUser = smtpUser;
        _smtpPass = smtpPass;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage();
        message.To.Add(new MailAddress(toEmail));  // L'email du destinataire
        message.From = new MailAddress(_smtpUser); // L'email de l'expéditeur
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(message);
        }
    }
}
