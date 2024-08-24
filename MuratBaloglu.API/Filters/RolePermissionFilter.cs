using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Infrastructure.Operations;
using System.Reflection;

namespace MuratBaloglu.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? name = context.HttpContext.User.Identity?.Name;

            if (!string.IsNullOrEmpty(name) && name != "vedataydogan07@gmail.com")
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                AuthorizeDefinitionAttribute? attribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                var httpAttribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute?.ActionType}.{NameRegulatoryOperation.RegulateCharactersSmallVersion(attribute?.Definition)}";

                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

                if (!hasRole)
                    context.Result = new StatusCodeResult(403);
                else
                    await next();
            }
            else
                await next();
        }
    }
}
