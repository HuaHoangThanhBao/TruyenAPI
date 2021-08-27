using System.ComponentModel;

namespace CoreLibrary.DataTransferObjects
{
    public class UserForRegistrationDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }

        public string ConfirmPassword { get; set; }

        public string ClientURI { get; set; }
    }
}
