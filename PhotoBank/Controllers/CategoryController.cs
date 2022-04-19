using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.Data;
using PhotoBank.Data.Repository;
using PhotoBank.Data.Repository.IRepository;
using PhotoBank.Models;

namespace PhotoBank.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        // GET: CategoryController

        public ActionResult Index()
        {
            IEnumerable<Category> Categories = _categoryRepo.GetAll(orderBy: r => r.OrderBy(t => t.Name));//_db.Categories.OrderBy(r => r.Name);
            return View(Categories);
        }


        ///////////////Create
        public ActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                if (_categoryRepo.Add(model))
                {
                    _categoryRepo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }

        ////////////////Edit 
        public ActionResult Edit(int id)
        {
            Category model = _categoryRepo.Find(id);
            if (model == null)
            {
                return NotFound();

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                if (_categoryRepo.Update(model))
                {
                    _categoryRepo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            return View(model);
        }

        ///////////////////Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Category model = _categoryRepo.Find(id);
            if (model == null)
            {
                return NotFound();

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Category cat)
        {
            if(_categoryRepo.Remove(cat))
            {
                _categoryRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
    }
}
