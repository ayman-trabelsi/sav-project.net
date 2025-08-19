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
    [Authorize(Roles = "ResponsableSAV")]
    public class ResponsableSavController : ControllerBase
    {
        private readonly IResponsableSAVRepository _ResponsableSAVRepository;
        private readonly ILogger<ResponsableSavController> _logger;

        public ResponsableSavController(IResponsableSAVRepository ResponsableSAVRepository, ILogger<ResponsableSavController> logger)
        {
            _ResponsableSAVRepository = ResponsableSAVRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Getting all ResponsableSAV users");
                var responsables = _ResponsableSAVRepository.GetResponsableSAVs();
                _logger.LogInformation("Found {Count} ResponsableSAV users", responsables.Count);
                return Ok(responsables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ResponsableSAV users");
                return StatusCode(500, "An error occurred while retrieving ResponsableSAV users");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid ResponsableSAV ID: {Id}", id);
                    return BadRequest("Invalid ResponsableSAV ID");
                }

                _logger.LogInformation("Getting ResponsableSAV with ID {Id}", id);
                var responsable = _ResponsableSAVRepository.GetResponsableSAVById(id);
                if (responsable == null)
                {
                    _logger.LogWarning("ResponsableSAV with ID {Id} not found", id);
                    return NotFound($"ResponsableSAV with ID {id} not found");
                }

                _logger.LogInformation("Found ResponsableSAV: {Username}", responsable.Username);
                return Ok(responsable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ResponsableSAV with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the ResponsableSAV");
            }
        }

        [HttpPost]
        public IActionResult AddResponsableSAV(ResponsableSAV ResponsableSAV)
        {
            try
            {
                if (ResponsableSAV == null)
                {
                    _logger.LogWarning("ResponsableSAV data is null");
                    return BadRequest("ResponsableSAV data is required");
                }

                if (string.IsNullOrWhiteSpace(ResponsableSAV.Username))
                {
                    _logger.LogWarning("Username is required");
                    return BadRequest("Username is required");
                }

                if (string.IsNullOrWhiteSpace(ResponsableSAV.Email))
                {
                    _logger.LogWarning("Email is required");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(ResponsableSAV.PasswordHash))
                {
                    _logger.LogWarning("Password is required");
                    return BadRequest("Password is required");
                }

                _logger.LogInformation("Adding new ResponsableSAV: {Username}", ResponsableSAV.Username);
                var result = _ResponsableSAVRepository.AddResponsableSAV(ResponsableSAV);
                
                if (result != null)
                {
                    _logger.LogInformation("Successfully added ResponsableSAV with ID: {Id}", result.Id);
                    return Ok(result);
                }
                else
                {
                    _logger.LogWarning("Failed to add ResponsableSAV");
                    return BadRequest("Failed to add ResponsableSAV");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while adding ResponsableSAV");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ResponsableSAV");
                return StatusCode(500, "An error occurred while adding the ResponsableSAV");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateResponsableSAV(int id, ResponsableSAV ResponsableSAV)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid ResponsableSAV ID: {Id}", id);
                    return BadRequest("Invalid ResponsableSAV ID");
                }

                if (ResponsableSAV == null)
                {
                    _logger.LogWarning("ResponsableSAV data is null");
                    return BadRequest("ResponsableSAV data is required");
                }

                if (id != ResponsableSAV.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {ResponsableSAVId}", id, ResponsableSAV.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(ResponsableSAV.Username))
                {
                    _logger.LogWarning("Username is required");
                    return BadRequest("Username is required");
                }

                if (string.IsNullOrWhiteSpace(ResponsableSAV.Email))
                {
                    _logger.LogWarning("Email is required");
                    return BadRequest("Email is required");
                }

                _logger.LogInformation("Updating ResponsableSAV with ID {Id}", id);
                var result = _ResponsableSAVRepository.UpdateResponsableSAV(ResponsableSAV);
                if (result == null)
                {
                    _logger.LogWarning("ResponsableSAV with ID {Id} not found", id);
                    return NotFound($"ResponsableSAV with ID {id} not found");
                }

                _logger.LogInformation("Successfully updated ResponsableSAV with ID {Id}", id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating ResponsableSAV");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ResponsableSAV with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the ResponsableSAV");
            }
        }
    }
}
