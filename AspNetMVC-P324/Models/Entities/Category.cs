using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetMVC_P324.Models.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }
    }
}
