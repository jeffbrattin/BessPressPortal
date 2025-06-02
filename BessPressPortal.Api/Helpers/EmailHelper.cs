using System.Diagnostics;

namespace BessPressPortal.Api.Helpers
{
    public static class EmailHelper
    {
        public static string? ExtractDomainFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var parts = email.Split('@');
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1]))
                return null;

            return parts[1].Trim().ToLowerInvariant();
        }
    }
}
