using Bussines.DependencyResolver;
using DataAccess.Concrete.SQLserver;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using WebUI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Localizer
builder.Services.AddSingleton<LaunguageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddMvcLocalization().AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) =>
{
    var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
    return factory.Create(nameof(SharedResource), assemblyName.Name);
});
builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportCulture = new List<CultureInfo>
    {
        new CultureInfo("az"),
        new CultureInfo("en"),
        new CultureInfo("ru"),
    };
    options.DefaultRequestCulture = new RequestCulture(culture: "az", uiCulture: "az");
    options.SupportedCultures = supportCulture;
    options.SupportedUICultures = supportCulture;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});
#endregion
builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Run();

builder.Services.ConfigureApplicationCookie(option =>
{
    //qeydiyatsiz admin panele url den giris zamani icaze vermir yonlendirir path
    option.LoginPath = "/Auth/Login";
});
builder.Services.AddHttpClient();
var app = builder.Build();
app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
