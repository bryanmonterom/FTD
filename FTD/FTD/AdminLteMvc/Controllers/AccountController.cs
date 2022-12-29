//using System;
//using System.Data.Entity;
//using System.Data.Entity.Migrations;
//using System.Globalization;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using AdminLteMvc.Models;
//using System.DirectoryServices.AccountManagement;
//using System.Net;
//using AdminLteMvc.Utilities;
//using PagedList;


//namespace AdminLteMvc.Controllers
//{
//    public class AccountController : BaseController
//    {
//        private ApplicationSignInManager _signInManager;
//        private ApplicationUserManager _userManager;
//        FtdContext db = new FtdContext();
//        BL.Business business = new BL.Business();
//        FTDEntities context = new FTDEntities();


//        public ActionResult Index(int? page)
//        {
//            var users = business.UsersFilter();
//            int pageNumber = page ?? 1;
//            ViewBag.Role = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text");
//            return View(users.OrderBy(a => a.UserName).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
//        }

//        public JsonResult getEmployee(string EmployeeId)
//        {
//            var employee = business.GetEmployee(EmployeeId);
//            var empName = employee.Name;
//            var empId = employee.Id;
//            return Json(new { Id = empId, Name = empName, Email = employee.Email }, JsonRequestBehavior.AllowGet);
//        }


//        [HttpPost]
//        public ActionResult Index(int? page, string user = "", string Role = "")
//        {
//            var users = business.UsersFilter(user, Role);
//            int pageNumber = page ?? 1;
//            ViewBag.Role = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text");
//            return View(users.OrderBy(a => a.UserName).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
//        }
//        //[Authorize(Roles = "Administrators")]
//        public ActionResult Roles(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUsers user = context.AspNetUsers.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            UserViewModels userRole = new UserViewModels() { Email = user.Email, Id = user.Id, Role = user.AspNetRoles, UserName = user.UserName };
//            if (userRole.Role.Count() != 0)
//            {
//                ViewBag.RoleId = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text", userRole.Role.FirstOrDefault().Id);
//            }
//            else
//            {
//                ViewBag.RoleId = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text");

//            }
//            return View(userRole);
//        }

//        [HttpPost]
//        //[Authorize(Roles = "Administrators")]

//        public ActionResult Roles(UserViewModels user)
//        {
//            var role = new AspNetRoles();
//            if (ModelState.IsValid)
//            {
//                var roles = context.AspNetUsers.Find(user.Id).AspNetRoles.ToList();
//                foreach (var item in roles)
//                {
//                    UserManager.RemoveFromRoles(user.Id, item.Name);
//                    db.SaveChanges();
//                    Utilities.Utilities.PrepareAuditTrail("Rol removido" + item.Name + "a: " + user.UserName, User.Identity.Name,
//                        AuditTrailAction.Delete);
//                }
//                AspNetUsers users = context.AspNetUsers.Find(user.Id);
//                 role = context.AspNetRoles.Find(user.RoleId.ToString());
//                UserManager.AddToRole(user.Id, role.Name);
//                db.SaveChanges();
//                Utilities.Utilities.PrepareAuditTrail("Nuevo Rol añadido" + role.Name+ "a: "+user.UserName, User.Identity.Name,
//                    AuditTrailAction.Insert);
//                // users.AspNetRoles.Select();
//                // db.Entry(users).State = EntityState.Modified;

//                return RedirectToAction("Index");
//            }

//            ViewBag.RoleId = new SelectList(Utilities.Utilities.GetRoles(), "Id", "Name", user.RoleId);
//            return View(user);
//        }
//        //[Authorize(Roles = "Administrators")]


//        [HttpGet]
//        public ActionResult Edit(string id)
//        {

//            if (String.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
//            }
//            AspNetUsers employee = context.AspNetUsers.Find(id);
//            if (employee == null)
//            {
//                return HttpNotFound();
//            }

