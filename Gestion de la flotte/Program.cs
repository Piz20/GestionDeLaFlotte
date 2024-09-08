using Gestion_de_la_flotte.Data;
using Gestion_de_la_flotte.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la cha�ne de connexion � la base de donn�es
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuration d'ASP.NET Core Identity avec votre classe `User` personnalis�e
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Assurez-vous d'inclure l'authentification
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=AdminDashBoard}/{id?}");
app.MapRazorPages();

app.Run();
