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

     [Route("Menu/Items")]
    public async Task<IActionResult> Items(int categoryId , string searchTerm =" " )
    {
        var categories = _userMenu.GetCategories();
        var items = await _userMenu.GetItemsByCategory(categories.First().CategoryId,searchTerm);
        ViewBag.Items = items;
        ViewBag.SearchTerm = searchTerm;
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

    [Route("Menu/ItemsByCategory")]
    public async Task<IActionResult> ItemsByCategory(int id , string searchTerm =" " )
    {
        var items = await _userMenu.GetItemsByCategory(id,searchTerm);
        return PartialView("_ItemsPartial", items);
    }


    public IActionResult Modifiers()
    {
        return View();
    }
}
    