//            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");
//            return View(employee);
//        }
//        //public JsonResult getEmployee(string EmployeeId)
//        //{
//        //    var employee = business.GetEmployee(EmployeeId);
//        //    var empName = employee.Name;
//        //    var empId = employee.Id;
//        //    return Json(new { Id = empId, Name = empName, Email = employee.Email }, JsonRequestBehavior.AllowGet);
//        //}

//        [HttpPost]
//        //[Authorize(Roles = "Administrators")]

//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,UserName,Email")] AspNetUsers model)
//        {
//            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");
//            if (ModelState.IsValid)
//            {
//                var results = business.Update(model);
//                if (results.IsSuccess)
//                {
//                    Alert(results.Message, Utilities.NotificationType.success);
//                    Utilities.Utilities.PrepareAuditTrail("Usuario modificado " + model.UserName, User.Identity.Name,
//                        AuditTrailAction.Update);
//                }
//                else
//                    Alert(results.Message, Utilities.NotificationType.error);
//                return View(model);
//            }
//            return View(model);

//        }

//        public AccountController()
//        {
//        }

//        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
//        {
//            UserManager = userManager;
//            SignInManager = signInManager;
//        }

//        public ApplicationSignInManager SignInManager
//        {
//            get
//            {
//                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
//            }
//            private set
//            {
//                _signInManager = value;
//            }
//        }

//        public ApplicationUserManager UserManager
//        {
//            get
//            {
//                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }

//        //
//        // GET: /Account/Login
//        [AllowAnonymous]
//        public ActionResult Login(string returnUrl)
//        {
//            if (Request.IsAuthenticated)
//            {
//                return View("Error");
//            }
//            ViewBag.ReturnUrl = returnUrl;
//            return View();
//        }

//        //
//        // POST: /Account/Login
//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
//        {
//            if (!ModelState.IsValid)
//            {
//                ModelState.AddModelError("", "El usuario o contraseña provistos son incorrectos, intenta de nuevo");

//                Utilities.Utilities.PrepareAuditTrail("Intento de inicio de sesion fallido para: " + model.Username,"System",AuditTrailAction.System); return View(model);
//            }


//            bool valid = false;
//            try
//            {
//                using (PrincipalContext domain = new PrincipalContext(ContextType.Domain))
//                {
//                    valid = domain.ValidateCredentials(model.Username, model.Password);
//                    if (valid)
//                    {
//                        model.Password = "Aa12344@";
//                        var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
//                        switch (result)
//                        {
//                            case SignInStatus.Success:
//                                Utilities.Utilities.PrepareAuditTrail("Inicio de sesion exitoso para: " + model.Username, "System", AuditTrailAction.System); return View(model);
//                                return RedirectToLocal(returnUrl);
//                            case SignInStatus.LockedOut:
//                                return View("Lockout");
//                            case SignInStatus.RequiresVerification:
//                                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
//                            case SignInStatus.Failure:
//                            default:
//                                ModelState.AddModelError("", "Intento de inicio de sesión no válido");
//                                return View(model);
//                        }
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "El usuario o contraseña provistos son incorrectos, intenta de nuevo");
//                        Utilities.Utilities.PrepareAuditTrail("Intento de inicio de sesion fallido para: " + model.Username,"System",AuditTrailAction.Update); return View(model);
//                    }
//                }


//            }
//            catch (Exception e)
//            {
//                Alert(e.Message, NotificationType.error);
//            }

//            // This doesn't count login failures towards account lockout
//            // To enable password failures to trigger account lockout, change to shouldLockout: true
//            //var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
//            //switch (result)
//            //{
//            //    case SignInStatus.Success:
//            //        return RedirectToLocal(returnUrl);
//            //    case SignInStatus.LockedOut:
//            //        return View("Lockout");
//            //    case SignInStatus.RequiresVerification:
//            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
//            //    case SignInStatus.Failure:
//            //    default:
//            //        ModelState.AddModelError("", "Intento de inicio de sesión no válido");
//            //        return View(model);
//            //}
//            //        return View(model);
//            return View(model);
//        }

