using AspNetCoreIdentityApp.Data;
using AspNetCoreIdentityApp.Extensions;
using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
});


//Email servisi i�in eklendi.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();



//Extensions i�inde StartupExtensions alt�nda yaz�l�.
builder.Services.IdentityExtensions();
builder.Services.CookieExtensions();


//security stamp yenilenme s�resi (default 30dk da b�rak�ld�)
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});




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

//Kimlik do�rulamas� i�in yaz�lan middleware (facebook,google ile giri� yapmak i�inde bu middleware gerekli)
app.UseAuthentication();

//Kimlik yetkilendirmesi i�in yaz�lan middleware
app.UseAuthorization();

//admin paneli i�in (admin sayfas�) yaz�ld�.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
