using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FPTPlay.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BaseAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                context.Result = RedirectToAction("Login", "Account", new { area = "" });
            }

            base.OnActionExecuting(context);
        }
    }
}
