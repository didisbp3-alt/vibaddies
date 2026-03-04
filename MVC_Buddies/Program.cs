using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using MVC_Buddies.Dtos;
using MVC_Buddies.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "pt-PT", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("pt-PT");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();

    // cookie -> query -> Accept-Language
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// HttpClient para a tua API
// UMA única base, HTTPS, com /api/
//builder.Services.AddHttpClient<HomeApiService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7033/api/");
//});

////builder.Services.AddHttpClient<IAuthApiService, AuthApiService>("API_Buddies", client =>
////{
////    client.BaseAddress = new Uri("http://localhost:7033/api/");
////});

//builder.Services.AddHttpClient<HomeApiService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7158/api/");
//});

//builder.Services.AddHttpClient("API_Buddies", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7158/api/");
//});
builder.Services.AddHttpClient<HomeApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7158/api/");
})
.AddHttpMessageHandler<JwtHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtHandler>();

builder.Services.AddHttpClient("API_Buddies", client =>
{
    client.BaseAddress = new Uri("https://localhost:7158/api/");
})
.AddHttpMessageHandler<JwtHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtHandler>();

builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7158/api/");
})
.AddHttpMessageHandler<JwtHandler>();


// HomeApiService (se também usa API)
//builder.Services.AddScoped<IHomeApiService, HomeApiService>();
builder.Services.AddScoped<IAdminApiService, AdminApiService>();
builder.Services.AddScoped<IAdminCrudApiService, AdminCrudApiService>();
builder.Services.AddScoped<IReportsApiService, ReportsApiService>();
//builder.Services.AddScoped<IPetOwnerApiService, PetOwnerApiService>();
//builder.Services.AddScoped<IPetSitterApiService, PetSitterApiService>();

builder.Services.AddHttpContextAccessor();

// ? COOKIE AUTH (isto é o que faz o Layout mudar e não voltar ao login)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".Buddies.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;

        // ?? Isto evita problemas em localhost/dev e mantém seguro em https
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
    });

builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

// ? Localização ANTES do routing
app.UseRequestLocalization(app.Services
    .GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value);

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ? Rota MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
