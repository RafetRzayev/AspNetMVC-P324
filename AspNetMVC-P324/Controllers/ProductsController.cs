using AspNetMVC_P324.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_P324.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private int _productCount;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productCount = _dbContext.Products.Count();
        }

        public IActionResult Index()
        {
            ViewBag.productCount = _productCount;


            var products = _dbContext.Products.Include(x => x.Category).Take(4).ToList();

            //var products1 = _dbContext.Products.Where(x => x.Id > 1).ToList();
            //var products2 = _dbContext.Products.ToList().Where(x => x.Id > 1);
            //var products1Count = _dbContext.Products.Count(x => x.Id > 1);
            //var products2Count = _dbContext.Products.Where(x => x.Id > 1).Count();

            //var productsWithCategoryA = _dbContext.Products.Include(x => x.Category).Where(x => x.CategoryId > 2).FirstOrDefault();
            //var productsWithCategoryB = _dbContext.Products.Include(x => x.Category).FirstOrDefault(x => x.CategoryId > 2);

            //var products2Count = _dbContext.Products.ToList().Count();



            return View(products);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _dbContext.Products.SingleOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult Partial(int skip)
        {
            if (skip >= _productCount)
                return BadRequest();

            var products = _dbContext.Products.Include(x => x.Category).Skip(skip).Take(4).ToList();

            return PartialView("_ProductPartial", products);
        }

        public IActionResult GetProductViewComponent()
        {
            var productViewComponent = new ViewComponentResult
            {
                ViewComponentName = "Product"
            };

            return productViewComponent;
        }

    }
}
