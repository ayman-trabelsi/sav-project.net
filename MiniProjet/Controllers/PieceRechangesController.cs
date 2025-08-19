using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Repository.IRepository;
using Shared.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PieceRechangesController : ControllerBase
    {
        private readonly IPieceRechangeRepository _repo;
        private readonly ILogger<PieceRechangesController> _logger;

        public PieceRechangesController(IPieceRechangeRepository repo, ILogger<PieceRechangesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all pieces rechange");
                var pieces = _repo.GetAll();
                _logger.LogInformation("Found {Count} pieces rechange", pieces.Count);
                return Ok(pieces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all pieces rechange");
                return StatusCode(500, "An error occurred while retrieving pieces rechange");
            }
        }

        [HttpGet("article/{articleId}")]
        public IActionResult GetByArticleId(int articleId)
        {
            try
            {
                if (articleId <= 0)
                {
                    _logger.LogWarning("Invalid article ID: {ArticleId}", articleId);
                    return BadRequest("Invalid article ID");
                }

                _logger.LogInformation("Getting pieces rechange for article ID {ArticleId}", articleId);
                var pieces = _repo.GetByArticleId(articleId);
                _logger.LogInformation("Found {Count} pieces rechange for article {ArticleId}", pieces.Count, articleId);
                return Ok(pieces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pieces rechange for article {ArticleId}", articleId);
                return StatusCode(500, "An error occurred while retrieving pieces rechange");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid piece rechange ID: {Id}", id);
                    return BadRequest("Invalid piece rechange ID");
                }

                _logger.LogInformation("Getting piece rechange with ID {Id}", id);
                var piece = _repo.GetById(id);
                if (piece == null)
                {
                    _logger.LogWarning("Piece rechange with ID {Id} not found", id);
                    return NotFound($"Piece rechange with ID {id} not found");
                }

                _logger.LogInformation("Found piece rechange: {Nom}", piece.Nom);
                return Ok(piece);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting piece rechange {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the piece rechange");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Create([FromBody] PieceRechange pieceRechange)
        {
            try
            {
                if (pieceRechange == null)
                {
                    _logger.LogWarning("Piece rechange is null");
                    return BadRequest("Piece rechange is null");
                }

                if (string.IsNullOrWhiteSpace(pieceRechange.Nom))
                {
                    _logger.LogWarning("Nom is required");
                    return BadRequest("Nom is required");
                }

                if (pieceRechange.Prix < 0)
                {
                    _logger.LogWarning("Price cannot be negative");
                    return BadRequest("Price cannot be negative");
                }

                if (pieceRechange.ArticleId <= 0)
                {
                    _logger.LogWarning("Article ID is required");
                    return BadRequest("Article ID is required");
                }

                _logger.LogInformation("Creating new piece rechange: {Nom}", pieceRechange.Nom);
                var result = _repo.AddPieceRechange(pieceRechange);
                if (result == null)
                {
                    _logger.LogError("Failed to create piece rechange");
                    return StatusCode(500, "Failed to create piece rechange");
                }

                _logger.LogInformation("Successfully created piece rechange with ID {Id}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating piece rechange");
                return StatusCode(500, "An error occurred while creating the piece rechange");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Update(int id, [FromBody] PieceRechange updated)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid piece rechange ID: {Id}", id);
                    return BadRequest("Invalid piece rechange ID");
                }

                if (updated == null)
                {
                    _logger.LogWarning("Updated piece rechange is null");
                    return BadRequest("Updated piece rechange is null");
                }

                if (id != updated.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {UpdatedId}", id, updated.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(updated.Nom))
                {
                    _logger.LogWarning("Nom is required");
                    return BadRequest("Nom is required");
                }

                if (updated.Prix < 0)
                {
                    _logger.LogWarning("Price cannot be negative");
                    return BadRequest("Price cannot be negative");
                }

                if (updated.ArticleId <= 0)
                {
                    _logger.LogWarning("Article ID is required");
                    return BadRequest("Article ID is required");
                }

                _logger.LogInformation("Updating piece rechange with ID {Id}", id);
                var success = _repo.Update(updated);
                if (!success)
                {
                    _logger.LogWarning("Piece rechange with ID {Id} not found for update", id);
                    return NotFound($"Piece rechange with ID {id} not found");
                }

                _logger.LogInformation("Successfully updated piece rechange with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating piece rechange {Id}", id);
                return StatusCode(500, "An error occurred while updating the piece rechange");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid piece rechange ID: {Id}", id);
                    return BadRequest("Invalid piece rechange ID");
                }

                _logger.LogInformation("Deleting piece rechange with ID {Id}", id);
                var success = _repo.Delete(id);
                if (!success)
                {
                    _logger.LogWarning("Piece rechange with ID {Id} not found for deletion", id);
                    return NotFound($"Piece rechange with ID {id} not found");
                }

                _logger.LogInformation("Successfully deleted piece rechange with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting piece rechange {Id}", id);
                return StatusCode(500, "An error occurred while deleting the piece rechange");
            }
        }
    }
} 