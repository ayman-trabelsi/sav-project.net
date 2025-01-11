using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsableSavController : ControllerBase
    {
        private readonly IResponsableSAVRepository _ResponsableSAVRepository;
        public ResponsableSavController(IResponsableSAVRepository ResponsableSAVRepository)
        {
            _ResponsableSAVRepository = ResponsableSAVRepository;
        }
        [HttpGet]
        public IActionResult Get() { 
            return Ok(_ResponsableSAVRepository.GetResponsableSAVs());
        }
        [HttpPost]
        public IActionResult AddResponsableSAV(ResponsableSAV ResponsableSAV)
        {
            var result = _ResponsableSAVRepository.AddResponsableSAV(ResponsableSAV);
            if (result != null)
            {
                return Ok();

            }
            else
            {
                return BadRequest();
            }
        }

    }
}
