using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DAL.Models;
using Pizzashop.BAL.Interfaces;
using BAL.Services;
using DAL.Interfaces;
using Pizzashop.Data.Repositories;
using BAL.Interfaces;
using DAL.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserDetails, UserDetails>();
builder.Services.AddScoped<IUserList, UserList>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRolesAndPermissions, UserRolesAndPermissions>();
builder.Services.AddScoped<IUserRolesAndPermissionsRepository, UserRolesAndPermissionsRepository>();
builder.Services.AddScoped<IUserMenu, UserMenu>();
builder.Services.AddScoped<IUserMenuRepository, UserMenuRepository>();

builder.Services.AddControllersWithViews();

var conn = builder.Configuration.GetConnectionString("defaults");
builder.Services.AddDbContext<PizzaShopContext>(options => options.UseNpgsql(conn));


builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
   
                var token = context.Request.Cookies["jwtToken"]; 
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
