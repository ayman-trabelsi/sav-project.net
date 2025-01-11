using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamationController : ControllerBase
    {
        private readonly IReclamationRepository _repo;
        public ReclamationController (IReclamationRepository repo)
        {
            _repo = repo;
        }
      
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.GetAll());
        }
        [HttpPost]
        public IActionResult Add(Reclamation reclamation)
        {
            var result = _repo.AddReclamation(reclamation);
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
