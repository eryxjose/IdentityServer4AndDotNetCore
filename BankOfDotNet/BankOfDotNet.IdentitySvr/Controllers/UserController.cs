using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankOfDotNet.IdentitySvr.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankOfDotNet.IdentitySvr.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult AddAppUser()
        //{
        //    AppUser user = new AppUser();
        //    user.Email = 
        //}

    }
}