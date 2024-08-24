using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuratBaloglu.Application.Abstractions.Services.Configurations;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.ApplicationServices, ActionType = ActionType.Reading, Definition = "Yetkilendirme Menüsünü (Tüm Endpointleri) Getirme")]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            try
            {
                var datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
                return Ok(datas);
            }
            catch
            {
                return BadRequest(new { Message = "Yetkilendirme Menüsü listelenirken bir hata ile karşılaşıldı." });
            }
        }
    }
}
