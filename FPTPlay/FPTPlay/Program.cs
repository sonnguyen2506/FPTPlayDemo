using Microsoft.EntityFrameworkCore;
using FPTPlay.Data;
using FPTPlay.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using FPTPlay.Helpers;
using FPTPlay.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// 🔥 DbContext
builder.Services.AddDbContext<FPTPlayContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 🔥 Đăng ký Service
builder.Services.AddScoped<UserService>();

// 🔐 Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 QUAN TRỌNG: phải có Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

// Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Tạo account Admin/123456 trên DB nếu chưa có
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FPTPlayContext>();

    // Nếu chưa có user nào thì tạo admin
    try
    {
        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Mobile = "0123456789",
                PasswordHash = Hash.HashPassword("123456"),
                FullName = "Administrator",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.Now
            });

            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

app.Run();