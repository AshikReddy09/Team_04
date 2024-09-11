using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Sprint_sol1.Contracts;
using Sprint_sol1.Data;
using Sprint_sol1.Repository;
using Sprint_sol1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/AccAdmin/LoginAdmin"; // Default login path
        options.LogoutPath = "/AccAdmin/Logout"; // Default logout path
        options.AccessDeniedPath = "/AccAdmin/AccessDenied"; // Access denied path
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee")); // Add a policy for employees if needed
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>)); // Repository configuration
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IAzureStorage, AzureStorage>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication
app.UseAuthorization();  // Enable authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Map("/AccessDenied", (context) =>
{
    context.Response.Redirect("/Shared/AccessDenied");
    return Task.CompletedTask;
});

app.Run();
