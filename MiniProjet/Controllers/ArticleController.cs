using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Controllers
{
    [Authorize] // Require authentication for all endpoints
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleRepository repository, ILogger<ArticlesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // ✅ Public access for authenticated users
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all articles");
                var articles = _repository.GetAll();
                _logger.LogInformation("Found {Count} articles", articles.Count);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all articles");
                return StatusCode(500, "An error occurred while retrieving articles");
            }
        }

        // ✅ Public access for authenticated users
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid article ID: {Id}", id);
                    return BadRequest("Invalid article ID");
                }

                _logger.LogInformation("Getting article with ID {Id}", id);
                var article = _repository.GetById(id);
                if (article == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Found article: {Libelle}", article.Libelle);
                return Ok(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting article with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the article");
            }
        }

        // 🔐 Requires ResponsableSAV role
        [HttpPost]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Create([FromBody] Article article)
        {
            try
            {
                if (article == null)
                {
                    _logger.LogWarning("Article is null");
                    return BadRequest("Article is null");
                }

                if (string.IsNullOrWhiteSpace(article.Libelle))
                {
                    _logger.LogWarning("Libelle is required");
                    return BadRequest("Libelle is required");
                }

                if (article.Prix < 0)
                {
                    _logger.LogWarning("Price cannot be negative");
                    return BadRequest("Price cannot be negative");
                }

                _logger.LogInformation("Creating new article: {Libelle}", article.Libelle);
                var result = _repository.Add(article);
                _logger.LogInformation("Successfully created article with ID {Id}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article");
                return StatusCode(500, "An error occurred while creating the article");
            }
        }

        // 🔐 Requires ResponsableSAV role
        [HttpPut("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Update(int id, [FromBody] Article article)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid article ID: {Id}", id);
                    return BadRequest("Invalid article ID");
                }

                if (article == null)
                {
                    _logger.LogWarning("Article is null");
                    return BadRequest("Article is null");
                }

                if (id != article.Id)
                {
                    _logger.LogWarning("ID mismatch: {Id} != {ArticleId}", id, article.Id);
                    return BadRequest("ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(article.Libelle))
                {
                    _logger.LogWarning("Libelle is required");
                    return BadRequest("Libelle is required");
                }

                if (article.Prix < 0)
                {
                    _logger.LogWarning("Price cannot be negative");
                    return BadRequest("Price cannot be negative");
                }

                _logger.LogInformation("Updating article with ID {Id}", id);
                var success = _repository.Update(article);
                if (!success)
                {
                    _logger.LogWarning("Article with ID {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully updated article with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the article");
            }
        }

        // 🔐 Requires ResponsableSAV role
        [HttpDelete("{id}")]
        [Authorize(Roles = "ResponsableSAV")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid article ID: {Id}", id);
                    return BadRequest("Invalid article ID");
                }

                _logger.LogInformation("Deleting article with ID {Id}", id);
                var success = _repository.Delete(id);
                if (!success)
                {
                    _logger.LogWarning("Article with ID {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully deleted article with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting article with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the article");
            }
        }
    }
}
