using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class AdministratorController : Controller
    {
        [Authorize]
        [Route("administrator/dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}