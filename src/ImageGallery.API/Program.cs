using ImageGallery.API.Authorization;
using ImageGallery.API.DbContexts;
using ImageGallery.API.Services;
using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddDbContext<GalleryContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ImageGalleryDBConnectionString"]);
});

// register the repository
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IAuthorizationHandler, MustOwnImageHandler>();

// register AutoMapper-related services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// this line of code is used for .net7.0 and before
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// this line of code is used only for in .net 8.0 and afterward
Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = "https://localhost:5001";
                    opt.Audience = "imagegalleryapi";
                    opt.TokenValidationParameters = new()
                    {
                        NameClaimType = "given_name",
                        RoleClaimType = "role",
                        ValidTypes = new[] { "at+jwt" }
                    };
                });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("UserCanAddImage",
                    AuthorizationPolicies.CanAddImage());

    opt.AddPolicy("ClientApplicationCanWrite",
                  policyBuilder =>
                  {
                      policyBuilder.RequireClaim("scope", "imagegalleryapi.write");
                  });

    opt.AddPolicy("MustOwenImage",
        policyBuilder =>
        {
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.AddRequirements(new MustOwnImageRequirment());
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
