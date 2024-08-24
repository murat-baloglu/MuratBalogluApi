using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Models.WorkingHours;
using MuratBaloglu.Application.Repositories.WorkingHourRepository;
using MuratBaloglu.Domain.Entities;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingHoursController : ControllerBase
    {
        private readonly IWorkingHourReadRepository _workingHourReadRepository;
        private readonly IWorkingHourWriteRepository _workingHourWriteRepository;

        public WorkingHoursController(IWorkingHourReadRepository workingHourReadRepository, IWorkingHourWriteRepository workingHourWriteRepository)
        {
            _workingHourReadRepository = workingHourReadRepository;
            _workingHourWriteRepository = workingHourWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var workingHour = await _workingHourReadRepository.GetAll(false).Select(wh => new
                {
                    wh.Monday,
                    wh.Tuesday,
                    wh.Wednesday,
                    wh.Thursday,
                    wh.Friday,
                    wh.Saturday,
                    wh.Sunday
                }).FirstOrDefaultAsync();

                return Ok(workingHour);
            }

            return BadRequest(new { Message = "Çalışma saatleri getirilemiyor." });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.WorkingHours, ActionType = ActionType.Writing, Definition = "Çalışma Saatleri Ekleme veya Güncelleme")]
        public async Task<IActionResult> Post(WorkingHourAddModel workingHourAddModel)
        {
            if (ModelState.IsValid)
            {
                var query = await _workingHourReadRepository.GetAll(false).FirstOrDefaultAsync();

                if (query is null)
                {
                    WorkingHour workingHour = new WorkingHour()
                    {
                        Monday = workingHourAddModel.Monday,
                        Tuesday = workingHourAddModel.Tuesday,
                        Wednesday = workingHourAddModel.Wednesday,
                        Thursday = workingHourAddModel.Thursday,
                        Friday = workingHourAddModel.Friday,
                        Saturday = workingHourAddModel.Saturday,
                        Sunday = workingHourAddModel.Sunday
                    };

                    await _workingHourWriteRepository.AddAsync(workingHour);
                    await _workingHourWriteRepository.SaveAsync();
                    return Ok(workingHourAddModel);
                }

                query.Monday = workingHourAddModel?.Monday;
                query.Tuesday = workingHourAddModel?.Tuesday;
                query.Wednesday = workingHourAddModel?.Wednesday;
                query.Thursday = workingHourAddModel?.Thursday;
                query.Friday = workingHourAddModel?.Friday;
                query.Saturday = workingHourAddModel?.Saturday;
                query.Sunday = workingHourAddModel?.Sunday;

                _workingHourWriteRepository.Update(query);
                await _workingHourWriteRepository.SaveAsync();
                return Ok(workingHourAddModel);

            }

            return BadRequest(new { Message = "Çalışma saatleri eklenirken bir hata ile karşılaşıldı." });
        }
    }
}
