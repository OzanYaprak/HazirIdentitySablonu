using AspNetCoreIdentityApp.ClaimProvider;
using AspNetCoreIdentityApp.Data;
using AspNetCoreIdentityApp.Extensions;
using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
});


//ClaimProvider için eklendi
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();


//Email servisi için eklendi.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();


//Extensions içinde StartupExtensions altýnda yazýlý.
builder.Services.IdentityExtensions();
builder.Services.CookieExtensions();


//security stamp yenilenme süresi (default 30dk da býrakýldý)
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

//Profile pictures wwwroot a gitmek için eklendi.
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));


var app = builder.Build();


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

//Kimlik doðrulamasý için yazýlan middleware (facebook,google ile giriþ yapmak içinde bu middleware gerekli)
app.UseAuthentication();

//Kimlik yetkilendirmesi için yazýlan middleware
app.UseAuthorization();

//admin paneli için (admin sayfasý) yazýldý.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
