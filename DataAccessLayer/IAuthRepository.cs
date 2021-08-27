using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;

namespace DataAccessLayer
{
    public interface IAuthRepository
    {
        AuthResponseDto ValidateRegistration(UserForRegistrationDto userForRegistration); 
        AuthResponseDto ValidateLogin(UserForAuthenticationDto userForAuthentication);
        AuthResponseDto ValidateResetPassword(ResetPasswordDto resetPasswordDto);
        AuthResponseDto ValidateUpdatePassword(UpdatePasswordDto updatePasswordDto);
    }
}
