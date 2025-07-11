namespace Rently.Infrastructure.Services.Messaging.SendGrid
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
