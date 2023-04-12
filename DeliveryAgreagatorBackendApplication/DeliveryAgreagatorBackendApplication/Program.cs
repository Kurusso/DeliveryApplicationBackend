using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Main.BL.Services;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication;
using DeliveryAgreagatorBackendApplication.Schemas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

    var xmlPath = Path.Combine(basePath, "DeliveryAgreagatorBackendApplication.xml");
    c.IncludeXmlComments(xmlPath);

});
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
            ValidIssuer = JwtConfigurations.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtConfigurations.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtConfigurations.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,

        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CartOperations", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("CartOperations", "Allow");
    });
    options.AddPolicy("SetRating", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("SetRating", "Allow");
    });
    options.AddPolicy("OrderOperationsCustomer", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Customer");
    });
    options.AddPolicy("OrderOperationsCook", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Cook");
    });
    options.AddPolicy("OrderOperationsCourier", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Courier");
    });
    options.AddPolicy("OrderOperationsManager", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("OrderOperation", "Manager");
    });
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
