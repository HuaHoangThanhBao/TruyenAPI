using Microsoft.AspNetCore.Identity;

namespace CoreLibrary.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
