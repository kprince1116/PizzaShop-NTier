using Microsoft.AspNetCore.Mvc;
using Pizzashop.BAL.Interfaces;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class LoginController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public LoginController(IConfiguration configuration, IAuthService authService)
    {
        _configuration = configuration;
        _authService = authService;
    }

    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(Request.Cookies["Email"]))
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(Loginviewmodel user)
    {
        // if (!ModelState.IsValid)
        // {
        //     return View(user);
        // }
        try
        {
            var result = await _authService.AuthenticateUserAsync(user);

            // Response.Cookies.Append("jwtToken", result, new CookieOptions
            // {
            //     Expires = DateTime.UtcNow.AddDays(7),
            //     HttpOnly = true,
            //     Secure = true,
            //     SameSite = SameSiteMode.Strict
            // });

            // if (user.RememberMe)
            // {
            //     Response.Cookies.Append("Email", user.Email, new CookieOptions
            //     {
            //         Expires = DateTime.UtcNow.AddDays(7),
            //         HttpOnly = true,
            //         Secure = false,
            //         SameSite = SameSiteMode.Lax
            //     });
            // }

            _authService.SetJwtToken(Response, result);

            if(user.RememberMe)
            {
                _authService.SetCookie(Response, user.Email);
            }

            TempData["LoginSuccess"] = true;
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View(user);
        }
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _authService.SendMail(email);

        if (user == false)
        {
            ModelState.AddModelError("email", "User does not exists");
            return View();
        }
        // ViewBag.Message = "An email has been sent to reset your password.";
        TempData["EmailSuccess"] = true;

        return View();
    }

    public IActionResult ResetPassword(string? email)
    {
        return View();
    }

    public async Task<IActionResult> ResetPasswrord(ResetPasswordviewmodel model)
    {
        var result = await _authService.ResetPassword(model);

        return RedirectToAction("Login","Login");
    }


 

}


