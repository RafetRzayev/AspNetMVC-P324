using Microsoft.AspNetCore.Mvc;

namespace AspNetMVC_P324.Areas.AdminPanel.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
