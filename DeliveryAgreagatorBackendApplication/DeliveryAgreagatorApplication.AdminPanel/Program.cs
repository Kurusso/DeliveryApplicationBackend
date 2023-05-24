using DeliveryAgreagatorApplication.AdminPanel.Models.Configurations;
using DeliveryAgreagatorApplication.AdminPanel.Services;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using DeliveryAgreagatorApplication.Main.BL.Services;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Specify connection string!");
string connection2 = builder.Configuration.GetConnectionString("DefaultConnection2") ?? throw new NullReferenceException("Specify connection string!");
var cookieSettings = builder.Configuration.GetSection("CookieSettings").Get<CookieConfigurations>();
builder.Services.AddDbContext<BackendDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connection2));
builder.Services.AddControllers();
builder.Services.AddScoped<IRestaurantAdminService, RestaurantAdminService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = cookieSettings.Name;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(cookieSettings.LifeTime);
    options.SlidingExpiration = true;
});
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Restaurants/Index", "");
});
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();

app.Run();
