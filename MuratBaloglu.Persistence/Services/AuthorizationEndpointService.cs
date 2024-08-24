using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.Abstractions.Services.Configurations;
using MuratBaloglu.Application.Repositories.EndpointRepository;
using MuratBaloglu.Application.Repositories.MenuRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Persistence.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        private readonly IApplicationService _applicationService;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IEndpointWriteRepository _endpointWriteRepository;
        private readonly IMenuReadRepository _menuReadRepository;
        private readonly IMenuWriteRepository _menuWriteRepository;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthorizationEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<AppRole> roleManager)
        {
            _applicationService = applicationService;
            _endpointReadRepository = endpointReadRepository;
            _endpointWriteRepository = endpointWriteRepository;
            _menuReadRepository = menuReadRepository;
            _menuWriteRepository = menuWriteRepository;
            _roleManager = roleManager;
        }

        public async Task AssignRoleToEndpointAsync(string[] roles, string menuName, string code, Type type)
        {
            Menu menu = await _menuReadRepository.GetSingleAsync(m => m.Name == menuName);
            if (menu == null)
            {
                menu = new Menu
                {
                    Id = Guid.NewGuid(),
                    Name = menuName,
                };

                await _menuWriteRepository.AddAsync(menu);

                await _menuWriteRepository.SaveAsync();
            }

            //Endpoint endpoint = await _endpointReadRepository.GetSingleAsync(e => e.Code == code);
            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.Menu)
                .Include(e => e.AppRoles)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menuName);

            if (endpoint == null)
            {
                var action = _applicationService.GetAuthorizeDefinitionEndpoints(type)
                    .FirstOrDefault(m => m.Name == menuName)?
                    .Actions.FirstOrDefault(e => e.Code == code);

                endpoint = new Endpoint
                {
                    Id = Guid.NewGuid(),
                    //Code = action.Code - buda olur
                    Code = code,
                    ActionType = action.ActionType,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    Menu = menu
                };

                await _endpointWriteRepository.AddAsync(endpoint);
                await _endpointWriteRepository.SaveAsync();
            }

            foreach (var role in endpoint.AppRoles)
            {
                endpoint.AppRoles.Remove(role);
            }

            var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

            foreach (var role in appRoles)
            {
                endpoint.AppRoles.Add(role);
            }

            await _endpointWriteRepository.SaveAsync();
        }

        public async Task<List<string>> GetRolesOfEndpointAsync(string code, string menuName)
        {
            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.AppRoles)
                .Include(e => e.Menu)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menuName);

            if (endpoint != null)
                return endpoint.AppRoles.Select(r => r.Name).ToList();

            return new List<string>();

            //else
            //    throw new NotFoundUserException("Endpointin rollerini getirirken bir hata ile karşılaşıldı.");
            //return null; //Yada burayi baska turlude yapabiliriz. Tekrar bak
        }
    }
}
