using Microsoft.AspNetCore.Identity;

namespace BessPressPortal.Shared.Models.Authentication
{
    public class UserInfo : IdentityUser<int> // Make sure it's int-based!
    {
        public string Role { get; set; } = string.Empty;
    }
}