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
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public async Task<ActionResult> SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }

            var user = new ApplicationUser
            {
                Email = User.Identity.Name,
                UserName = User.Identity.Name,
            };

            var currentUser = await UserManager.FindByEmailAsync(user.Email);
            if (currentUser.Roles.Count != 0)
            {
                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

                var currentRole = await UserManager.GetRolesAsync(currentUser.Id);
                if (currentRole[0] == "Faculty")
                {
                    // Add Faculty role claim to user
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Faculty"));
                }
                else
                {
                    // Add Admin role claim to user
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }
                IOwinContext context = HttpContext.GetOwinContext();

                context.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                context.Authentication.SignIn(identity);
            }


            var result = await UserManager.CreateAsync(user);

            if (result.Succeeded)
            {
                // Sign in after create Succeeded
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            // Sign in the user if user already had account
            await SignInManager.SignInAsync(currentUser, isPersistent: false, rememberBrowser: false);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext()
                        .Authentication
                        .SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return RedirectToAction("Index", "Home");
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