using AspNetMVC_P324.Areas.AdminPanel.Data;
using AspNetMVC_P324.Areas.AdminPanel.Models;
using AspNetMVC_P324.DAL;
using AspNetMVC_P324.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Sekil secilmelidir");
                return View();
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Sekilin hecmi max 10mb ola biler");
                return View();
            }

            var unicalFileName = await model.Image.GenerateFile(Constants.RootPath);

            await _dbContext.SliderImages.AddAsync(new SliderImage
            {
                Name = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var slideImage = await _dbContext.SliderImages.FindAsync(id);

            return View(new SlideImageUpdateModel
            {
                ImageUrl = slideImage.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SlideImageUpdateModel model)
        {
            if (id == null) return NotFound();

            var slideImage = await _dbContext.SliderImages.FindAsync(id);

            if (slideImage == null) return NotFound();

            if (slideImage.Id != id) BadRequest();

            if (!ModelState.IsValid)
            {
                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Sekil secilmelidir");

                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Sekilin hecmi max 10mb ola biler");

                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            var path = Path.Combine(Constants.RootPath, "img", slideImage.Name);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalFileName = await model.Image.GenerateFile(Constants.RootPath);

            slideImage.Name = unicalFileName;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slideImage = await _dbContext.SliderImages.FindAsync(id);

            if (slideImage == null) return NotFound();

            if (slideImage.Id != id) BadRequest();

            var path = Path.Combine(Constants.RootPath, "img", slideImage.Name);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.SliderImages.Remove(slideImage);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult MultipleCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MultipleCreate(SlideImageCreateMultipleModel model)
        {
            if (!ModelState.IsValid)
                return View();

            foreach (var image in model.Images)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("Images", "Sekil secilmelidir");
                    //return View();
                    continue;
                }

                if (!image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Images", $"{image.FileName}-Sekilin hecmi max 10mb ola biler");
                    //return View();
                    continue;
                }

                var unicalFileName = await image.GenerateFile(Constants.RootPath);

                await _dbContext.SliderImages.AddAsync(new SliderImage
                {
                    Name = unicalFileName,
                });

                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
