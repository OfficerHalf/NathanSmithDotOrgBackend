using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NathanSmithDotOrgBackend.Auth;

namespace NathanSmithDotOrgBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AuthHelper _helper;
        public AccountController(AuthHelper helper)
        {
            _helper = helper;
        }

        // POST api/account/create
        [HttpPost("create")]
        public ActionResult<string> Create([FromBody] UserAndPass combo)
        {
            string userId = _helper.CreateAccount(combo.username, combo.password);
            if (userId == null)
                return StatusCode(409);
            return userId;
        }

        // Post api/account/login
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] UserAndPass combo)
        {
            string userId = _helper.Login(combo.username, combo.password);
            if (userId == null)
                return BadRequest();

            // Add session
            HttpContext.Session.SetString("_userId", userId);

            return userId;
        }

        [HttpGet("logout")]
        public void Logout()
        {
            HttpContext.Session.Remove("_userId");
        }

        // GET api/account
        [HttpGet]
        public ActionResult<List<string>> List()
        {
            return _helper.ListAccounts();
        }

        // GET api/account/status
        [HttpGet("status")]
        public ActionResult<string> Status()
        {
            return HttpContext.Session.GetString("_userId");
        }
    }

    public class UserAndPass
    {
        public string username;
        public string password;
    }
}
