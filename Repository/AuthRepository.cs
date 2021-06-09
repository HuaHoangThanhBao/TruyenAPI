using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using System.Linq;

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

        public ResponseDetails LogIn(string username, string password)
        {
            var userRepo = new UserRepository(_context);
            if(!userRepo.FindByCondition(m => m.TenUser.Equals(username)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "User không tồn tại",
                    Value = username
                };
            }
            else
            {
                var user = userRepo.FindByCondition(m => m.TenUser == username).First();
                if(!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Mật khẩu không khớp",
                        Value = username
                    };
                }
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Đăng nhập thành công" };
        }
    }
}
