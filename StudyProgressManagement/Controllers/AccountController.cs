using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using StudyProgressManagement.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.ReturnUrl = "/";
            return View();
        }

        //
        // POST: /Account/SignIn
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/Account/SignInCallBack" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        //
        // GET: /Account/SignInCallBack
        public async Task<ActionResult> SignInCallBack()
        {
            // Get user information
            var user = new ApplicationUser
            {
                Email = User.Identity.Name,
                UserName = User.Identity.Name,
            };

            // Check if user exists
            var currentUser = await UserManager.FindByEmailAsync(user.Email);
            if (currentUser != null)
            {
                if (currentUser.Roles.Count != 0)
                {
                    // Add role claim to user
                    ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

                    var currentRole = await UserManager.GetRolesAsync(currentUser.Id);
                    identity.AddClaim(new Claim(ClaimTypes.Role, currentRole[0]));
                    IOwinContext context = HttpContext.GetOwinContext();

                    context.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    context.Authentication.SignIn(identity);
                }
            }
            else
            {
                // Create new user
                await UserManager.CreateAsync(user);
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/SignOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            /// Send an OpenID Connect sign-out request.
            HttpContext.GetOwinContext()
                        .Authentication
                        .SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}