
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;
using PhotoBank.ViewModels;
using System.Drawing;
using System.Drawing.Imaging;

namespace PhotoBank.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotoController(IPhotoRepository photoRepo,
                                IUserRepository userRepo,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<User> userManager)
        {
            _photoRepo = photoRepo;
            _userRepo = userRepo;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Author")]
        public async Task<ActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            User user = _userRepo.Find(userId);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            IEnumerable<Photo> photos = new List<Photo>();
            if (userRoles.Contains(WebConstants.AuthorRole))
            {
                photos = _photoRepo.GetAll(filter: r => r.AuthorId == userId,
                                includeProperties: "Author",
                                orderBy: r => r.OrderByDescending(r => r.Published));
            }
            else
            {
                photos = _photoRepo.GetAll(includeProperties: "Author",
                    orderBy: r =>
                        r.OrderByDescending(r =>
                            r.Published));
            }
            return View(photos);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpGet]
        public ActionResult Create()
        {
            PhotoVM model = new PhotoVM()
            {
                Photo = new Photo(),
                Categories = _photoRepo.GetCategoryList()
            };
            return View(model);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhotoVM model)
        {
            bool newPhoto = String.IsNullOrEmpty(model.Photo.Id.ToString()) ||
                            model.Photo.Id.ToString() == "00000000-0000-0000-0000-000000000000";
            if (model != null)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Any())
                {
                    string uploadDirectory = _webHostEnvironment.WebRootPath + WebConstants.PhotoPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    string previewDirectory = _webHostEnvironment.WebRootPath + WebConstants.PreviewPath;
                    using (var stream =
                           new FileStream(Path.Combine(uploadDirectory, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(stream);
                        var image = Image.FromStream(stream);
                        var orientation = image.GetPropertyItem(274).Value[0]; //Orientation
                        switch (orientation)
                        {
                            case 1:
                                // No rotation required.
                                break;
                            case 2:
                                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case 3:
                                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 4:
                                image.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case 5:
                                image.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;
                            case 6:
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case 7:
                                image.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case 8:
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                        }

                        //Create preview
                        double rate = new double();
                        Bitmap resized;

                        if (image.Size.Width > image.Size.Height)
                        {
                            rate = (double)image.Size.Width / (double)image.Size.Height;
                            resized = new Bitmap(image, new Size(WebConstants.PreviewResolution, (int)(WebConstants.PreviewResolution / rate)));
                        }
                        else
                        {
                            rate = (double)image.Size.Height / (double)image.Size.Width;
                            resized = new Bitmap(image, new Size((int)(WebConstants.PreviewResolution / rate), WebConstants.PreviewResolution));
                        }


                        using var imageStream = new MemoryStream();
                        resized.Save(Path.Combine(previewDirectory, fileName + extension), ImageFormat.Jpeg);
                    }

                    model.Photo.File = fileName + extension;
                    model.Photo.Preview = fileName + extension;
                    model.Photo.AuthorId = _userManager.GetUserId(User);
                }

                if (newPhoto)
                {
                    //add photo
                    _photoRepo.Add(model.Photo);
                }
                else
                {
                    //Update photo
                    Photo photo = _photoRepo.Find(model.Photo.Id);
                    if (photo == null)
                    {
                        return NotFound();
                    }
                    photo.Name = model.Photo.Name;
                    photo.Description = model.Photo.Description;
                    photo.CategoryId = model.Photo.CategoryId;
                    model.Photo.AuthorId = _userManager.GetUserId(User);
                    if (files.Any())
                    {
                        photo.File = model.Photo.File;
                        photo.Preview = model.Photo.Preview;
                    }
                    _photoRepo.Update(photo);
                }
                _photoRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            Photo photo = _photoRepo.Find(Guid.Parse(id));
            if (photo == null)
            {
                return NotFound();
            }
            PhotoVM model = new PhotoVM()
            {
                Photo = photo,
                Categories = _photoRepo.GetCategoryList()
            };
            return View("Create", model);
        }

        public ActionResult Delete(string id)
        {
            Photo photo = _photoRepo.Find(Guid.Parse(id));
            if (photo == null)
            {
                return NotFound();
            }

            PhotoVM model = new PhotoVM()
            {
                Photo = photo,
                Categories = _photoRepo.GetCategoryList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PhotoVM model)
        {
            if (model == null)
            {
                return View(model);
            }

            Photo photo = _photoRepo.Find(model.Photo.Id);
            if (photo == null)
            {
                return NotFound();
            }

            if (_photoRepo.Remove(photo))
            {
                string fileName = _webHostEnvironment.WebRootPath + WebConstants.PhotoPath + photo.File;
                string previewFileName = _webHostEnvironment.WebRootPath + WebConstants.PreviewPath + photo.File;
                var file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.Delete();
                }
                file = new FileInfo(previewFileName);
                if (file.Exists)
                {
                    file.Delete();
                }
                _photoRepo.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
