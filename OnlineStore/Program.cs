using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using OnlineStore.Models;


var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));



// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication("Cookies").AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/EnterOrRegistration/Enter");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/EnterOrRegistration/Registration");
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyForAdmin", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Администратор");
    });
    options.AddPolicy("OnlyForRegisteredUser", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Пользователь");
    });
    options.AddPolicy("OnlyForManager", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Менеджер");
    });
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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
