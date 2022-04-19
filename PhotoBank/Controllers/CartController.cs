using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;
using System.Net;
using System.Net.Mail;

namespace PhotoBank.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CartController(ICartRepository cartRepo, 
                                IUserRepository userRepo,
                                UserManager<User> userManager, 
                                IWebHostEnvironment webHostEnvironment)
        {
            _cartRepo=cartRepo;
            _userRepo=userRepo;
            _userManager=userManager;
            _webHostEnvironment=webHostEnvironment;
        }
        // GET: CartController
        public ActionResult Index()
        {
            User user = _userRepo.FirstOrDefault(r => r.Email == _userManager.GetUserName(User));
            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<Cart> cart = _cartRepo.GetAll(filter: r => r.UserId == user.Id, includeProperties: "Photo"); 
            return View(cart);
        }

        [HttpPost]
        public int AddToCart(Guid id)
        {
            string userId = _userManager.GetUserId(User);
            if (String.IsNullOrEmpty(userId))
            {
                return -1;
            }
            Cart cart = _cartRepo.FirstOrDefault(r => r.PhotoId == id && r.UserId == userId);
            if (cart == null)
            {
                cart = new Cart{PhotoId = id, UserId = userId,CreatedDate = DateTime.Now};
                if (_cartRepo.Add(cart))
                {
                    _cartRepo.Save();
                }
                else
                {
                    return -1;
                }
            }
            int itemsCounter = _cartRepo.GetAll(r => r.UserId == userId).Count();
            return itemsCounter;
        }

        
        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            Cart cart = _cartRepo.Find(id);
            if (cart == null)
            {
                return NotFound();
            }
            if (_cartRepo.Remove(cart))
            {
                _cartRepo.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public bool SendMail()
        {
            User user = _userRepo.FirstOrDefault(r => r.Email == _userManager.GetUserName(User));
            if (user == null)
            {
                return false;
            }

            IEnumerable<Cart> cart = _cartRepo.GetAll(includeProperties: "Photo", filter: r => r.UserId == user.Id); 
            
            MailAddress from = new MailAddress("rudenkoconst@gmail.com", "PhotoBank");
            MailAddress to = new MailAddress(user.Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Photos from PhotoBank";
            m.Body = "<h2>Photos from PhotoBank</h2>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("rudenkoconst@gmail.com", "irchngfnhnncavds");
            smtp.EnableSsl = true;
            int count=0;
            int mailCount = 0;
            string rootPath = _webHostEnvironment.WebRootPath;

            try
            {
                foreach (Cart item in cart)
                {
                
                    if (count == WebConstants.MaxAttachments)
                    {
                        smtp.Send(m);
                        m.Attachments.Clear();
                        mailCount++;
                        m.Subject = "Photos from PhotoBank ("+mailCount.ToString()+")";
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }
                    m.Attachments.Add(new Attachment(rootPath+WebConstants.PhotoPath+item.Photo.File));
                }
                smtp.Send(m);

                foreach (var item in cart)
                {
                    _cartRepo.Remove(item);
                }
                _cartRepo.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public ActionResult OrderCompleted()
        {
            return View("OrderCompleated");
        }
    }
}
