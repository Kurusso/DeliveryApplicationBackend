using DeliveryAgreagatorApplication.Auth.BL.Services;
using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using DeliveryAgreagatorApplication.Common.Schemas;
using DeliveryAgreagatorBackendApplication.Auth;
using DeliveryAgreagatorBackendApplication.Auth.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Specify connection string!");
builder.Services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenSerivce, TokenSerivce>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options=>
options.Password.RequiredLength = 10)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.SchemaFilter<EnumSchemaFilter>();
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "DeliveryAgreagatorApplication.Auth.xml");
    c.IncludeXmlComments(xmlPath);

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ValidatorsPile.ValidateTokenParent
        };
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
    options.AddPolicy("RefreshOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("TokenTypeClaim", "Refresh");
    });
});
var app = builder.Build();
RolesAndClaimsInit.Initialize(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
