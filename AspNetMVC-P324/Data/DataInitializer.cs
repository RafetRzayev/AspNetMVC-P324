using AspNetMVC_P324.DAL;
using AspNetMVC_P324.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_P324.Data
{
    public class DataInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(AppDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedData()
        {
            await _dbContext.Database.MigrateAsync();

            var roles = new List<string> { RoleConstants.AdminRole, RoleConstants.UserRole };

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;

                var result = await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = role
                });

                if (!result.Succeeded)
                {
                    //logging
                }
            }

            var user = new User
            {
                Fullname = "Admin admin",
                UserName = "admin",
                Email = "admin@code.az"
            };

            if (await _userManager.FindByNameAsync(user.UserName) != null)
                return;

            await _userManager.CreateAsync(user, "12345@");
            await _userManager.AddToRoleAsync(user, RoleConstants.AdminRole);
        }
    }
}
