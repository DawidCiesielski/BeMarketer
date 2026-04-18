using BeMarketer.Data;
using BeMarketer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq; // Wymagane do LINQ (.Select)
using System.Threading.Tasks;

namespace BeMarketer.Data // Dodano przestrzeń nazw dla porządku
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            // Upewnij się, że migracje są zastosowane (bezpieczniej opakować w try-catch, 
            // gdyby baza była zablokowana)
            try
            {
                await db.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nie można zaaplikować migracji z poziomu Seeder'a: {ex.Message}");
            }

            // Role do utworzenia
            string[] roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Dane admina z konfiguracji (lub bezpieczne domyślne)
            var adminEmail = "admin@bemarketer.pl";
            var adminUserName = adminEmail; // Najlepiej, aby Username był Emailem
            var adminPassword = "Admin123!"; // Musi spełniać zasady (np. znak specjalny)

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Role = UserRole.Admin // Zwróć uwagę, by Namespace był prawidłowy
                };

                var createResult = await userManager.CreateAsync(admin, adminPassword);
                if (!createResult.Succeeded)
                {
                    // POPRAWKA: Wyciągamy faktyczne opisy błędów z listy obiektów IdentityError
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Nie udało się utworzyć konta admina. Powody: {errors}");
                }
            }

            // Przypisz rolę Admin jeśli jeszcze nie przypisana
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}