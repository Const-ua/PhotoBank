using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;
using PhotoBank.ViewModels;

namespace PhotoBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepo;
        private static UserManager<User> _userManager;
        private static SignInManager<User> _signInManager;

        public AccountController(IUserRepository userRepo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index() => View(_userRepo.GetAll());

        ////////////Create
        public ActionResult Create()
        {

            User model = new User();// { IsAuthor = true };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        //Email = model.Email,
                        Phone = model.Phone,
                        IsAuthor = model.IsAuthor
                    };
                    _userRepo.Add(user);
                    _userRepo.Save();
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View(model);
                }
            }

            return View(model);
        }

        ////////////// Edit
        public ActionResult Edit(string id)
        {
            User user = _userRepo.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View("Create", user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepo.Find(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    if (_userRepo.Update(user))
                    {
                        _userRepo.Save();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Create", model);
                    }
                }
            }

            return View("Create", model);
        }
        ////////////////Delete
        public ActionResult Delete(string id)
        {
            User user = _userRepo.Find(id);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(User model)
        {
            User user = _userRepo.Find(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (_userRepo.Remove(user))
            {
                _userRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Register()
        {
            RegisterVM model = new RegisterVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Phone = model.Phone,
                    IsAuthor = model.IsAuthor
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.IsAuthor)
                    {
                        await _userManager.AddToRoleAsync(user, WebConstants.AuthorRole);
                    }
                    var r = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                    if (r.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            LoginVM model = new LoginVM { RememberMe = true };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            User user = _userRepo.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordVM model = new ChangePasswordVM { Id = user.Id };
            ViewBag.Errors = new List<string>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            ViewBag.Errors = new List<string>();
            if (ModelState.IsValid)
            {
                User user = _userRepo.Find(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ViewBag.Errors.Add(error.Description);
                }
            }

            return View(model);
        }

        public ActionResult AccessDenied(string returnUrl)
        {

            return View("AccessDenied", returnUrl);
        }
    }
}
