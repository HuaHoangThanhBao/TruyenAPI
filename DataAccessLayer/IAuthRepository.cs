using CoreLibrary.Models;

namespace DataAccessLayer
{
    public interface IAuthRepository
    {
        ResponseDetails LogIn(string username, string password);
    }
}
