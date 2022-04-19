using Microsoft.Extensions.Logging;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;

namespace PhotoBank.Data.Repository
{
    public class CategoryRepository :  Repository<Category>, ICategoryRepository
    {
     
        private readonly PbContext _db;

        public CategoryRepository(PbContext db,ILogger<Repository<Category>> logger) : base(db, logger)
        {
        _db=db;
        }

        public Category Find(int id)
        {
            Category category = _db.Categories.Find(id);
            return category;
        }
    }
}
