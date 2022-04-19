using Microsoft.Extensions.Logging;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;

namespace PhotoBank.Data.Repository
{
    public class CartRepository: Repository<Cart>, ICartRepository
    {
        private readonly PbContext _db;
        private readonly ILogger<Repository<Cart>> _logger;

        public CartRepository(PbContext db,ILogger<Repository<Cart>> logger) : base(db, logger)
        {
            _db=db;
            _logger = logger;
        }

        public Cart Find(int id)
        {
            Cart cart = _db.Carts.Find(id); 
            return cart;
        }
    }
}
