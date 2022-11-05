using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_P324.Areas.AdminPanel.Models
{
    public class SlideImageCreateModel
    {
        public IFormFile Image { get; set; }
    }
}
