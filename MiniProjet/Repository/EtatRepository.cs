using Microsoft.EntityFrameworkCore;
using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class EtatRepository : IEtatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EtatRepository> _logger;

        public EtatRepository(ApplicationDbContext context, ILogger<EtatRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Etat> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all etats");
                var etats = _context.Etats.ToList();
                _logger.LogInformation("Found {Count} etats", etats.Count);
                return etats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all etats");
                throw;
            }
        }

        public Etat? GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid etat ID", nameof(id));

                _logger.LogInformation("Getting etat with ID {Id}", id);
                var etat = _context.Etats.Find(id);
                
                if (etat == null)
                {
                    _logger.LogWarning("Etat with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found etat: {Libelle}", etat.Libelle);
                }
                
                return etat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting etat with ID {Id}", id);
                throw;
            }
        }

        public Etat Add(Etat etat)
        {
            try
            {
                if (etat == null)
                    throw new ArgumentNullException(nameof(etat));

                if (string.IsNullOrWhiteSpace(etat.Libelle))
                    throw new ArgumentException("Libelle is required", nameof(etat));

                _logger.LogInformation("Adding new etat: {Libelle}", etat.Libelle);
                _context.Etats.Add(etat);
                _context.SaveChanges();
                _logger.LogInformation("Successfully added etat with ID {Id}", etat.Id);
                return etat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding etat");
                throw;
            }
        }

        public bool Update(Etat etat)
        {
            try
            {
                if (etat == null)
                    throw new ArgumentNullException(nameof(etat));

                if (string.IsNullOrWhiteSpace(etat.Libelle))
                    throw new ArgumentException("Libelle is required", nameof(etat));

                _logger.LogInformation("Updating etat with ID {Id}", etat.Id);
                var existing = _context.Etats.Find(etat.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Etat with ID {Id} not found", etat.Id);
                    return false;
                }

                existing.Libelle = etat.Libelle;

                _context.SaveChanges();
                _logger.LogInformation("Successfully updated etat with ID {Id}", etat.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating etat with ID {Id}", etat.Id);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid etat ID", nameof(id));

                _logger.LogInformation("Deleting etat with ID {Id}", id);
                var etat = _context.Etats.Find(id);
                if (etat == null)
                {
                    _logger.LogWarning("Etat with ID {Id} not found", id);
                    return false;
                }

                // Check if etat is being used by any reclamations
                var hasReclamations = _context.Reclamations.Any(r => r.EtatId == id);
                if (hasReclamations)
                {
                    _logger.LogWarning("Cannot delete etat with ID {Id} because it is being used by reclamations", id);
                    throw new InvalidOperationException("Cannot delete etat that is being used by reclamations");
                }

                _context.Etats.Remove(etat);
                _context.SaveChanges();
                _logger.LogInformation("Successfully deleted etat with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting etat with ID {Id}", id);
                throw;
            }
        }
    }
} 