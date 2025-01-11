using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAuthentificationRepository _repo;
        public AuthentificationController(IAuthentificationRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Authentifier(UtilisateurDto _userData)
        {
            var result = _repo.Authentifier(_userData);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

