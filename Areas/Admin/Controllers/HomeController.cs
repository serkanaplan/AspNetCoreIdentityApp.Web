﻿using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers;


[Authorize(Roles = "admin")]
[Area("Admin")]
public class HomeController(UserManager<AppUser> userManager) : Controller
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public IActionResult Index() => View();

    public async Task<IActionResult> UserList()
    {
        var userList = await _userManager.Users.ToListAsync();

        var userViewModelList = userList.Select(x => new UserViewModel()
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.UserName
        }).ToList();

        return View(userViewModelList);
    }
}
