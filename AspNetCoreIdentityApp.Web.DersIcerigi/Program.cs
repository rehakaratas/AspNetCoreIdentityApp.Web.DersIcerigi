using AspNetCoreIdentityApp.Web.DersIcerigi.ClaimProvider;
using AspNetCoreIdentityApp.Web.DersIcerigi.Extensions;
using AspNetCoreIdentityApp.Repository.DersIcerigi.Models;
using AspNetCoreIdentityApp.Core.DersIcerigi.OptionModels;
using AspNetCoreIdentityApp.Core.DersIcerigi.PermissionsRoot;
using AspNetCoreIdentityApp.Web.DersIcerigi.Requirements;
using AspNetCoreIdentityApp.Repository.DersIcerigi.Seeds;
using AspNetCoreIdentityApp.Service.DersIcerigi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"),options=>
    {
        options.MigrationsAssembly("AspNetCoreIdentityApp.Repository");
    });
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});


builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentityWithExtension();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IClaimsTransformation,UserClaimProvider>();

builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
builder.Services.AddScoped<IMemberService,MemberService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ankara");
    });


    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });


    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge=18});
    });

    options.AddPolicy("OrderPermissionReadAndDelete", policy =>
    {
        policy.RequireClaim("permission",Permission.Order.Read);
        policy.RequireClaim("permission",Permission.Order.Delete);
        policy.RequireClaim("permission",Permission.Stock.Delete);
    });

    options.AddPolicy("Permission.Order.Read", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Read);
       
    });

    options.AddPolicy("Permission.Order.Delete", policy =>
    {
        
        policy.RequireClaim("permission", Permission.Order.Delete);
        
    });

    options.AddPolicy("Permission.Stock.Delete", policy =>
    {
       
        policy.RequireClaim("permission", Permission.Stock.Delete);
    });

});


builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "AppCookie";
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.LogoutPath = new PathString("/Member/LogOut");
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});



var app = builder.Build();


using(var scope=app.Services.CreateScope())
{
    var roleMaanager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await PermissionSeed.Seed(roleMaanager);
}

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



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
