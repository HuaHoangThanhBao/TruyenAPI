using CoreLibrary;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Helpers;
using CoreLibrary.Models;
using DataAccessLayer;
using Repository.Extensions;
using System.Linq;
using System.Text.RegularExpressions;

namespace Repository
{
    public class AuthRepository: RepositoryBase<LoginModel>, IAuthRepository
    {
        private RepositoryContext _context;

        public AuthRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public AuthResponseDto ValidateRegistration(UserForRegistrationDto user)
        {
            if(user.Quyen.ToString() == "")
                return new AuthResponseDto { Message = "Vui lòng cung cấp quyền cho user" };

            if (user.Quyen != Data.UserRole && user.Quyen != Data.EditorRole && user.Quyen != Data.AdminRole)
                return new AuthResponseDto { Message = "Quyền của user không hợp lệ" };

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasNoneAlpha = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (user.LastName.Length < Data.MinLength || user.LastName.Length > Data.NameRequiredMaxLength)
                return new AuthResponseDto{ Message = $"Họ phải có độ dài trong khoảng từ {Data.MinLength} - {Data.NameRequiredMaxLength} ký tự" };

            if (hasNoneAlpha.IsMatch(user.LastName))
                return new AuthResponseDto { Message = "Họ không được chứa ký tự đặc biệt" };

            if (user.FirstName.Length < Data.MinLength || user.FirstName.Length > Data.NameRequiredMaxLength)
                return new AuthResponseDto{ Message =  $"Tên phải có độ dài trong khoảng từ {Data.MinLength} - {Data.NameRequiredMaxLength} ký tự" };

            if (hasNoneAlpha.IsMatch(user.FirstName))
                return new AuthResponseDto { Message = "Tên không được chứa ký tự đặc biệt" };

            if (user.Email.Length < Data.MinLength || user.Email.Length > Data.EmailRequiredMaxLength)
                return new AuthResponseDto{ Message = $"Email phải có độ dài trong khoảng từ {Data.MinLength} - {Data.EmailRequiredMaxLength} ký tự" };

            if (user.Password.Length < Data.PassRequiredMinLength || user.Password.Length > Data.PassRequiredMaxLength)
                return new AuthResponseDto{ Message =  $"Mật khẩu phải có độ dài trong khoảng từ {Data.PassRequiredMinLength} - {Data.PassRequiredMaxLength} ký tự"};

            if (user.Password != user.ConfirmPassword)
                return new AuthResponseDto { Message = "Mật khẩu xác nhận không khớp!" };

            if (!hasNumber.IsMatch(user.Password))
                return new AuthResponseDto { Message = "Mật khẩu phải có ít nhất 1 chữ số (0-9)" };

            if (!hasUpperChar.IsMatch(user.Password))
                return new AuthResponseDto { Message = "Mật khẩu phải có ít nhất 1 ký tự in hoa (A-Z)" };

            if (!hasNoneAlpha.IsMatch(user.Password))
                return new AuthResponseDto { Message = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt" };

            return null;
        }

        public AuthResponseDto ValidateLogin(UserForAuthenticationDto user)
        {
            if (user.Email.Length < Data.MinLength || user.Email.Length > Data.EmailRequiredMaxLength)
                return new AuthResponseDto { Message = $"Email phải có độ dài trong khoảng từ {Data.MinLength} - {Data.EmailRequiredMaxLength} ký tự" };

            if (user.Password.Length < Data.PassRequiredMinLength || user.Password.Length > Data.PassRequiredMaxLength)
                return new AuthResponseDto{ Message =  $"Mật khẩu phải có độ dài trong khoảng từ {Data.PassRequiredMinLength} - {Data.PassRequiredMaxLength} ký tự" };

            return null;
        }

        public AuthResponseDto ValidateResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto.Password.Length < Data.PassRequiredMinLength || resetPasswordDto.Password.Length > Data.PassRequiredMaxLength)
                return new AuthResponseDto { Message = $"Mật khẩu phải có độ dài trong khoảng từ {Data.PassRequiredMinLength} - {Data.PassRequiredMaxLength} ký tự" };

            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                return new AuthResponseDto { Message = "Mật khẩu xác nhận không khớp!" };

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasNoneAlpha = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasNumber.IsMatch(resetPasswordDto.Password))
                return new AuthResponseDto { Message = "Mật khẩu phải có ít nhất 1 chữ số (0-9)" };

            if (!hasUpperChar.IsMatch(resetPasswordDto.Password))
                return new AuthResponseDto { Message = "Mật khẩu phải có ít nhất 1 ký tự in hoa (A-Z)" };

            if (!hasNoneAlpha.IsMatch(resetPasswordDto.Password))
                return new AuthResponseDto { Message = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt" };

            return null;
        }

        public AuthResponseDto ValidateUpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            if (updatePasswordDto.OldPassword.Length < Data.PassRequiredMinLength || updatePasswordDto.OldPassword.Length > Data.PassRequiredMaxLength)
                return new AuthResponseDto { Message = $"Mật khẩu cũ có độ dài trong khoảng từ {Data.PassRequiredMinLength} - {Data.PassRequiredMaxLength} ký tự" };

            if (updatePasswordDto.Password.Length < Data.PassRequiredMinLength || updatePasswordDto.Password.Length > Data.PassRequiredMaxLength)
                return new AuthResponseDto { Message = $"Mật khẩu phải mới có độ dài trong khoảng từ {Data.PassRequiredMinLength} - {Data.PassRequiredMaxLength} ký tự" };

            if (updatePasswordDto.Password != updatePasswordDto.ConfirmPassword)
                return new AuthResponseDto { Message = "Mật khẩu xác nhận không khớp!" };

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasNoneAlpha = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasNumber.IsMatch(updatePasswordDto.Password))
                return new AuthResponseDto { Message = "Mật khẩu mới phải có ít nhất 1 chữ số (0-9)" };

            if (!hasUpperChar.IsMatch(updatePasswordDto.Password))
                return new AuthResponseDto { Message = "Mật khẩu mới phải có ít nhất 1 ký tự in hoa (A-Z)" };

            if (!hasNoneAlpha.IsMatch(updatePasswordDto.Password))
                return new AuthResponseDto { Message = "Mật khẩu mới phải có ít nhất 1 ký tự đặc biệt" };

            return null;
        }
    }
}
