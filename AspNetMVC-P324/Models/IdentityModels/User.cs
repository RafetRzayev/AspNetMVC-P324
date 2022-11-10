using Microsoft.AspNetCore.Identity;

namespace AspNetMVC_P324.Models.IdentityModels
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
    }
}
