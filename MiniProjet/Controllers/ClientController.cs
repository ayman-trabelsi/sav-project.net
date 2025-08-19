using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Repository.IRepository;
using Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientRepository clientRepository, ILogger<ClientController> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Getting all clients");
                var clients = _clientRepository.GetClients();
                _logger.LogInformation("Found {Count} clients", clients.Count);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all clients");
                return StatusCode(500, "An error occurred while retrieving clients");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid client ID: {Id}", id);
                    return BadRequest("Invalid client ID");
                }

                _logger.LogInformation("Getting client with ID {Id}", id);
                var client = _clientRepository.GetClientById(id);
                if (client == null)
                {
                    _logger.LogWarning("Client with ID {Id} not found", id);
                    return NotFound($"Client with ID {id} not found");
                }

                _logger.LogInformation("Found client: {Username}", client.Username);
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the client");
            }
        }

        [HttpPost]
        public IActionResult AddClient(Client client)
        {
            try
            {
                if (client == null)
                {
                    _logger.LogWarning("Client data is null");
                    return BadRequest("Client data is required");
                }

                if (string.IsNullOrWhiteSpace(client.Username))
                {
                    _logger.LogWarning("Username is required");
                    return BadRequest("Username is required");
                }

                if (string.IsNullOrWhiteSpace(client.Email))
                {
                    _logger.LogWarning("Email is required");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(client.PasswordHash))
                {
                    _logger.LogWarning("Password is required");
                    return BadRequest("Password is required");
                }

                _logger.LogInformation("Adding new client: {Username}", client.Username);
                var result = _clientRepository.AddClient(client);
                
                if (result != null)
                {
                    _logger.LogInformation("Successfully added client with ID: {Id}", result.Id);
                    return Ok(result);
                }
                else
                {
                    _logger.LogWarning("Failed to add client");
                    return BadRequest("Failed to add client");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while adding client");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding client");
                return StatusCode(500, "An error occurred while adding the client");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult UpdateClient(int id, Client client)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid client ID: {Id}", id);
                    return BadRequest("Invalid client ID");
                }

                if (client == null)
                {
                    _logger.LogWarning("Client data is null");
                    return BadRequest("Client data is required");
                }

                if (id != client.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {ClientId}", id, client.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(client.Username))
                {
                    _logger.LogWarning("Username is required");
                    return BadRequest("Username is required");
                }

                if (string.IsNullOrWhiteSpace(client.Email))
                {
                    _logger.LogWarning("Email is required");
                    return BadRequest("Email is required");
                }

                _logger.LogInformation("Updating client with ID {Id}", id);
                var result = _clientRepository.UpdateClient(client);
                if (result == null)
                {
                    _logger.LogWarning("Client with ID {Id} not found", id);
                    return NotFound($"Client with ID {id} not found");
                }

                _logger.LogInformation("Successfully updated client with ID {Id}", id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating client");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the client");
            }
        }
    }
}
