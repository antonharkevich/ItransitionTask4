using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CustomIdentityApp.Models;
using CustomIdentityApp.ViewModels;
using System;

namespace CustomIdentityApp.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index() => View(_userManager.Users.ToList());


        
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
           


            int identificator = 0;

            foreach (var user in _userManager.Users.ToList())
            {
                if(user.IsChecked == true)
                {
                    User ouruser = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (ouruser == user)
                    {
                        IdentityResult result = await _userManager.DeleteAsync(user);
                        await _signInManager.SignOutAsync();
                        identificator = 1;
                    }
                    else
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                       
                        IdentityResult result = await _userManager.DeleteAsync(user);
                    }
                }
                
            }
            if (identificator == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]

        public async Task<ActionResult> Block(string id)
        {
            


            int identificator = 0;

            foreach (var user in _userManager.Users.ToList())
            {
                if (user.IsChecked == true)
                {
                    User ouruser = await _userManager.FindByNameAsync(User.Identity.Name);
                    user.Status = "blocked";
                    user.IsBlocked = true;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (ouruser == user)
                        {
                            await _signInManager.SignOutAsync();
                        }
                        else
                        {
                            await _userManager.UpdateSecurityStampAsync(user);
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

            }
            if (identificator == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [Authorize]

        public async Task<ActionResult> UnBlock(string id)
        {
            foreach (var user in _userManager.Users.ToList())
            {
                if (user.IsChecked == true)
                {
                    user.Status = "unblocked";
                    user.IsBlocked = false;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

            }
            return RedirectToAction("Index");
            



        }
        [Authorize]
        public async Task<ActionResult> Select(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            user.IsChecked = true;
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> SelectAll()
        {
            foreach (var user in _userManager.Users.ToList())
            {
                user.IsChecked = true;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
               
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> RemoveAll()
        {
            foreach (var user in _userManager.Users.ToList())
            {
                user.IsChecked = false;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }


            }
            return RedirectToAction("Index");
        }


        [Authorize]
        public async Task<ActionResult> Remove(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            user.IsChecked = false;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
