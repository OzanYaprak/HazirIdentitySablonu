using AspNetCoreIdentityApp.ClaimProvider;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Data;
using AspNetCoreIdentityApp.Extensions;
using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.Requirements;
using AspNetCoreIdentityApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

//Policy tan�mlama i�in yaz�ld� --B�R POLICY ��ER�S�NDE B�RDEN FAZLA KURAL YAZILAB�L�N�R.
builder.Services.AddAuthorization(options =>
{
    //�stanbulPolicy tan�mlamas� yapt���m�z alanlar� city bilgisi istanbul ve ankara olan kullan�c�lar g�rebilecek veya,
    //Ankaray� silip sadece istanbul olarakta yapabilir veya daha fazla �ehir ekleyebiliriz.
    options.AddPolicy("�stanbulPolicy", policy =>
    {
        policy.RequireClaim("City", "�stanbul"); //KURAL        
    });


    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement()); //ExchangeExpireRequirement i�indeki KURAL  
    });
});

//Requirements i�in eklendi
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();

//ClaimProvider i�in eklendi
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();


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

//Profile pictures wwwroot a gitmek i�in eklendi.
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

//Kimlik do�rulamas� i�in yaz�lan middleware (facebook,google ile giri� yapmak i�inde bu middleware gerekli)
app.UseAuthentication();

//Kimlik yetkilendirmesi i�in yaz�lan middleware
app.UseAuthorization();

//admin paneli i�in (admin sayfas�) yaz�ld�.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
