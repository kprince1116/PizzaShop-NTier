using BAL.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Pizzashop.DAL.ViewModels;

namespace Pizzashop.Presentation.Controllers;

public class MenuController : Controller
{

    private readonly IUserMenu _userMenu;

    public MenuController(IUserMenu userMenu)
    {
        _userMenu = userMenu;
    }
    public async Task<IActionResult> Items()
    {
        var categories = _userMenu.GetCategories();
        var items = await _userMenu.GetItemsByCategory(categories.First().CategoryId); 
        ViewBag.Items = items;     
         return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(Categoryviewmodel model)
    {
        var isAdded = await _userMenu.AddCategory(model);
        if(isAdded)
        {
            return RedirectToAction("Index","Home");
        }
        else{
            return Content("error");
        }
    }

    public async Task<IActionResult> ItemsByCategory(int id)
    {
        var items = await _userMenu.GetItemsByCategory(id);
        return PartialView("_ItemsPartial", items);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Categoryviewmodel model)
    {
        await _userMenu.UpdateCategory(model);
        return RedirectToAction("Index","Home");
    }
    

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _userMenu.GetModifierById(id);
    
        return RedirectToAction("Index","Home");

    }

    public IActionResult Modifiers()
    {
        return View();
    }
}
    