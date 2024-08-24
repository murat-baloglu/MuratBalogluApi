using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MuratBaloglu.Application.Abstractions.Services.Configurations;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.DTOs.Configurations;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Infrastructure.Operations;
using System.Reflection;

namespace MuratBaloglu.Infrastructure.Services.Configurations
{
    public class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
        {
            Assembly? assembly = Assembly.GetAssembly(type);
            var controllers = assembly?.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<Menu> menus = new List<Menu>();

            if (controllers is not null)
            {
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if (actions is not null)
                    {
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes is not null)
                            {
                                Menu? menu = null;

                                var authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                if (authorizeDefinitionAttribute != null)
                                {
                                    if (!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
                                    {
                                        menu = new Menu { Name = authorizeDefinitionAttribute.Menu };
                                        menus.Add(menu);
                                    }
                                    else
                                        menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute.Menu);

                                    Application.DTOs.Configurations.Action _action = new()
                                    {
                                        //ActionType = authorizeDefinitionAttribute.ActionType,
                                        //Enum ın string değerini elde etmek için
                                        ActionType = Enum.GetName(typeof(ActionType), authorizeDefinitionAttribute.ActionType),
                                        Definition = authorizeDefinitionAttribute.Definition
                                    };

                                    var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                                    if (httpAttribute != null)
                                        _action.HttpType = httpAttribute.HttpMethods.First();
                                    else
                                        _action.HttpType = HttpMethods.Get;

                                    _action.Code = $"{_action.HttpType}.{_action.ActionType}.{NameRegulatoryOperation.RegulateCharactersSmallVersion(_action.Definition)}";

                                    menu?.Actions.Add(_action);
                                }
                            }
                        }
                    }
                }
            }

            return menus;
        }
    }
}
