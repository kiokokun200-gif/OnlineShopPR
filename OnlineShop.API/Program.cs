using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var key = new SymmetricSecurityKey(
    RandomNumberGenerator.GetBytes(32));

var validation = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = "JwtOnlineShope",

    ValidateAudience = true,
    ValidAudience = "JwtOnlineShopeApi",

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = key,


    NameClaimType = "name",
    RoleClaimType = "role"
};

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();  