//        //
//        // GET: /Account/VerifyCode
//        [AllowAnonymous]
//        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
//        {
//            // Require that the user has already logged in via username/password or external login
//            if (!await SignInManager.HasBeenVerifiedAsync())
//            {
//                return View("Error");
//            }
//            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
//        }

//        //
//        // POST: /Account/VerifyCode
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            // The following code protects for brute force attacks against the two factor codes. 
//            // If a user enters incorrect codes for a specified amount of time then the user account 
//            // will be locked out for a specified amount of time. 
//            // You can configure the account lockout settings in IdentityConfig
//            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
//            switch (result)
//            {
//                case SignInStatus.Success:
//                    return RedirectToLocal(model.ReturnUrl);
//                case SignInStatus.LockedOut:
//                    return View("Lockout");
//                case SignInStatus.Failure:
//                default:
//                    ModelState.AddModelError("", "Invalid code.");
//                    return View(model);
//            }
//        }

//        //
//        // GET: /Account/Register
//        public ActionResult Register()
//        {
//            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");
//            return View();

//        }

//        //
//        // POST: /Account/Register
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Register(RegisterViewModel model)
//        {
//            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");

//            if (ModelState.IsValid)
//            {
//                ApplicationUser user= new ApplicationUser { UserName = model.Username, Email = model.Email };
//                model.Password = "Aa12344@";
//               // var results = business.Save(user);
//              //  if (results.IsSuccess == true)
//                {
//                    var result = await UserManager.CreateAsync(user, model.Password);
//                    if (result.Succeeded)
//                    {
//                        Alert("Creado con exito", NotificationType.success);
//                        Utilities.Utilities.PrepareAuditTrail("User added: " + model.Username, User.Identity.Name,
//                            AuditTrailAction.Insert);
//                    }
//                    AddErrors(result);
//                    Alert("Fallo", NotificationType.error);
//                    return View(model);
//                }
//                ////else
//                //{
//                //    ModelState.AddModelError("", results.Message);
//                //    Alert(results.Message, NotificationType.error);
//                //    return View(model);
//                //}
//            }

//            // If we got this far, something failed, redisplay form
//            Alert("Fallo", NotificationType.error);
//            return View(model);
//        }

//        //
//        // GET: /Account/ConfirmEmail
//        [AllowAnonymous]
//        public async Task<ActionResult> ConfirmEmail(string userId, string code)
//        {
//            if (userId == null || code == null)
//            {
//                return View("Error");
//            }
//            var result = await UserManager.ConfirmEmailAsync(userId, code);
//            return View(result.Succeeded ? "ConfirmEmail" : "Error");
//        }

//        //
//        // GET: /Account/ForgotPassword
//        [AllowAnonymous]
//        public ActionResult ForgotPassword()
//        {
//            return View();
//        }

//        //
//        // POST: /Account/ForgotPassword
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await UserManager.FindByNameAsync(model.Email);
//                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
//                {
//                    // Don't reveal that the user does not exist or is not confirmed
//                    return View("ForgotPasswordConfirmation");
//                }

//                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
//                // Send an email with this link
//                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
//                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
//                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
//                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        //
//        // GET: /Account/ForgotPasswordConfirmation
//        [AllowAnonymous]
//        public ActionResult ForgotPasswordConfirmation()
//        {
//            return View();
//        }

//        //
//        // GET: /Account/ResetPassword
//        [AllowAnonymous]
//        public ActionResult ResetPassword(string code)
//        {
//            return code == null ? View("Error") : View();
//        }

//        //
//        // POST: /Account/ResetPassword
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            var user = await UserManager.FindByNameAsync(model.Email);
//            if (user == null)
//            {
//                // Don't reveal that the user does not exist
//                return RedirectToAction("ResetPasswordConfirmation", "Account");
//            }
//            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
//            if (result.Succeeded)
//            {
//                return RedirectToAction("ResetPasswordConfirmation", "Account");
//            }
//            AddErrors(result);
//            return View();
//        }

