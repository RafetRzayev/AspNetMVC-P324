using AspNetMVC_P324.Areas.AdminPanel.Models;
using AspNetMVC_P324.DAL;
using AspNetMVC_P324.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_P324.Areas.AdminPanel.Controllers
{
    public class SlideImageController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public SlideImageController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var slideImages = await _dbContext.SliderImages.ToListAsync();

            return View(slideImages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideImageCreateModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (!model.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Sekil secilmelidir");
                return View();
            }

            if (model.Image.Length > 1024 * 1024*2012)
            {
                ModelState.AddModelError("Image", "Sekilin hecmi max 1mb ola biler");
                return View();
            }

            var unicalName = $"{Guid.NewGuid}-{model.Image.FileName}";
            var path = Path.Combine(_environment.WebRootPath, "img", unicalName);


            var fs = new FileStream(path, FileMode.Create);
            await model.Image.CopyToAsync(fs);

            await _dbContext.SliderImages.AddAsync(new SliderImage
            {
                Name = unicalName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
