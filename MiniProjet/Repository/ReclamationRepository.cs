using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class ReclamationRepository : IReclamationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReclamationRepository> _logger;

        public ReclamationRepository(ApplicationDbContext context, ILogger<ReclamationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Reclamation> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all reclamations");
                
                // Get all reclamations with their related entities (no UserType filter)
                var query = _context.Reclamations
                    .AsNoTracking()
                    .Include(r => r.article)
                    .Include(r => r.Etat)
                    .Include(r => r.Client)
                    .OrderByDescending(r => r.DateReclamation);

                // Log the SQL query
                var sql = query.ToQueryString();
                _logger.LogInformation("SQL Query: {Sql}", sql);
                
                var reclamations = query.ToList();

                _logger.LogInformation("Found {Count} reclamations", reclamations.Count);
                
                // Log each reclamation for debugging
                foreach (var reclamation in reclamations)
                {
                    _logger.LogInformation("Reclamation: Id={Id}, Description={Description}, ClientId={ClientId}, Client={ClientUsername}, Article={ArticleLibelle}",
                        reclamation.Id, 
                        reclamation.Description, 
                        reclamation.ClientId,
                        reclamation.Client?.Username,
                        reclamation.article?.Libelle);
                }
                
                return reclamations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all reclamations: {Message}", ex.Message);
                throw;
            }
        }

        public List<Reclamation> GetReclamationsByClientId(int clientId)
        {
            try
            {
                if (clientId <= 0)
                    throw new ArgumentException("Invalid client ID", nameof(clientId));

                _logger.LogInformation("Getting reclamations for client ID: {ClientId}", clientId);
                
                var client = _context.Users.Find(clientId);
                if (client == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found", clientId);
                    throw new KeyNotFoundException($"Client with ID {clientId} not found");
                }

                // Simplified query to get reclamations for the specific client
                var reclamations = _context.Reclamations
                    .AsNoTracking()
                    .Include(r => r.article)
                    .Include(r => r.Etat)
                    .Where(r => r.ClientId == clientId)
                    .OrderByDescending(r => r.DateReclamation)
                    .ToList();
                
                _logger.LogInformation("Found {Count} reclamations for client {ClientId}", reclamations.Count, clientId);
                
                // Log each reclamation for debugging
                foreach (var reclamation in reclamations)
                {
                    _logger.LogInformation("Reclamation: Id={Id}, Description={Description}, ArticleId={ArticleId}, ClientId={ClientId}, EtatId={EtatId}",
                        reclamation.Id, reclamation.Description, reclamation.idArticleReclamation, reclamation.ClientId, reclamation.EtatId);
                }
                
                return reclamations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reclamations for client {ClientId}", clientId);
                throw;
            }
        }

        public Reclamation AddReclamation(Reclamation reclamation)
        {
            try
            {
                if (reclamation == null)
                    throw new ArgumentNullException(nameof(reclamation));

                if (string.IsNullOrWhiteSpace(reclamation.Description))
                    throw new ArgumentException("Description is required", nameof(reclamation));

                if (reclamation.idArticleReclamation <= 0)
                    throw new ArgumentException("Article ID must be greater than 0", nameof(reclamation));

                if (reclamation.ClientId <= 0)
                    throw new ArgumentException("Client ID must be greater than 0", nameof(reclamation));

                if (reclamation.EtatId <= 0)
                    throw new ArgumentException("Etat ID must be greater than 0", nameof(reclamation));

                _logger.LogInformation("Adding new reclamation for client {ClientId}", reclamation.ClientId);
                
                // Set the date if not provided
                if (reclamation.DateReclamation == default)
                {
                    reclamation.DateReclamation = DateTime.Now;
                }

                // Add the reclamation directly without creating a new object
                _context.Reclamations.Add(reclamation);
                _context.SaveChanges();

                // Log the saved reclamation details
                _logger.LogInformation("Saved reclamation: Id={Id}, Description={Description}, ArticleId={ArticleId}, ClientId={ClientId}, EtatId={EtatId}",
                    reclamation.Id, reclamation.Description, reclamation.idArticleReclamation, reclamation.ClientId, reclamation.EtatId);

                // Return the saved reclamation
                return reclamation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reclamation");
                throw;
            }
        }

        public Reclamation UpdateReclamation(Reclamation reclamation)
        {
            try
            {
                if (reclamation == null)
                    throw new ArgumentNullException(nameof(reclamation));

                if (string.IsNullOrWhiteSpace(reclamation.Description))
                    throw new ArgumentException("Description is required", nameof(reclamation));

                if (reclamation.idArticleReclamation <= 0)
                    throw new ArgumentException("Article ID must be greater than 0", nameof(reclamation));

                if (reclamation.ClientId <= 0)
                    throw new ArgumentException("Client ID must be greater than 0", nameof(reclamation));

                if (reclamation.EtatId <= 0)
                    throw new ArgumentException("Etat ID must be greater than 0", nameof(reclamation));

                _logger.LogInformation("Updating reclamation with ID {Id}", reclamation.Id);
                var existingReclamation = _context.Reclamations.Find(reclamation.Id);
                if (existingReclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {Id} not found", reclamation.Id);
                    throw new KeyNotFoundException($"Reclamation with ID {reclamation.Id} not found");
                }

                existingReclamation.Description = reclamation.Description;
                existingReclamation.DateReclamation = reclamation.DateReclamation;
                existingReclamation.idArticleReclamation = reclamation.idArticleReclamation;
                existingReclamation.EtatId = reclamation.EtatId;
                existingReclamation.ClientId = reclamation.ClientId;
                existingReclamation.InterventionId = reclamation.InterventionId;

                _context.SaveChanges();
                _logger.LogInformation("Successfully updated reclamation with ID {Id}", reclamation.Id);
                return existingReclamation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reclamation with ID {Id}", reclamation.Id);
                throw;
            }
        }

        public bool DeleteReclamation(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid reclamation ID", nameof(id));

                _logger.LogInformation("Deleting reclamation with ID {Id}", id);
                var reclamation = _context.Reclamations.Find(id);
                if (reclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {Id} not found", id);
                    throw new KeyNotFoundException($"Reclamation with ID {id} not found");
                }

                _context.Reclamations.Remove(reclamation);
                _context.SaveChanges();
                _logger.LogInformation("Successfully deleted reclamation with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reclamation with ID {Id}", id);
                throw;
            }
        }

        public Reclamation GetReclamation(int id)
        {
            try
            {
                _logger.LogInformation("Getting reclamation with ID: {Id}", id);
                var reclamation = _context.Reclamations
                    .AsNoTracking()
                    .Include(r => r.article)
                    .Include(r => r.Etat)
                    .Include(r => r.Client)
                    .FirstOrDefault(r => r.Id == id);

                if (reclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {Id} not found", id);
                    return null;
                }

                _logger.LogInformation("Found reclamation: Id={Id}, Description={Description}, ClientId={ClientId}, Article={ArticleLibelle}",
                    reclamation.Id, reclamation.Description, reclamation.ClientId, reclamation.article?.Libelle);

                return reclamation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reclamation with ID {Id}", id);
                throw;
            }
        }
    }
}
