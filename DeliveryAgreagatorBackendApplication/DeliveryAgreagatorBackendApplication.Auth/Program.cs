using DeliveryAgreagatorApplication.Auth.BL.Services;
using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using DeliveryAgreagatorApplication.Common.Schemas;
using DeliveryAgreagatorBackendApplication.Auth.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Specify connection string!");
builder.Services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenSerivce, TokenSerivce>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options=>
options.Password.RequiredLength = 10)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.SchemaFilter<EnumSchemaFilter>();
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "DeliveryAgreagatorApplication.Auth.xml");
    c.IncludeXmlComments(xmlPath);

});
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtConfigurations>();
builder.Services.Configure<JwtConfigurations>(builder.Configuration.GetSection("JwtSettings"));
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
    options.AddPolicy("RefreshOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("TokenTypeClaim", "Refresh");
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("TokenTypeClaim", "Access")
            .Build();
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
