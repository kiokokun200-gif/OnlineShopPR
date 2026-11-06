using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.API;
using OnlineShop.API.Models;
using OnlineShop.API.Services;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
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
    ValidIssuer = jwt.Issuer,

    ValidateAudience = true,
    ValidAudience = jwt.Audience,

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = jwt.GetSymmetricSecurityKey(),

    ValidateLifetime = false,
    NameClaimType = "name",
    //RoleClaimType = "role"
};


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt =>
    {
        opt.MapInboundClaims = true;
        opt.TokenValidationParameters = validation;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddDbContext<OnlineShopContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwibmFtZSI6InN0cmluZyIsInJvbGUiOiLQmtC70LjQtdC90YIiLCJpc3MiOiJKd3RPbmxpbmVTaG9wZSIsImF1ZCI6Ikp3dE9ubGluZVNob3BlQXBpIn0.HvS9EdjAStZj4n_eRWQP1xP6_K2DjQdQYHJLcKjLjjw"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped< AuthService>();


builder.Services.AddTransient<ProductsService>();
builder.Services.AddTransient<KorzinaService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.

app.MapControllers();
app.Run();  