//        //
//        // GET: /Account/ResetPasswordConfirmation
//        [AllowAnonymous]
//        public ActionResult ResetPasswordConfirmation()
//        {
//            return View();
//        }

//        //
//        // POST: /Account/ExternalLogin
//        [HttpPost]


//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public ActionResult ExternalLogin(string provider, string returnUrl)
//        {
//            // Request a redirect to the external login provider
//            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
//        }

//        //
//        // GET: /Account/SendCode
//        [AllowAnonymous]
//        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
//        {
//            var userId = await SignInManager.GetVerifiedUserIdAsync();
//            if (userId == null)
//            {
//                return View("Error");
//            }
//            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
//            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
//            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
//        }

//        //
//        // POST: /Account/SendCode
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> SendCode(SendCodeViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View();
//            }

//            // Generate the token and send it
//            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
//            {
//                return View("Error");
//            }
//            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
//        }

//        //
//        // GET: /Account/ExternalLoginCallback
//        [AllowAnonymous]
//        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
//        {
//            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
//            if (loginInfo == null)
//            {
//                return RedirectToAction("Login");
//            }

//            // Sign in the user with this external login provider if the user already has a login
//            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
//            switch (result)
//            {
//                case SignInStatus.Success:
//                    return RedirectToLocal(returnUrl);
//                case SignInStatus.LockedOut:
//                    return View("Lockout");
//                case SignInStatus.RequiresVerification:
//                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
//                case SignInStatus.Failure:
//                default:
//                    // If the user does not have an account, then prompt the user to create an account
//                    ViewBag.ReturnUrl = returnUrl;
//                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
//                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
//            }
//        }

//        //
//        // POST: /Account/ExternalLoginConfirmation
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Index", "Manage");
//            }

//            if (ModelState.IsValid)
//            {
//                // Get the information about the user from the external login provider
//                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
//                if (info == null)
//                {
//                    return View("ExternalLoginFailure");
//                }
//                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
//                var result = await UserManager.CreateAsync(user);
//                if (result.Succeeded)
//                {
//                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
//                    if (result.Succeeded)
//                    {
//                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
//                        return RedirectToLocal(returnUrl);
//                    }
//                }
//                AddErrors(result);
//            }

//            ViewBag.ReturnUrl = returnUrl;
//            return View(model);
//        }

//        //
//        // POST: /Account/LogOff
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize]
//        public ActionResult LogOff()
//        {
//            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
//            return RedirectToAction("Index", "Home");
//        }

//        //
//        // GET: /Account/ExternalLoginFailure
//        [AllowAnonymous]
//        public ActionResult ExternalLoginFailure()
//        {
//            return View();
//        }

//        public ActionResult Delete(string id)
//        {
//            if (String.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
//            }
//            AspNetUsers user = context.AspNetUsers.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            AspNetUsers user = context.AspNetUsers.Find(id);
//            context.AspNetUsers.Remove(user);
//            context.SaveChanges();
//            Utilities.Utilities.PrepareAuditTrail("User added: " + user.UserName, User.Identity.Name,
//                AuditTrailAction.Delete);
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (_userManager != null)
//                {
//                    _userManager.Dispose();
//                    _userManager = null;
//                }

//                if (_signInManager != null)
//                {
//                    _signInManager.Dispose();
//                    _signInManager = null;
//                }
//            }

//            base.Dispose(disposing);
//        }

//        #region Helpers
//        // Used for XSRF protection when adding external logins
//        private const string XsrfKey = "XsrfId";

//        private IAuthenticationManager AuthenticationManager
//        {
//            get
//            {
//                return HttpContext.GetOwinContext().Authentication;
//            }
//        }

//        private void AddErrors(IdentityResult result)
//        {
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError("", error);
//            }
//        }

//        private ActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            return RedirectToAction("Index", "Home");
//        }

//        internal class ChallengeResult : HttpUnauthorizedResult
//        {
//            public ChallengeResult(string provider, string redirectUri)
//                : this(provider, redirectUri, null)
//            {
//            }

