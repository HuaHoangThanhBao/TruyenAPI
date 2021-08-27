using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class UserForAuthenticationDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string clientURI { get; set; }
    }
}
