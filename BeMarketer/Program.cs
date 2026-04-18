using BeMarketer.Data;
using BeMarketer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------
// 1. REJESTRACJA SERWISÓW (tutaj używamy "builder.Services")
// --------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// --------------------------------------------------------
// 2. ZBUDOWANIE APLIKACJI (Kluczowy moment!)
// Narzędzia EF Core muszą dotrzeć do tej linijki, aby zadziałać.
// --------------------------------------------------------
var app = builder.Build();

// --------------------------------------------------------
// 3. SEEDOWANIE BAZY DANYCH (Musi być PO builder.Build()!)
// Tutaj używamy "app.Services", a nie "builder.Services"
// --------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Wywołanie Twojej metody do seedowania
        // await DbSeeder.SeedAsync(services, builder.Configuration, app.Logger);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wystąpił błąd podczas seedowania bazy danych.");
    }
}

// --------------------------------------------------------
// 4. KONFIGURACJA MIDDLEWARE
// --------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ważne: to musi być przed Authorization!
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// --------------------------------------------------------
// 5. URUCHOMIENIE APLIKACJI
// --------------------------------------------------------
app.Run();