using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EtatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EtatsController> _logger;

        public EtatsController(ApplicationDbContext context, ILogger<EtatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/etats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Etat>>> GetEtats()
        {
            try
            {
                _logger.LogInformation("Getting all etats");
                var etats = await _context.Etats.ToListAsync();
                _logger.LogInformation("Found {Count} etats", etats.Count);
                return Ok(etats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all etats");
                return StatusCode(500, "An error occurred while retrieving etats");
            }
        }

        // GET: api/etats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Etat>> GetEtat(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid etat ID: {Id}", id);
                    return BadRequest("Invalid etat ID");
                }

                _logger.LogInformation("Getting etat with ID {Id}", id);
                var etat = await _context.Etats.FindAsync(id);

                if (etat == null)
                {
                    _logger.LogWarning("Etat with ID {Id} not found", id);
                    return NotFound($"Etat with ID {id} not found");
                }

                _logger.LogInformation("Found etat: {Libelle}", etat.Libelle);
                return Ok(etat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting etat with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the etat");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ResponsableSAV")]
        public async Task<ActionResult<Etat>> CreateEtat(Etat etat)
        {
            try
            {
                if (etat == null)
                {
                    _logger.LogWarning("Etat is null");
                    return BadRequest("Etat is null");
                }

                if (string.IsNullOrWhiteSpace(etat.Libelle))
                {
                    _logger.LogWarning("Libelle is required");
                    return BadRequest("Libelle is required");
                }

                _logger.LogInformation("Creating new etat: {Libelle}", etat.Libelle);
                _context.Etats.Add(etat);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created etat with ID {Id}", etat.Id);
                return CreatedAtAction(nameof(GetEtat), new { id = etat.Id }, etat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating etat");
                return StatusCode(500, "An error occurred while creating the etat");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public async Task<IActionResult> UpdateEtat(int id, Etat etat)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid etat ID: {Id}", id);
                    return BadRequest("Invalid etat ID");
                }

                if (etat == null)
                {
                    _logger.LogWarning("Etat is null");
                    return BadRequest("Etat is null");
                }

                if (id != etat.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {EtatId}", id, etat.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(etat.Libelle))
                {
                    _logger.LogWarning("Libelle is required");
                    return BadRequest("Libelle is required");
                }

                _logger.LogInformation("Updating etat with ID {Id}", id);
                var existingEtat = await _context.Etats.FindAsync(id);
                if (existingEtat == null)
                {
                    _logger.LogWarning("Etat with ID {Id} not found", id);
                    return NotFound($"Etat with ID {id} not found");
                }

                existingEtat.Libelle = etat.Libelle;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated etat with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating etat with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the etat");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public async Task<IActionResult> DeleteEtat(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid etat ID: {Id}", id);
                    return BadRequest("Invalid etat ID");
                }

                _logger.LogInformation("Deleting etat with ID {Id}", id);
                var etat = await _context.Etats.FindAsync(id);
                if (etat == null)
                {
                    _logger.LogWarning("Etat with ID {Id} not found", id);
                    return NotFound($"Etat with ID {id} not found");
                }

                // Check if etat is being used by any reclamations
                var hasReclamations = await _context.Reclamations.AnyAsync(r => r.EtatId == id);
                if (hasReclamations)
                {
                    _logger.LogWarning("Cannot delete etat with ID {Id} because it is being used by reclamations", id);
                    return BadRequest("Cannot delete etat that is being used by reclamations");
                }

                _context.Etats.Remove(etat);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted etat with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting etat with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the etat");
            }
        }
    }
}