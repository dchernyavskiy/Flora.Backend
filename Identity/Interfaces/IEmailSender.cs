namespace Flora.Identity.Interfaces;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(string message, string receiverFullName, string receiverEmail, string subject);
}