using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PieceRechangeController : ControllerBase
    {
        private readonly IPieceRechangeRepository _repo;
        public PieceRechangeController(IPieceRechangeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.GetAll());
        }
        [HttpPost]
        public IActionResult Add(PieceRechange piece)
        {
            var result = _repo.AddPieceRechange(piece);
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
