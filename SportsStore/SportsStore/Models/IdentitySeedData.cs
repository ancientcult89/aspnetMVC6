using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        { 
            AppIdentityDbContext dbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>(); 
            
            if(dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();

            UserManager<IdentityUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByNameAsync(adminUser);
            if (user == null)
            {
                user = new IdentityUser("Admin");
                user.Email = "admin@example.com";
                user.PhoneNumber = "555-1234";
                await userManager.CreateAsync(user, adminPassword);    
            }
        }
    }
}
