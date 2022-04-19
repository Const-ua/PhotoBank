using PhotoBank.Models;

namespace PhotoBank.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category> 
    {
        Category Find(int id);
    }
}
