using AspNetMVC_P324.DAL;
using AspNetMVC_P324.Models.Entities;
using AspNetMVC_P324.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AspNetMVC_P324.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {

            HttpContext.Session.SetString("session", "Hello");
            Response.Cookies.Append("cookie", "P324", new CookieOptions { Expires = DateTimeOffset.Now.AddHours(1) });

            var sliderImages = _dbContext.SliderImages.ToList();
            var slider = _dbContext.Sliders.SingleOrDefault();
            var categories = _dbContext.Categories.ToList();
            var products = _dbContext.Products.ToList();

            var homeViewModel = new HomeViewModel
            {
                SliderImages = sliderImages,
                Slider = slider,
                Categories = categories,
                Products = products
            };

            return View(homeViewModel);
        }

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return RedirectToAction(nameof(Index));

            var products = _dbContext.Products.Where(x=>x.Name.ToLower().Contains(searchText.ToLower())).ToList();

            return PartialView("_SearchedProductPartial", products);
        }

        public IActionResult Basket()
        {
            //var session = HttpContext.Session.GetString("session");
            //var cookie = Request.Cookies["cookie"];
            //return Content(session + " - " + cookie);

            var basketJson = Request.Cookies["basket"];

            if (basketJson == null) return BadRequest();

            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketJson);

            return Json(basketViewModels);
        }

        public IActionResult AddToBasket(int id)
        {
            var product = _dbContext.Products.Include(x => x.Category).SingleOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            var basketJson = Request.Cookies["basket"];

            List<BasketViewModel> existBasketViewModels = null;
            if (basketJson != null)
                existBasketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketJson);

            if (existBasketViewModels != null)
            {
                var existBasketViewModel = existBasketViewModels.Where(x => x.Id == product.Id).SingleOrDefault();

                if (existBasketViewModel != null) existBasketViewModel.Count++;
                else
                {
                    existBasketViewModels.Add(new BasketViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Category = product.Category,
                        Count = 1
                    });
                }

            }
            else
            {
                existBasketViewModels = new List<BasketViewModel>();

                existBasketViewModels.Add(new BasketViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Category = product.Category,
                    Count = 1
                });

            }



            var basketViewModelJson = JsonConvert.SerializeObject(existBasketViewModels, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Response.Cookies.Append("basket",basketViewModelJson);

            return RedirectToAction(nameof(Basket));

        }
    }
}
