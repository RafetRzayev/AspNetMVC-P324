using AspNetMVC_P324.Areas.AdminPanel.Models;
using AspNetMVC_P324.DAL;
using AspNetMVC_P324.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_P324.Areas.AdminPanel.Controllers
{

    public class CategoryController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateModel category)
        {
            //var errorList = ModelState.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //);

            if (!ModelState.IsValid)
                return View();
                //return Json(errorList);

            var existName = await _dbContext.Categories.AnyAsync(x => x.Name.ToLower().Equals(category.Name.ToLower()));

            if (existName)
            {
                ModelState.AddModelError("name", "eyni ad tekrarlana bilmez");
                return View();
            }

            var categoryEntity = new Category
            {
                Name = category.Name,
                Description = category.Description
            };

            await _dbContext.Categories.AddAsync(categoryEntity);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
