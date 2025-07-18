﻿namespace Rently.Core.Interfaces.Messaging
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
