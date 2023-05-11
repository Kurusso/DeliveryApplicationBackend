using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;
using System.Text.Json;
using DeliveryAgreagatorApplication.Notifications.Common.Models;
using DeliveryAgreagatorApplication.Notifications.Common.Services;
using DeliveryAgreagatorApplication.Notifications.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Specify connection string!");
builder.Services.AddDbContext<NotificationDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<RabbitMqListener>();

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
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
var app = builder.Build();
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
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.RequireClaim("TokenTypeClaim", "Access")
			.Build();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.MapHub<NotificationsHub>("/notifications");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
