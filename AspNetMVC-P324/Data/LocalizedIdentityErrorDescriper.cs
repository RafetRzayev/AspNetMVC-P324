using Microsoft.AspNetCore.Identity;

namespace AspNetMVC_P324.Data
{
    public class LocalizedIdentityErrorDescriper : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Email tekrarlana bilmez"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "Password dogru deyil"
            };
        }
    }
}
