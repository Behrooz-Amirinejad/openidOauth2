using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAccessTokenManagement();

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                })
                .AddUserAccessTokenHandler();

// use middleware authentication
builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme , opt =>
                {
                    opt.AccessDeniedPath = "/Authentication/AccessDenied";
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                                  opt =>
                                  {
                                      opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                                      opt.Authority = "https://localhost:5001";
                                      opt.ClientId = "imagegalleryclient";
                                      opt.ClientSecret = "secret";
                                      opt.ResponseType = "code";
                                      //opt.Scope.Add("openid");
                                      //opt.Scope.Add("profile");
                                      //opt.CallbackPath = new PathString("signin-oidc");
                                      opt.SaveTokens = true;
                                      opt.GetClaimsFromUserInfoEndpoint = true;
                                      opt.ClaimActions.Remove("aud");
                                      opt.ClaimActions.DeleteClaim("sid");
                                      opt.ClaimActions.DeleteClaim("idp");
                                      opt.Scope.Add("roles");
                                      opt.Scope.Add("imagegalleryapi.fullaccess");
                                      opt.ClaimActions.MapJsonKey("role", "role");
                                      opt.TokenValidationParameters = new()
                                      {
                                          NameClaimType = "given_name",
                                          RoleClaimType = "role",
                                      };
                                  });
                
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