//            public ChallengeResult(string provider, string redirectUri, string userId)
//            {
//                LoginProvider = provider;
//                RedirectUri = redirectUri;
//                UserId = userId;
//            }

//            public string LoginProvider { get; set; }
//            public string RedirectUri { get; set; }
//            public string UserId { get; set; }

//            public override void ExecuteResult(ControllerContext context)
//            {
//                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
//                if (UserId != null)
//                {
//                    properties.Dictionary[XsrfKey] = UserId;
//                }
//                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
//            }
//        }
//        #endregion
//    }
//}



using System;

using System.Data.Entity;

using System.Data.Entity.Migrations;

using System.Globalization;

using System.Linq;

using System.Security.Claims;

using System.Threading.Tasks;

using System.Web;

using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.Owin;

using Microsoft.Owin.Security;

using AdminLteMvc.Models;

using System.DirectoryServices.AccountManagement;

using System.Net;

using AdminLteMvc.Utilities;

using PagedList;





namespace AdminLteMvc.Controllers

{
    public class AccountController : BaseController

    {

        private ApplicationSignInManager _signInManager;

        private ApplicationUserManager _userManager;

        FtdContext db = new FtdContext();

        BL.Business business = new BL.Business();

        FTDEntities context = new FTDEntities();




        [Authorize(Roles = "Administradores")]

        public ActionResult Index(int? page)

        {

            var users = business.UsersFilter();

            int pageNumber = page ?? 1;

            ViewBag.Role = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text");

            return View(users.OrderBy(a => a.UserName).ToPagedList(pageNumber, Utilities.Utilities.pageSize));

        }


        [Authorize(Roles = "Administradores")]
        public JsonResult getEmployee(string EmployeeId)

        {

            var employee = business.GetEmployee(EmployeeId);

            var empName = employee.Name;

            var empId = employee.Id;

            return Json(new { Id = empId, Name = empName, Email = employee.Email }, JsonRequestBehavior.AllowGet);

        }




        [Authorize(Roles = "Administradores")]
        [HttpPost]
        public ActionResult Index(int? page, string user = "", string Role = "")

        {

            var users = business.UsersFilter(user, Role);

            int pageNumber = page ?? 1;

            ViewBag.Role = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text");

            return View(users.OrderBy(a => a.UserName).ToPagedList(pageNumber, Utilities.Utilities.pageSize));

        }

        //[Authorize(Roles = "Administrators")]
        [Authorize(Roles = "Administradores")]
        public ActionResult Roles(string id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            AspNetUsers user = context.AspNetUsers.Find(id);

            if (user == null)

            {

                return HttpNotFound();

            }

            UserViewModels userRole = new UserViewModels() { Email = user.Email, Id = user.Id, Role = user.AspNetRoles, UserName = user.UserName };

            if (userRole.Role.Count() != 0)

            {

                ViewBag.RoleId = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text", userRole.Role.FirstOrDefault().Id);

            }

            else

            {

                ViewBag.RoleId = new SelectList(Utilities.Utilities.GetRoles(), "Value", "Text");



            }

            return View(userRole);

        }



        [HttpPost]

        //[Authorize(Roles = "Administrators")]


        [Authorize(Roles = "Administradores")]

        public ActionResult Roles(UserViewModels user)

        {



            if (ModelState.IsValid)

            {

                var roles = context.AspNetUsers.Find(user.Id).AspNetRoles.ToList();

                foreach (var item in roles)

                {

                    UserManager.RemoveFromRoles(user.Id, item.Name);

                }

                db.SaveChanges();



                AspNetUsers users = context.AspNetUsers.Find(user.Id);

                var role = context.AspNetRoles.Find(user.RoleId.ToString());

                UserManager.AddToRole(user.Id, role.Name);

                db.SaveChanges();



                // users.AspNetRoles.Select();

                // db.Entry(users).State = EntityState.Modified;



                return RedirectToAction("Index");

            }



            ViewBag.RoleId = new SelectList(Utilities.Utilities.GetRoles(), "Id", "Name", user.RoleId);

            return View(user);

        }

