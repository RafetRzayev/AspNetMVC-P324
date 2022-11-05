using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_P324.Areas.AdminPanel.Models
{
    public class CategoryCreateModel
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }
    }
}
