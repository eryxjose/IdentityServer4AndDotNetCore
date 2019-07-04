using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankOfDotNet.IdentitySvr.Models;
using BankOfDotNet.IdentitySvr.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankOfDotNet.IdentitySvr.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;[HttpPost]

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return new ObjectResult(new
                {
                    code = StatusCodes.Status400BadRequest,
                    details = "",
                    message = "",
                    messageToken = "invalidBody",
                });
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {

                if (result.Errors.FirstOrDefault().Code == "DuplicateUserName")
                {
                    return new ObjectResult(result.Errors.FirstOrDefault()) { StatusCode = StatusCodes.Status409Conflict };
                }

                return new ObjectResult(new
                {
                    code = StatusCodes.Status400BadRequest,
                    details = result.Errors,
                    message = "",
                    messageToken = "invalidUserOrPassword",
                })
                { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                //_logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                await _signInManager.SignInAsync(user, isPersistent: false);
                //_logger.LogInformation("User created a new account with password.");

                return new JsonResult(new
                {
                    id = user.Id,
                });
            }

        }
        public IActionResult Index()
        {
            return View();
        }

    }
}