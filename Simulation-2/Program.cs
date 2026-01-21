using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Simulation_2.DAL;
using Simulation_2.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 8;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    opt.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=home}/{action=index}/{id?}"
    );
app.MapControllerRoute(
    name:"default",
    pattern:"{controller=home}/{action=index}/{id?}"
    );
app.Run();
