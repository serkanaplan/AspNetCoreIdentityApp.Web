using AspNetCoreIdentityApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using AspNetCoreIdentityApp.Web.Extenisons;
using Microsoft.AspNetCore.Identity;
using AspNetCoreIdentityApp.Web.OptionsModels;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using AspNetCoreIdentityApp.Web.Requirements;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreIdentityApp.Web.Seeds;
using AspNetCoreIdentityApp.Web.ClaimProvider;
using AspNetCoreIdentityApp.Web.Permissions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

// appsettings.json dosyasındaki "EmailSettings" bölümünü okur,Bu ayarları EmailSettings sınıfının bir örneğine bağlar,Bu yapılandırılmış EmailSettings örneğini uygulamanın servis konteynerine kaydeder.
// Böylece, uygulamanın herhangi bir yerinde IOptions<EmailSettings> enjekte edilerek bu ayarlara erişilebilir.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// bu satırı silebilirsin, securitystamp konusu için eklendi zaten default 30 dakikadır
builder.Services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromMinutes(30));

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

builder.Services.AddIdentityWithExt();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AnkaraPolicy", policy => policy.RequireClaim("city", "ankara"))
    .AddPolicy("ExchangePolicy", policy => policy.AddRequirements(new ExchangeExpireRequirement()))
    .AddPolicy("ViolencePolicy", policy => policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 }))
    .AddPolicy("Permissions.Order.Read", policy => policy.RequireClaim("permission", Permissions.Order.Read))
    .AddPolicy("Permissions.Order.Delete", policy => policy.RequireClaim("permission", Permissions.Order.Delete))
    .AddPolicy("Permissions.Stock.Delete", policy => policy.RequireClaim("permission", Permissions.Stock.Delete))
    .AddPolicy("OrderPermissionReadAndDelete", policy =>
    {
        policy.RequireClaim("permission", Permissions.Order.Read);
        policy.RequireClaim("permission", Permissions.Order.Delete);
        policy.RequireClaim("permission", Permissions.Stock.Delete);
    });

// Uygulama çerezlerini yapılandırmak için kullanılır.
builder.Services.ConfigureApplicationCookie(opt =>
{
    // Yeni bir çerez oluştur ve adını belirle
    var cookieBuilder = new CookieBuilder
    {
        Name = "AppCookie"
    };

    // Kullanıcı kimliği doğrulanmamışsa yönlendirilecek giriş sayfası yolu
    opt.LoginPath = new PathString("/Home/Signin");

    // Çıkış işlemi için kullanılacak yol
    opt.LogoutPath = new PathString("/Member/logout");

    // Yetkisiz erişim durumunda yönlendirilecek sayfa
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");

    // Oluşturulan çerez yapılandırmasını ata
    opt.Cookie = cookieBuilder;

    // Çerezin geçerlilik süresini 60 gün olarak ayarla
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);

    // Kaydırmalı son kullanma tarihini etkinleştir
    // Kullanıcı aktif olduğu sürece çerezin süresi yenilenir
    opt.SlidingExpiration = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await PermissionSeed.Seed(roleManager);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();