using AspNetCoreGeneratedDocument;
using BAL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class RolesAndPermissions : Controller
{
    private readonly IUserRolesAndPermissions _userRolesAndPermissions;

    public RolesAndPermissions(IUserRolesAndPermissions userRolesAndPermissions)
    {
       
        _userRolesAndPermissions = userRolesAndPermissions;
    }


    public IActionResult Roles()
    {   
        var role = _userRolesAndPermissions.GetRoles();
        return View(role);
    }

    public async Task<IActionResult> Permissions(int id)
    {
        var Permissions = await _userRolesAndPermissions.GetPermissions(id);

        ViewBag.RoleName = Permissions.First().RoleName;

        return View(Permissions);
    }

    [HttpPost]
    public async Task<IActionResult> Permissions(List<RolesAndPermissionsviewmodel> user)
    {
        await _userRolesAndPermissions.UpdatePermissions(user);
        return RedirectToAction("UserList","User"); 
    }
   

}               
