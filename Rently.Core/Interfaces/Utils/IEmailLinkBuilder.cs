namespace Rently.Core.Interfaces.Utils
{
    public interface IEmailLinkBuilder
    {
        string BuildEmailConfirmationLink(string userId, string token);
    }
}
