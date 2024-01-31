using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication3.Model;
using AspNetCore.ReCaptcha;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

//recaptcha
builder.Services.AddReCaptcha(builder.Configuration.GetSection("GoogleRecaptcha"));

builder.Services.AddIdentity<UserProfile, IdentityRole>(options =>
{

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout duration after the specified number of failed access attempts
    options.Lockout.MaxFailedAccessAttempts = 3; // Number of failed access attempts before an account is locked
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddSignInManager<SignInManager<UserProfile>>()
.AddDefaultTokenProviders();

/*
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredChangePassword = true; // Users must change their password
    options.Password.ChangePasswordOptions = new PasswordChangeOptions
    {
        ChangePasswordInDays = 30, // Users must change their password after 30 days
        ChangePasswordFromLastChange = 5, // Users cannot change their password within 5 days from the last change
    };
});
*/



builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton(builder.Configuration);

var app = builder.Build();
app.UseStatusCodePagesWithRedirects("/Errors/CustomError/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    var session = context.Session;
    var user = context.User;

    // Check if a different user is logged in
    if (user.Identity.IsAuthenticated && session.GetString("UserId") != user.FindFirstValue(ClaimTypes.NameIdentifier))
    {
        // Handle multiple logins, e.g., log out the current user
        await context.SignOutAsync(IdentityConstants.ApplicationScheme);
        context.Response.Redirect("/Login");
        return;
    }

    // Update the stored user id in the session
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

    // Check if the userId is not null before storing it in the session
    if (!string.IsNullOrEmpty(userId))
    {
        // Update the stored user id in the session
        session.SetString("UserId", userId);
    }
    await next();
});



app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();




app.Run();
