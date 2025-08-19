using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ModelsDto;
using MiniProjet.Repository.IRepository;
using Shared.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ResponsableSAV")]
    public class TechniciensController : ControllerBase
    {
        private readonly ITechnicienRepository _repo;
        private readonly ILogger<TechniciensController> _logger;

        public TechniciensController(ITechnicienRepository repo, ILogger<TechniciensController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all techniciens");
                var techniciens = _repo.GetAll();
                _logger.LogInformation("Found {Count} techniciens", techniciens.Count);
                return Ok(techniciens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all techniciens");
                return StatusCode(500, "An error occurred while retrieving techniciens");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid technicien ID: {Id}", id);
                    return BadRequest("Invalid technicien ID");
                }

                _logger.LogInformation("Getting technicien with ID {Id}", id);
                var technicien = _repo.GetById(id);
                if (technicien == null)
                {
                    _logger.LogWarning("Technicien with ID {Id} not found", id);
                    return NotFound($"Technicien with ID {id} not found");
                }

                _logger.LogInformation("Found technicien: {Nom}", technicien.Nom);
                return Ok(technicien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting technicien {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the technicien");
            }
        }

        [HttpPost]
        public IActionResult AddTechnicien(Technicien technicien)
        {
            try
            {
                if (technicien == null)
                {
                    _logger.LogWarning("Technicien data is null");
                    return BadRequest("Technicien data is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Nom))
                {
                    _logger.LogWarning("Nom is required");
                    return BadRequest("Nom is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Email))
                {
                    _logger.LogWarning("Email is required");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Telephone))
                {
                    _logger.LogWarning("Telephone is required");
                    return BadRequest("Telephone is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Specialite))
                {
                    _logger.LogWarning("Specialite is required");
                    return BadRequest("Specialite is required");
                }

                _logger.LogInformation("Creating new technicien: {Nom}", technicien.Nom);
                var result = _repo.AddTechnicien(technicien);
                if (result == null)
                {
                    _logger.LogError("Failed to create technicien");
                    return BadRequest("Failed to create technicien");
                }

                _logger.LogInformation("Successfully created technicien with ID {Id}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating technicien");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating technicien");
                return StatusCode(500, "An error occurred while creating the technicien");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTechnicien(int id, Technicien technicien)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid technicien ID: {Id}", id);
                    return BadRequest("Invalid technicien ID");
                }

                if (technicien == null)
                {
                    _logger.LogWarning("Technicien data is null");
                    return BadRequest("Technicien data is required");
                }

                if (id != technicien.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {TechnicienId}", id, technicien.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(technicien.Nom))
                {
                    _logger.LogWarning("Nom is required");
                    return BadRequest("Nom is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Email))
                {
                    _logger.LogWarning("Email is required");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Telephone))
                {
                    _logger.LogWarning("Telephone is required");
                    return BadRequest("Telephone is required");
                }

                if (string.IsNullOrWhiteSpace(technicien.Specialite))
                {
                    _logger.LogWarning("Specialite is required");
                    return BadRequest("Specialite is required");
                }

                _logger.LogInformation("Updating technicien with ID {Id}", id);
                var result = _repo.Update(technicien);
                if (result == null)
                {
                    _logger.LogWarning("Technicien with ID {Id} not found", id);
                    return NotFound($"Technicien with ID {id} not found");
                }

                _logger.LogInformation("Successfully updated technicien with ID {Id}", id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating technicien");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating technicien with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the technicien");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTechnicien(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid technicien ID: {Id}", id);
                    return BadRequest("Invalid technicien ID");
                }

                _logger.LogInformation("Deleting technicien with ID {Id}", id);
                var success = _repo.Delete(id);
                if (!success)
                {
                    _logger.LogWarning("Technicien with ID {Id} not found", id);
                    return NotFound($"Technicien with ID {id} not found");
                }

                _logger.LogInformation("Successfully deleted technicien with ID {Id}", id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while deleting technicien");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting technicien with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the technicien");
            }
        }

        [HttpGet("{id}/interventions")]
        public IActionResult GetInterventions(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid technicien ID: {Id}", id);
                    return BadRequest("Invalid technicien ID");
                }

                _logger.LogInformation("Getting interventions for technicien with ID {Id}", id);
                var interventions = _repo.GetInterventions(id);
                _logger.LogInformation("Found {Count} interventions for technicien {Id}", interventions.Count, id);
                return Ok(interventions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting interventions for technicien {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the technicien's interventions");
            }
        }
    }
} 