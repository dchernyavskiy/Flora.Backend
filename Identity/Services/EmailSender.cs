using System.Net;
using System.Net.Mail;
using Flora.Identity.Interfaces;


namespace Flora.Identity.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendEmailAsync(string message, string receiverFullName, string receiverEmail,
        string subject)
    {
        var from = _configuration["Smtp:From"];
        var password = _configuration["Smtp:Password"];
        var host = _configuration["Smtp:Host"];

        var client = new SmtpClient(host, 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(from, password)
        };

        var mail = new MailMessage(from, receiverEmail, subject, message)
        {
            IsBodyHtml = true,
        };
        
        try
        {
            client.Send(mail);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }

        // var domain = _configuration["Mailgun:Domain"]!;
        // var from = _configuration["Mailgun:From"]!;
        // var password = _configuration["Mailgun:Password"]!;
        //
        // var client = new RestClient("https://api.mailgun.net/v3",
        //     options =>
        //     {
        //         options.Authenticator = new HttpBasicAuthenticator("api", password);
        //     });
        //
        // var request = new RestRequest();
        // request.AddParameter("domain", domain, ParameterType.UrlSegment);
        // request.Resource = "{domain}/messages";
        // request.AddParameter("from", from);
        // request.AddParameter("to", $"{receiverFullName} <{receiverEmail}>");
        // request.AddParameter("subject", subject);
        // request.AddParameter("html", message);
        // request.AddParameter("o:tracking", false);
        // request.Method = Method.Post;
        //
        // var result = await client.ExecuteAsync(request);
        //
        // return result.IsSuccessful;
    }
}