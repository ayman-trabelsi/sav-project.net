using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Repository.IRepository;
using Shared.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReclamationsController : ControllerBase
    {
        private readonly IReclamationRepository _reclamationRepository;
        private readonly ILogger<ReclamationsController> _logger;

        public ReclamationsController(IReclamationRepository reclamationRepository, ILogger<ReclamationsController> logger)
        {
            _reclamationRepository = reclamationRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Client,ResponsableSAV")]
        public ActionResult<IEnumerable<Reclamation>> GetReclamations()
        {
            try
            {
                _logger.LogInformation("Getting reclamations");
                _logger.LogDebug("Headers: {Headers}", Request.Headers);

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                _logger.LogInformation("User role: {Role}, Client ID: {ClientId}", userRole, clientId);

                if (userRole == "ResponsableSAV")
                {
                    _logger.LogInformation("User is ResponsableSAV, returning all reclamations");
                    var allReclamations = _reclamationRepository.GetAll();
                    _logger.LogInformation("Found {Count} reclamations", allReclamations.Count);
                    return Ok(allReclamations);
                }
                else if (userRole == "Client")
                {
                    _logger.LogInformation("User is Client with ID {ClientId}, returning their reclamations", clientId);
                    var reclamations = _reclamationRepository.GetReclamationsByClientId(clientId);
                    _logger.LogInformation("Found {Count} reclamations for client {ClientId}", reclamations.Count, clientId);
                    return Ok(reclamations);
                }
                else
                {
                    _logger.LogWarning("Unauthorized access attempt by user with role {Role}", userRole);
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reclamations");
                return StatusCode(500, "An error occurred while retrieving reclamations");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Client,ResponsableSAV")]
        public ActionResult<Reclamation> GetReclamation(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid reclamation ID: {Id}", id);
                    return BadRequest("Invalid reclamation ID");
                }

                _logger.LogInformation("Getting reclamation with ID {Id}", id);
                var reclamation = _reclamationRepository.GetReclamation(id);
                if (reclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {Id} not found", id);
                    return NotFound();
                }

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userRole != "ResponsableSAV" && reclamation.ClientId != clientId)
                {
                    _logger.LogWarning("Access denied: Client {ClientId} tried to access reclamation {ReclamationId}", clientId, id);
                    return Forbid();
                }

                _logger.LogInformation("Successfully retrieved reclamation {Id}", id);
                return Ok(reclamation);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Client not found for reclamation {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reclamation {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the reclamation");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public ActionResult<Reclamation> CreateReclamation(Reclamation reclamation)
        {
            try
            {
                _logger.LogInformation("Creating new reclamation");
                _logger.LogDebug("Request body: {@Reclamation}", reclamation);

                if (string.IsNullOrWhiteSpace(reclamation.Description))
                {
                    _logger.LogWarning("Description is required");
                    return BadRequest("Description is required");
                }

                if (reclamation.idArticleReclamation <= 0)
                {
                    _logger.LogWarning("Article ID is required");
                    return BadRequest("Article ID is required");
                }

                var clientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogDebug("Client ID claim from token: {ClientId}", clientIdClaim);

                if (string.IsNullOrEmpty(clientIdClaim))
                {
                    _logger.LogWarning("Client ID not found in token");
                    return BadRequest("Client ID not found in token");
                }

                reclamation.ClientId = int.Parse(clientIdClaim);
                reclamation.DateReclamation = DateTime.Now;
                reclamation.EtatId = 1; // "En attente"

                _logger.LogInformation("Creating reclamation for client ID: {ClientId}", reclamation.ClientId);
                _logger.LogDebug("Reclamation details: Description={Description}, ArticleId={ArticleId}, ClientId={ClientId}, EtatId={EtatId}",
                    reclamation.Description, reclamation.idArticleReclamation, reclamation.ClientId, reclamation.EtatId);

                var createdReclamation = _reclamationRepository.AddReclamation(reclamation);
                if (createdReclamation == null)
                {
                    _logger.LogError("Failed to create reclamation");
                    return BadRequest("Failed to create reclamation");
                }

                _logger.LogInformation("Successfully created reclamation with ID: {Id}", createdReclamation.Id);
                return CreatedAtAction(nameof(GetReclamations), new { id = createdReclamation.Id }, createdReclamation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reclamation");
                return StatusCode(500, "An error occurred while creating the reclamation");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult UpdateReclamation(int id, Reclamation reclamation)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid reclamation ID: {Id}", id);
                    return BadRequest("Invalid reclamation ID");
                }

                if (id != reclamation.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {ReclamationId}", id, reclamation.Id);
                    return BadRequest("ID mismatch");
                }

                _logger.LogInformation("Updating reclamation {Id}", id);
                var updatedReclamation = _reclamationRepository.UpdateReclamation(reclamation);
                if (updatedReclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully updated reclamation {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reclamation {Id}", id);
                return StatusCode(500, "An error occurred while updating the reclamation");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult DeleteReclamation(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid reclamation ID: {Id}", id);
                    return BadRequest("Invalid reclamation ID");
                }

                _logger.LogInformation("Deleting reclamation {Id}", id);
                var result = _reclamationRepository.DeleteReclamation(id);
                if (!result)
                {
                    _logger.LogWarning("Reclamation with ID {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully deleted reclamation {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reclamation {Id}", id);
                return StatusCode(500, "An error occurred while deleting the reclamation");
            }
        }
    }
} 