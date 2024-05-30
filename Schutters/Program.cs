using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Schutters.Data;
using Schutters.Models;
using Schutters.Repositories;
using Schutters.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SchuttersConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
 options.SignIn.RequireConfirmedAccount = false)
 .AddRoles<IdentityRole>()
 .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<ClubService>();
builder.Services.AddTransient<LedenService>();
builder.Services.AddTransient<IClubRepository, SQLClubRepository>();
builder.Services.AddTransient<ILedenRepository, SQLLedenRepository>();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SchuttersContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("SchuttersConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
