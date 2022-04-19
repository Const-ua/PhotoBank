using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;
using System.Diagnostics;

namespace PhotoBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoRepository _photoRepo;
        private readonly ICartRepository _cartRepo;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, 
                                IPhotoRepository photoRepo, ICartRepository cartRepo,
                                UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _cartRepo= cartRepo;
            _photoRepo = photoRepo;
            _userManager = userManager;
            _roleManager= roleManager;
            ClearCart();
        }

        public IActionResult Index(int categoryId)
        {
            IEnumerable<Photo> photos = new List<Photo>();
            if (categoryId == null || categoryId == 0)
            {
                photos = _photoRepo.GetAll(
                    includeProperties: "Author,Category");
            }
            else
            {
               photos = _photoRepo.GetAll(includeProperties:"Author, Category", filter: r=>r.CategoryId==categoryId);
            }
            return View(photos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ClearCart()
        {
            DateTime dt = DateTime.Now.AddHours(WebConstants.StoreGoodsInCartHours*-1);
            IEnumerable<Cart> carts = _cartRepo.GetAll(filter: r => r.CreatedDate < dt);  
            foreach (var cart in carts)
            {
                _cartRepo.Remove(cart);
            }

            _cartRepo.Save();
            return;
        }
    }
}