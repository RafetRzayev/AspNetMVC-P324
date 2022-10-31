using AspNetMVC_P324.DAL;
using Microsoft.AspNetCore.Mvc;

namespace AspNetMVC_P324.ViewComponents
{
    public class SosialMediaViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public SosialMediaViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IViewComponentResult Invoke()
        {
            var sosialMedias=_dbContext.SosialMedias.ToList();
            return View(sosialMedias);
        }
    }
}
