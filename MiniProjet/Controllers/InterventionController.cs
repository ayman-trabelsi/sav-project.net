using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionController : ControllerBase
    {
        private readonly IInterventionRepository _repo;
        public InterventionController(IInterventionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.GetAll());
        }
        [HttpPost]
        public IActionResult Add(InterventionRequestDto item)
        {
            var result = _repo.Add(item);
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