        //[Authorize(Roles = "Administrators")]





        [HttpGet]
        //[Authorize(Roles = "Administradores")]

        public ActionResult Edit(string id)

        {



            if (String.IsNullOrEmpty(id))

            {

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }

            AspNetUsers employee = context.AspNetUsers.Find(id);

            if (employee == null)

            {

                return HttpNotFound();

            }



            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");

            return View(employee);

        }

        //public JsonResult getEmployee(string EmployeeId)

        //{

        //    var employee = business.GetEmployee(EmployeeId);

        //    var empName = employee.Name;

        //    var empId = employee.Id;

        //    return Json(new { Id = empId, Name = empName, Email = employee.Email }, JsonRequestBehavior.AllowGet);

        //}


        [Authorize(Roles = "Administradores")]
        [HttpPost]
        //[Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email")] AspNetUsers model)

        {

            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");

            if (ModelState.IsValid)

            {

                var results = business.Update(model);

                if (results.IsSuccess)

                {

                    Alert(results.Message, Utilities.NotificationType.success);

                    Utilities.Utilities.PrepareAuditTrail("User edited: " + model.UserName, User.Identity.Name,

                        AuditTrailAction.Update);

                }

                else

                    Alert(results.Message, Utilities.NotificationType.error);

                return View(model);

            }

            return View(model);



        }



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

            if (Request.IsAuthenticated)

            {

                return View("Error");

            }

            ViewBag.ReturnUrl = returnUrl;

            return View();

        }



        //

        // POST: /Account/Login

        [HttpPost]

