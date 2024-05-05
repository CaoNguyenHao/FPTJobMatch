using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FPTJobMatch.Data;
using FPTJobMatch.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit;
using Microsoft.Extensions.DependencyInjection;
using MailKit.Net.Smtp;
using MimeKit;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<FPTJobMatch.Models.IEmailSender, EmailSender>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//var services = builder.Services;

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Allow", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();

    });
    options.AddPolicy("ShowAdminMenu", pb =>
    {
        pb.RequireRole("Admin");
    });
    options.AddPolicy("ShowCompanyMenu", pb =>
    {
        pb.RequireRole("Company");

    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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


app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

