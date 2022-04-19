
using PhotoBank.Models;

namespace PhotoBank.Data.Repository.IRepository
{
    public interface ICartRepository : IRepository<Cart> 
    {
        Cart Find(int id);
    }
}