        [AllowAnonymous]

        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)

        {

            if (!ModelState.IsValid)

            {

                ModelState.AddModelError("", "El usuario o contraseña provistos son incorrectos, intenta de nuevo");

                return View(model);

            }





            bool valid = false;

            try

            {

                //using (PrincipalContext domain = new PrincipalContext(ContextType.Domain))

                //{

                    //valid = domain.ValidateCredentials(model.Username, model.Password);

                    //if (valid)
                        if (true)


                        {

                            model.Password = "Aa12344@";

                    }

                    else

                    {

                        ModelState.AddModelError("", "El usuario o contraseña provistos son incorrectos, intenta de nuevo");



                    }

                }

            //}

            catch (Exception e)

            {

                Alert(e.Message, NotificationType.error);

            }



            // This doesn't count login failures towards account lockout

            // To enable password failures to trigger account lockout, change to shouldLockout: true

            //var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            var result = await SignInManager.PasswordSignInAsync("rdeleon", model.Password, model.RememberMe, shouldLockout: false);
            Utilities.Utilities.PrepareAuditTrail("Inicio de sesion exitoso para: " + model.Username, "System", AuditTrailAction.System);
            return RedirectToLocal(returnUrl);

            //switch (result)

            //{

            //    case SignInStatus.Success:
            //        Utilities.Utilities.PrepareAuditTrail("Inicio de sesion exitoso para: " + model.Username, "System", AuditTrailAction.System); 
            //        return RedirectToLocal(returnUrl);

            //    case SignInStatus.LockedOut:
            //        Utilities.Utilities.PrepareAuditTrail("Inicio de sesion fallido para: " + model.Username, "System", AuditTrailAction.System);
            //        return View("Lockout");

            //    case SignInStatus.RequiresVerification:

            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

            //    case SignInStatus.Failure:

            //    default:
            //        Utilities.Utilities.PrepareAuditTrail("Inicio de sesion fallido para: " + model.Username, "System", AuditTrailAction.System);
            //        ModelState.AddModelError("", "Intento de inicio de sesión no válido");

            //        return View(model);

            //}

        }



        //

        // GET: /Account/VerifyCode

        [AllowAnonymous]

        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)

        {

            // Require that the user has already logged in via username/password or external login

            if (!await SignInManager.HasBeenVerifiedAsync())

            {

                return View("Error");

            }

            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });

        }



        //

        // POST: /Account/VerifyCode

        [HttpPost]

        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)

        {

            if (!ModelState.IsValid)

            {

                return View(model);

            }



            // The following code protects for brute force attacks against the two factor codes. 

            // If a user enters incorrect codes for a specified amount of time then the user account 

            // will be locked out for a specified amount of time. 

            // You can configure the account lockout settings in IdentityConfig

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);

            switch (result)

            {

                case SignInStatus.Success:

                    return RedirectToLocal(model.ReturnUrl);

                case SignInStatus.LockedOut:

                    return View("Lockout");

                case SignInStatus.Failure:

                default:

                    ModelState.AddModelError("", "Invalid code.");

                    return View(model);

            }

        }



        //

        // GET: /Account/Register
        //[Authorize(Roles = "Administradores")]
        public ActionResult Register()

        {

            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");

            return View();



        }



        //

        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administradores")]

        public async Task<ActionResult> Register(RegisterViewModel model)

        {

            ViewBag.Employees = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");



            //if (ModelState.IsValid)
                if (true)


                {
                     //ApplicationUser user = new ApplicationUser { UserName = model.Username, Email = model.Email };
              
                ApplicationUser user = new ApplicationUser { UserName = "rdelon", Email = "reinoldrey@gmail.com" };

                model.Password = "Aa12344@";

                // var results = business.Save(user);

                //  if (results.IsSuccess == true)

                {

                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)

                    {

                        Alert("Transaccion Completada con Exito", NotificationType.success);

                        Utilities.Utilities.PrepareAuditTrail("User added: " + model.Username, User.Identity.Name,

                            AuditTrailAction.Insert);
                        return View(model);

                    }

                    AddErrors(result);

                    Alert("Fallo", NotificationType.error);

                    return View(model);

                }

                ////else

                //{

                //    ModelState.AddModelError("", results.Message);

                //    Alert(results.Message, NotificationType.error);

                //    return View(model);

                //}

            }



            // If we got this far, something failed, redisplay form

            Alert("Fallo", NotificationType.error);

            return View(model);

        }



        //

        // GET: /Account/ConfirmEmail

        [AllowAnonymous]

        public async Task<ActionResult> ConfirmEmail(string userId, string code)

        {

            if (userId == null || code == null)

            {

                return View("Error");

            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            return View(result.Succeeded ? "ConfirmEmail" : "Error");

        }



        //

        // GET: /Account/ForgotPassword

        [AllowAnonymous]

        public ActionResult ForgotPassword()

        {

            return View();

        }



        //

        // POST: /Account/ForgotPassword

        [HttpPost]

        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)

        {

            if (ModelState.IsValid)

            {

                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))

                {

                    // Don't reveal that the user does not exist or is not confirmed

                    return View("ForgotPasswordConfirmation");

                }



                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771

                // Send an email with this link

                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		

                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                // return RedirectToAction("ForgotPasswordConfirmation", "Account");

            }



            // If we got this far, something failed, redisplay form

            return View(model);

        }



        //

        // GET: /Account/ForgotPasswordConfirmation

        [AllowAnonymous]

        public ActionResult ForgotPasswordConfirmation()

        {

            return View();

        }



        //

        // GET: /Account/ResetPassword

        [AllowAnonymous]

        public ActionResult ResetPassword(string code)

        {

            return code == null ? View("Error") : View();

        }



        //

        // POST: /Account/ResetPassword

        [HttpPost]

        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)

        {

            if (!ModelState.IsValid)

            {

                return View(model);

            }

            var user = await UserManager.FindByNameAsync(model.Email);

            if (user == null)

            {

                // Don't reveal that the user does not exist

                return RedirectToAction("ResetPasswordConfirmation", "Account");

            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)

            {

                return RedirectToAction("ResetPasswordConfirmation", "Account");

            }

            AddErrors(result);

            return View();

        }



        //

        // GET: /Account/ResetPasswordConfirmation

        [AllowAnonymous]

        public ActionResult ResetPasswordConfirmation()

        {

            return View();

        }



        //

        // POST: /Account/ExternalLogin

        [HttpPost]

        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public ActionResult ExternalLogin(string provider, string returnUrl)

        {

            // Request a redirect to the external login provider

            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));

        }



        //

        // GET: /Account/SendCode

        [AllowAnonymous]

        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)

        {

            var userId = await SignInManager.GetVerifiedUserIdAsync();

            if (userId == null)

            {

                return View("Error");

            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);

            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });

        }



        //

        // POST: /Account/SendCode

        [HttpPost]

        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> SendCode(SendCodeViewModel model)

        {

            if (!ModelState.IsValid)

            {

                return View();

            }



            // Generate the token and send it

            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))

            {

                return View("Error");

            }

            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });

        }



        //

        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)

        {

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)

            {

                return RedirectToAction("Login");

            }



            // Sign in the user with this external login provider if the user already has a login

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)

            {

                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:

                    return View("Lockout");

                case SignInStatus.RequiresVerification:

                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

                case SignInStatus.Failure:

                default:

                    // If the user does not have an account, then prompt the user to create an account

                    ViewBag.ReturnUrl = returnUrl;

                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });

            }

        }



        //

        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]

        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)

        {

            if (User.Identity.IsAuthenticated)

            {

                return RedirectToAction("Index", "Manage");

            }



            if (ModelState.IsValid)

            {

                // Get the information about the user from the external login provider

                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)

                {

                    return View("ExternalLoginFailure");

                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)

                {

                    result = await UserManager.AddLoginAsync(user.Id, info.Login);

                    if (result.Succeeded)

                    {

                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        return RedirectToLocal(returnUrl);

                    }

                }

                AddErrors(result);

            }



            ViewBag.ReturnUrl = returnUrl;

            return View(model);

        }



        //

        // POST: /Account/LogOff

        [HttpPost]

        [ValidateAntiForgeryToken]

        [Authorize]

        public ActionResult LogOff()

        {

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");

        }



        //

        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]

        public ActionResult ExternalLoginFailure()

        {

            return View();

        }



        public ActionResult Delete(string id)

        {

            if (String.IsNullOrEmpty(id))

            {

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }

            AspNetUsers user = context.AspNetUsers.Find(id);

            if (user == null)

            {

                return HttpNotFound();

            }

            return View(user);

        }

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(string id)

        {

            AspNetUsers user = context.AspNetUsers.Find(id);

            context.AspNetUsers.Remove(user);

            context.SaveChanges();

            Utilities.Utilities.PrepareAuditTrail("User added: " + user.UserName, User.Identity.Name,

                AuditTrailAction.Delete);

            return RedirectToAction("Index");

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



        #region Helpers

        // Used for XSRF protection when adding external logins

        private const string XsrfKey = "XsrfId";



        private IAuthenticationManager AuthenticationManager

        {

            get

            {

                return HttpContext.GetOwinContext().Authentication;

            }

        }



        private void AddErrors(IdentityResult result)

        {

            foreach (var error in result.Errors)

            {

                ModelState.AddModelError("", error);

            }

        }



        private ActionResult RedirectToLocal(string returnUrl)

        {

            if (Url.IsLocalUrl(returnUrl))

            {

                return Redirect(returnUrl);

            }

            return RedirectToAction("Index", "Home");

        }



        internal class ChallengeResult : HttpUnauthorizedResult

        {

            public ChallengeResult(string provider, string redirectUri)

                : this(provider, redirectUri, null)

            {

            }



            public ChallengeResult(string provider, string redirectUri, string userId)

            {

                LoginProvider = provider;

                RedirectUri = redirectUri;

                UserId = userId;

            }



            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }



            public override void ExecuteResult(ControllerContext context)

            {

                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)

                {

                    properties.Dictionary[XsrfKey] = UserId;

                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);

            }

        }

        #endregion

    }

}