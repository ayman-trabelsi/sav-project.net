using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicienController : ControllerBase
    {
        private readonly ITechnicienRepository _repo;
        public TechnicienController(ITechnicienRepository repo)
        {

            _repo = repo;
        }
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(_repo.GetAll());
        }
        [HttpPost]
        public IActionResult AddTechnicien(Technicien technicien)
        {

            var result = _repo.AddTechnicien(technicien);
            if (result != null)
            {

                return Ok(result);
            }
            else return BadRequest();
        }

    }
}
