using PhotoBank.Models;

namespace PhotoBank.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        User Find(string Id);
    }
}
