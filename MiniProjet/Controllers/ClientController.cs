using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.Repository.IRepository;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_clientRepository.GetClients());
        }
        [HttpPost]
        public IActionResult AddClient(Client client)
        {
            var result = _clientRepository.AddClient(client);
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
