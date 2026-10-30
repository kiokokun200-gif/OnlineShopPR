using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.API;
using OnlineShop.API.Models;
using OnlineShop.API.Services;
using System.Security.Cryptography;
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.Configure<JWTOption>(
    builder.Configuration.GetSection(JWTOption.SectionName));

var jwt = builder.Configuration.GetSection(JWTOption.SectionName).Get<JWTOption>()
    ?? throw new InvalidOperationException("Error Jwt .env");


var validation = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = "JwtOnlineShope",

    ValidateAudience = true,
    ValidAudience = "JwtOnlineShopeApi",

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = jwt.GetSymmetricSecurityKey(),

    NameClaimType = "name",
    RoleClaimType = "role"
};


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.MapInboundClaims = true;
        opt.TokenValidationParameters = validation;
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddDbContext<OnlineShopContext>();

builder.Services.AddScoped< AuthService>(); 

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.

app.MapControllers();
app.Run();  