using LibraryManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryManagementSystem.Attributes
{
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;

            if (!session.IsAuthenticated())
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var userRole = session.GetUserRole();
            
            if (_roles.Length > 0 && !_roles.Contains(userRole))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }
        }
    }
}
