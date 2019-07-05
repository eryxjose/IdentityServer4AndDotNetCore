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
    [Authorize]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


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

            try
            {

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
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //_logger.LogInformation("User created a new account with password.");

                    return new JsonResult(new
                    {
                        id = user.Id,
                    });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                //AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            //_logger.LogInformation("User changed their password successfully.");
            //StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Json("index view");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return Json("login view");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginResult()
        {
            return Json("Login post ");
        }

    }
}