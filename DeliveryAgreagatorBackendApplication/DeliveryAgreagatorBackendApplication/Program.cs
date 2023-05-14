using DeliveryAgreagatorApplication.Common.Configurations;
using DeliveryAgreagatorApplication.Common.Schemas;
using DeliveryAgreagatorApplication.Main.BL.Services;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Specify connection string!");
builder.Services.AddDbContext<BackendDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
	c.SchemaFilter<EnumSchemaFilter>();
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "DeliveryAgreagatorApplication.Main.xml");
    c.IncludeXmlComments(xmlPath);

});
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtConfigurations>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("CartOperations", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("CartOperations", "Allow");
        policy.RequireClaim("TokenTypeClaim", "Access");
        policy.RequireClaim("Ban", "False");
    });
    options.AddPolicy("SetRating", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("SetRating", "Allow");
        policy.RequireClaim("TokenTypeClaim", "Access");
        policy.RequireClaim("Ban", "False");
    });
    options.AddPolicy("OrderOperationsCustomer", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Customer");
        policy.RequireClaim("TokenTypeClaim", "Access");
        policy.RequireClaim("Ban", "False");
    });
    options.AddPolicy("OrderOperationsCook", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Cook");
        policy.RequireClaim("TokenTypeClaim", "Access");
        policy.RequireClaim("Ban", "False");
    });
    options.AddPolicy("OrderOperationsCourier", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Courier");
        policy.RequireClaim("TokenTypeClaim", "Access");
        policy.RequireClaim("Ban", "False");
    });
    options.AddPolicy("OrderOperationsManager", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("GetOrders", "Manager");
        policy.RequireClaim("TokenTypeClaim", "Access");
        policy.RequireClaim("Ban", "False");
    });
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("TokenTypeClaim", "Access")
        .RequireClaim("Ban", "False")
        .Build();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
