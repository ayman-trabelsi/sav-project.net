using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.ModelsDto;
using MiniProjet.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ResponsableSAV")]
    public class InterventionsController : ControllerBase
    {
        private readonly IInterventionRepository _repo;
        private readonly ILogger<InterventionsController> _logger;

        public InterventionsController(IInterventionRepository repo, ILogger<InterventionsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all interventions");
                var interventions = _repo.GetAll();
                _logger.LogInformation("Found {Count} interventions", interventions.Count);
                return Ok(interventions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all interventions");
                return StatusCode(500, "An error occurred while retrieving interventions");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid intervention ID: {Id}", id);
                    return BadRequest("Invalid intervention ID");
                }

                _logger.LogInformation("Getting intervention with ID {Id}", id);
                var intervention = _repo.GetById(id);
                if (intervention == null)
                {
                    _logger.LogWarning("Intervention with ID {Id} not found", id);
                    return NotFound($"Intervention with ID {id} not found");
                }

                _logger.LogInformation("Found intervention for reclamation {ReclamationId}", intervention.ReclamationId);
                return Ok(intervention);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting intervention {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the intervention");
            }
        }

        [HttpPost]
        public IActionResult Add(InterventionRequestDto item)
        {
            try
            {
                if (item == null)
                {
                    _logger.LogWarning("Intervention request is null");
                    return BadRequest("Intervention request is null");
                }

                if (item.intervention == null)
                {
                    _logger.LogWarning("Intervention data is null");
                    return BadRequest("Intervention data is required");
                }

                if (string.IsNullOrWhiteSpace(item.intervention.Description))
                {
                    _logger.LogWarning("Description is required");
                    return BadRequest("Description is required");
                }

                if (item.intervention.TechnicienId <= 0)
                {
                    _logger.LogWarning("Technicien ID is required");
                    return BadRequest("Technicien ID is required");
                }

                if (item.intervention.ReclamationId <= 0)
                {
                    _logger.LogWarning("Reclamation ID is required");
                    return BadRequest("Reclamation ID is required");
                }

                _logger.LogInformation("Creating/Updating intervention for reclamation {ReclamationId}", item.intervention.ReclamationId);
                var result = _repo.Add(item);
                if (result == null)
                {
                    _logger.LogError("Failed to create/update intervention");
                    return BadRequest("Failed to create/update intervention");
                }

                _logger.LogInformation("Successfully created/updated intervention with ID {Id}", result.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating/updating intervention");
                return StatusCode(500, "An error occurred while creating/updating the intervention");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Intervention intervention)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid intervention ID: {Id}", id);
                    return BadRequest("Invalid intervention ID");
                }

                if (intervention == null)
                {
                    _logger.LogWarning("Intervention data is null");
                    return BadRequest("Intervention data is required");
                }

                if (id != intervention.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {InterventionId}", id, intervention.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(intervention.Description))
                {
                    _logger.LogWarning("Description is required");
                    return BadRequest("Description is required");
                }

                if (intervention.TechnicienId <= 0)
                {
                    _logger.LogWarning("Technicien ID is required");
                    return BadRequest("Technicien ID is required");
                }

                if (intervention.ReclamationId <= 0)
                {
                    _logger.LogWarning("Reclamation ID is required");
                    return BadRequest("Reclamation ID is required");
                }

                _logger.LogInformation("Updating intervention with ID {Id}", id);
                var result = _repo.Update(intervention);
                if (result == null)
                {
                    _logger.LogWarning("Intervention with ID {Id} not found", id);
                    return NotFound($"Intervention with ID {id} not found");
                }

                _logger.LogInformation("Successfully updated intervention with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating intervention {Id}", id);
                return StatusCode(500, "An error occurred while updating the intervention");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid intervention ID: {Id}", id);
                    return BadRequest("Invalid intervention ID");
                }

                _logger.LogInformation("Deleting intervention with ID {Id}", id);
                var result = _repo.Delete(id);
                if (!result)
                {
                    _logger.LogWarning("Intervention with ID {Id} not found", id);
                    return NotFound($"Intervention with ID {id} not found");
                }

                _logger.LogInformation("Successfully deleted intervention with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting intervention {Id}", id);
                return StatusCode(500, "An error occurred while deleting the intervention");
            }
        }
    }
}
