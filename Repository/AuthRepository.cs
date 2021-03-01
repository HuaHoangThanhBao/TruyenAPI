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
            if(!userRepo.FindByCondition(m => m.TenUser.Equals(username) && m.Password.Equals(password)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "User không tồn tại",
                    Value = username
                };
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Đăng nhập thành công" };
        }
    }
}
