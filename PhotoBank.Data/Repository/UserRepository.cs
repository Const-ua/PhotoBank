using Microsoft.Extensions.Logging;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;

namespace PhotoBank.Data.Repository
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        private readonly PbContext _db;
        private readonly ILogger<Repository<User>> _logger;

        public UserRepository(PbContext db, ILogger<Repository<User>> logger) : base(db, logger)
        {
            _db=db;
            _logger = logger;
        }

        public User Find(string id)
        {
            User user = _db.Users.Find(id);
            return user;
        }
    }
}
