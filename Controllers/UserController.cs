using BAL.Interfaces;
using DAL.Models;

// using DAL.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
// using Pizzashop.DAL.Models;

namespace Pizzashop.Presentation.Controllers;

public class UserController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserList _userList;

    public UserController(IConfiguration configuration, IUserList userList)
    {
        _configuration = configuration;
        _userList = userList;
    }

    public async Task<IActionResult> UserList(int pageNumber = 1, int pageSize = 3, string searchTerm = "")
    {

        var userList = await _userList.GetUserList(pageNumber, pageSize, searchTerm);
        int userCount = await _userList.GetUsercount(searchTerm);

        ViewBag.TotalPages = (int)Math.Ceiling((double)userCount / pageSize);
        ViewBag.CurrentPage = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalItems = userCount;
        ViewBag.SearchTerm = searchTerm;

        return View(userList);

    }

    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(User user)
    {
        string token = Request.Cookies["jwtToken"];

        await _userList.AddUserAsync(user);

        TempData["AddUserSuccess"] = true;

        return RedirectToAction("UserList", "User");

    }

    public async Task<IActionResult> EditUser(int UserId)
    {

        var user = await _userList.GetUserById(UserId);

        // if(user == null)
        // {
        //     return NotFound();
        // }

        ViewData["Country"] = new SelectList(await _userList.GetCountriesAsync() , "Countryid", "Countryname", user.Country);
        ViewData["State"] = new SelectList(await _userList.GetStatesAsync(), "Stateid", "Statename", user.State);
        ViewData["City"] = new SelectList(await _userList.GetCountriesAsync() , "Cityid", "Cityname", user.City);
        ViewData["Userrole"] = new SelectList(await _userList.GetRolesAsync(), "Roleid", "Rolename", user.Userrole);


        return View(user);
    }

    [HttpPost]

    public async Task<IActionResult> EditUser(User user)
    {
        await _userList.UpdateUserAsync(user); 

        TempData["EditUserSuccess"] = true;

        return RedirectToAction("userList","User");
    }

    [HttpPost]
    
    public async Task<IActionResult> SoftDelete(int UserId)
    {
        var existinguser = await _userList.GetById(UserId);
        TempData["DeleteUserSuccess"] = true;
        return RedirectToAction("UserList","User");
    }

}
