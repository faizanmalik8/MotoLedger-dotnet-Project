using Microsoft.AspNetCore.Identity;

namespace MotoLedger.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
