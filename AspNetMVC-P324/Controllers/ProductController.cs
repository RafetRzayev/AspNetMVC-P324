using AspNetMVC_P324.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_P324.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var products = _dbContext.Products.Include(x => x.Category).ToList();

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
    }
}
