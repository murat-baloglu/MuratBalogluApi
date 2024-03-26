using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Models.PatientComments;
using MuratBaloglu.Application.Repositories.PatientCommentRepository;
using MuratBaloglu.Domain.Entities;


namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientCommentsController : ControllerBase
    {
        private readonly IPatientCommentReadRepository _patientCommentReadRepository;
        private readonly IPatientCommentWriteRepository _patientCommentWriteRepository;

        public PatientCommentsController(IPatientCommentReadRepository patientCommentReadRepository, IPatientCommentWriteRepository patientCommentWriteRepository)
        {
            _patientCommentReadRepository = patientCommentReadRepository;
            _patientCommentWriteRepository = patientCommentWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var patientComments = await _patientCommentReadRepository.GetAll(false).ToListAsync();
                return Ok(patientComments);
            }

            return BadRequest("Hasta Yorumları listelenirken bir hata ile karşılaşıldı ...");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLastTwelvePatientComment()
        {
            if (ModelState.IsValid)
            {
                var patientComments = await _patientCommentReadRepository.GetAll(false).OrderByDescending(v => v.CreatedDate).Take(12).ToListAsync();
                return Ok(patientComments);
            }

            return BadRequest("Hasta Yorumları listelenirken bir hata ile karşılaşıldı ...");
        }

        [HttpPost]
        public async Task<IActionResult> Post(PatientCommentAddModel patientCommentAddModel)
        {
            if (ModelState.IsValid)
            {
                PatientComment patientComment = new PatientComment()
                {
                    PatientName = patientCommentAddModel.PatientName.Trim(),
                    Disease = patientCommentAddModel.Disease.Trim(),
                    PatientReview = patientCommentAddModel.PatientReview.Trim()
                };

                bool hasPatientComment = await _patientCommentReadRepository
                    .GetWhere(pc => pc.PatientName.Contains(patientComment.PatientName)
                                    && pc.PatientReview.Contains(patientComment.PatientReview)).AnyAsync();
                if (!hasPatientComment)
                {
                    await _patientCommentWriteRepository.AddAsync(patientComment);
                    await _patientCommentWriteRepository.SaveAsync();
                    return Ok(patientComment);
                }
                else
                    return BadRequest("Aynı hasta ismine ve yorumuna sahip zaten bir hasta yorumu var. Lütfen tekrar kontrol ediniz ...");
            }

            return BadRequest("Hasta yorumu eklenirken bir hata ile karşılaşıldı ...");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                await _patientCommentWriteRepository.RemoveAsync(id);
                await _patientCommentWriteRepository.SaveAsync();
                return Ok(new { Message = "Silme işlemi başarı ile gerçekleşmiştir." });
            }

            return BadRequest("Silme aşamasında bir sorun ile karşılaşıldı..");
        }

        [HttpPut]
        public async Task<IActionResult> Put(PatientComment patientCommentModel)
        {
            if (ModelState.IsValid)
            {
                PatientComment patientComment = await _patientCommentReadRepository.GetByIdAsync(patientCommentModel.Id.ToString());
                patientComment.PatientName = patientCommentModel.PatientName.Trim();
                patientComment.Disease = patientCommentModel.Disease.Trim();
                patientComment.PatientReview = patientCommentModel.PatientReview.Trim();

                await _patientCommentWriteRepository.SaveAsync();

                return Ok(patientComment);
            }

            return BadRequest("Hasta yorumu güncellenirken bir hata ile karşılaşıldı ...");
        }
    }
}
