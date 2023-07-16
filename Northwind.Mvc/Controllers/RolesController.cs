﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Mvc.Controllers
{
    public class RolesController : Controller
    {
        private string AdminRole = "Administrators";
        private string UserEmail = "test@example.com";
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!(await _roleManager.RoleExistsAsync(AdminRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(AdminRole));
            }
            IdentityUser user = await _userManager.FindByEmailAsync(UserEmail);
            if (user is null)
            {
                user = new();
                user.Email = UserEmail;
                user.UserName = UserEmail;

                IdentityResult result = await _userManager.CreateAsync(user, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    WriteLine($"User {user.UserName} created is successfully.");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        WriteLine(error.Description);
                    }
                }
            }

            if (!user.EmailConfirmed)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    WriteLine($"User {user.UserName} email confirmed successfully.");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        WriteLine(error.Description);
                    }
                }
            }
            if(!(await _userManager.IsInRoleAsync(user,AdminRole)))
            {
                IdentityResult result = await _userManager.AddToRoleAsync(user,AdminRole);

                if(result.Succeeded)
                {
                    WriteLine($"User {user.UserName} added to {AdminRole} successfully.");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        WriteLine(error.Description);
                    }
                }
            }
            return Redirect("/");
        }
    }
}
