using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArticleRepository> _logger;

        public ArticleRepository(ApplicationDbContext context, ILogger<ArticleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Article> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all articles");
                var articles = _context.Articles
                    .AsNoTracking()
                    .ToList();
                    
                _logger.LogInformation("Found {Count} articles", articles.Count);
                foreach (var article in articles)
                {
                    _logger.LogInformation("Article {Id}: {Libelle} - ImageUrl: {ImageUrl}", 
                        article.Id, article.Libelle, article.ImageUrl);
                }
                
                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all articles");
                throw;
            }
        }

        public Article? GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid article ID", nameof(id));

                _logger.LogInformation("Getting article with ID {Id}", id);
                var article = _context.Articles.Find(id);
                
                if (article == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found article: {Libelle}, ImageUrl: {ImageUrl}", 
                        article.Libelle, article.ImageUrl);
                }
                
                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting article with ID {Id}", id);
                throw;
            }
        }

        public Article Add(Article article)
        {
            try
            {
                if (article == null)
                    throw new ArgumentNullException(nameof(article));

                if (string.IsNullOrWhiteSpace(article.Libelle))
                    throw new ArgumentException("Libelle is required", nameof(article));

                if (article.Prix < 0)
                    throw new ArgumentException("Price cannot be negative", nameof(article));

                _logger.LogInformation("Adding new article: {Libelle}", article.Libelle);
                _context.Articles.Add(article);
                _context.SaveChanges();
                _logger.LogInformation("Successfully added article with ID {Id}", article.Id);
                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding article");
                throw;
            }
        }

        public bool Update(Article article)
        {
            try
            {
                if (article == null)
                    throw new ArgumentNullException(nameof(article));

                if (string.IsNullOrWhiteSpace(article.Libelle))
                    throw new ArgumentException("Libelle is required", nameof(article));

                if (article.Prix < 0)
                    throw new ArgumentException("Price cannot be negative", nameof(article));

                _logger.LogInformation("Updating article with ID {Id}", article.Id);
                var existing = _context.Articles.Find(article.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found", article.Id);
                    return false;
                }

                _logger.LogInformation("Current ImageUrl: {CurrentImageUrl}, New ImageUrl: {NewImageUrl}", 
                    existing.ImageUrl, article.ImageUrl);

                existing.Libelle = article.Libelle;
                existing.EstSousGarantie = article.EstSousGarantie;
                existing.Prix = article.Prix;
                existing.ImageUrl = article.ImageUrl;

                _context.SaveChanges();
                _logger.LogInformation("Successfully updated article with ID {Id}, new ImageUrl: {ImageUrl}", 
                    article.Id, existing.ImageUrl);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article with ID {Id}", article.Id);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid article ID", nameof(id));

                _logger.LogInformation("Deleting article with ID {Id}", id);
                var article = _context.Articles
                    .Include(a => a.PiecesRechanges)
                    .FirstOrDefault(a => a.Id == id);

                if (article == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found", id);
                    return false;
                }

                // Delete related PiecesRechange records first
                if (article.PiecesRechanges.Any())
                {
                    _logger.LogInformation("Deleting {Count} related pieces rechange for article {Id}", 
                        article.PiecesRechanges.Count, id);
                    _context.PiecesRechange.RemoveRange(article.PiecesRechanges);
                }

                _context.Articles.Remove(article);
                _context.SaveChanges();
                _logger.LogInformation("Successfully deleted article with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting article with ID {Id}", id);
                throw;
            }
        }
    }
}