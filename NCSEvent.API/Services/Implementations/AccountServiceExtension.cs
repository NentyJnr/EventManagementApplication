using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Implementations
{
    public static class AccountServiceExtension
    {
        public static async Task SeedAdmin(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Roles>>();

                // Check if admin user exists
                var adminUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == "adminonly@gmail.com");

                if (adminUser == null)
                {
                    // Create admin user
                    adminUser = new Users
                    {
                        FirstName = "Admin",
                        LastName = "Only",
                        Email = "adminonly@gmail.com",
                        DateCreated = DateTime.Now,
                        UserName = "adminonly@gmail.com",
                        NormalizedEmail = "ADMINONLY@GMAIL.COM",
                        NormalizedUserName = "ADMINONLY@GMAIL.COM",
                        PhoneNumber = "1234567890",
                        EmailConfirmed = true
                    };

                    var password = new PasswordHasher<Users>();
                    adminUser.PasswordHash = password.HashPassword(adminUser, "Admin@123.");

                    var result = await userManager.CreateAsync(adminUser);

                    if (result.Succeeded)
                    {
                        var roleExists = await roleManager.RoleExistsAsync("Admin Role");
                        if (!roleExists)
                        {
                            var AdminRole = new Roles
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Admin Role",
                                NormalizedName = "ADMIN ROLE",
                                Description = "Admin role"
                            };
                            await roleManager.CreateAsync(AdminRole);
                        }

                        await userManager.AddToRoleAsync(adminUser, "Admin Role");
                    }
                }

            }
        }
    }
}
