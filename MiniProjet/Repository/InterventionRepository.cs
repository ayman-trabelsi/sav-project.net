using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Shared.ModelsDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class InterventionRepository : IInterventionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InterventionRepository> _logger;

        public InterventionRepository(ApplicationDbContext context, ILogger<InterventionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Intervention Add(InterventionRequestDto intervention)
        {
            try
            {
                if (intervention == null)
                    throw new ArgumentNullException(nameof(intervention));

                if (intervention.intervention == null)
                    throw new ArgumentException("Intervention data is required", nameof(intervention));

                if (string.IsNullOrWhiteSpace(intervention.intervention.Description))
                    throw new ArgumentException("Description is required", nameof(intervention));

                if (intervention.intervention.TechnicienId <= 0)
                    throw new ArgumentException("TechnicienId must be greater than 0", nameof(intervention));

                if (intervention.intervention.ReclamationId <= 0)
                    throw new ArgumentException("ReclamationId must be greater than 0", nameof(intervention));

                _logger.LogInformation("Adding new intervention for reclamation {ReclamationId}", intervention.intervention.ReclamationId);

                // Check if reclamation exists and get its article
                var reclamation = _context.Reclamations
                    .Include(r => r.article)
                    .FirstOrDefault(e => e.Id == intervention.intervention.ReclamationId);

                if (reclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {ReclamationId} not found", intervention.intervention.ReclamationId);
                    throw new KeyNotFoundException($"Reclamation with ID {intervention.intervention.ReclamationId} not found");
                }

                var article = reclamation.article;
                if (article == null)
                {
                    _logger.LogWarning("Article not found for reclamation {ReclamationId}", reclamation.Id);
                    throw new KeyNotFoundException($"Article not found for reclamation {reclamation.Id}");
                }

                // Check if technicien exists
                var technicien = _context.Techniciens.Find(intervention.intervention.TechnicienId);
                if (technicien == null)
                {
                    _logger.LogWarning("Technicien with ID {TechnicienId} not found", intervention.intervention.TechnicienId);
                    throw new KeyNotFoundException($"Technicien with ID {intervention.intervention.TechnicienId} not found");
                }

                // Check if intervention already exists for this reclamation
                var existingIntervention = _context.Interventions
                    .FirstOrDefault(i => i.ReclamationId == intervention.intervention.ReclamationId);

                if (existingIntervention != null)
                {
                    _logger.LogInformation("Updating existing intervention {Id} for reclamation {ReclamationId}", 
                        existingIntervention.Id, intervention.intervention.ReclamationId);

                    // Calculate montant facture based on warranty
                    if (article.EstSousGarantie)
                    {
                        existingIntervention.MontantFacture = 0;
                        existingIntervention.MainDOeuvre = 0;
                        _logger.LogInformation("Article is under warranty, setting montant facture to 0");
                    }
                    else
                    {
                        // Calculate piece prices from selected pieceRechangeIds
                        decimal piecePrices = 0;
                        if (intervention.pieceRechangeIds != null && intervention.pieceRechangeIds.Any())
                        {
                            var pieces = _context.PiecesRechange
                                .Where(p => intervention.pieceRechangeIds.Contains(p.Id))
                                .ToList();
                            piecePrices = pieces.Sum(p => p.Prix);
                            existingIntervention.Prix = piecePrices;
                        }
                        else
                        {
                            existingIntervention.Prix = 0;
                        }
                        // Set labor cost (fixed)
                        existingIntervention.MainDOeuvre = 50;
                        existingIntervention.MontantFacture = existingIntervention.Prix + existingIntervention.MainDOeuvre;
                        _logger.LogInformation("Article is not under warranty, montant facture set to {Montant}", 
                            existingIntervention.MontantFacture);
                    }

                    // Update intervention details
                    existingIntervention.DateIntervention = intervention.intervention.DateIntervention;
                    existingIntervention.Description = intervention.intervention.Description;
                    existingIntervention.TechnicienId = intervention.intervention.TechnicienId;
                    existingIntervention.Prix = existingIntervention.Prix;

                    _context.Interventions.Update(existingIntervention);
                    _context.SaveChanges();

                    return existingIntervention;
                }
                else
                {
                    _logger.LogInformation("Creating new intervention for reclamation {ReclamationId}", 
                        intervention.intervention.ReclamationId);

                    // Calculate montant facture based on warranty
                    if (article.EstSousGarantie)
                    {
                        intervention.intervention.MontantFacture = 0;
                        intervention.intervention.MainDOeuvre = 0;
                        _logger.LogInformation("Article is under warranty, setting montant facture to 0");
                    }
                    else
                    {
                        // Calculate piece prices from selected pieceRechangeIds
                        decimal piecePrices = 0;
                        if (intervention.pieceRechangeIds != null && intervention.pieceRechangeIds.Any())
                        {
                            var pieces = _context.PiecesRechange
                                .Where(p => intervention.pieceRechangeIds.Contains(p.Id))
                                .ToList();
                            piecePrices = pieces.Sum(p => p.Prix);
                            intervention.intervention.Prix = piecePrices;
                        }
                        else
                        {
                            intervention.intervention.Prix = 0;
                        }
                        // Set labor cost (fixed)
                        intervention.intervention.MainDOeuvre = 50;
                        intervention.intervention.MontantFacture = intervention.intervention.Prix + intervention.intervention.MainDOeuvre;
                        _logger.LogInformation("Article is not under warranty, montant facture set to {Montant}", 
                            intervention.intervention.MontantFacture);
                    }

                    // Add the intervention
                    _context.Interventions.Add(intervention.intervention);
                    _context.SaveChanges();

                    // Update reclamation status
                    reclamation.EtatId = 3; // En cours (was 2 for Traité)
                    reclamation.InterventionId = intervention.intervention.Id;
                    _context.Update(reclamation);
                    _logger.LogInformation("Updated reclamation {ReclamationId} status to En cours", reclamation.Id);

                    return intervention.intervention;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding intervention");
                throw;
            }
        }

        public List<Intervention> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all interventions");
                var interventions = _context.Interventions
                    .Include(i => i.Technicien)
                    .Include(i => i.Reclamation)
                        .ThenInclude(r => r.article)
                    .Include(i => i.Reclamation)
                        .ThenInclude(r => r.Client)
                    .Select(i => new Intervention
                    {
                        Id = i.Id,
                        DateIntervention = i.DateIntervention,
                        Description = i.Description,
                        Prix = i.Prix,
                        MontantFacture = i.MontantFacture,
                        TechnicienId = i.TechnicienId,
                        ReclamationId = i.ReclamationId,
                        Technicien = new Technicien
                        {
                            Id = i.Technicien.Id,
                            Nom = i.Technicien.Nom,
                            Email = i.Technicien.Email,
                            Telephone = i.Technicien.Telephone,
                            Specialite = i.Technicien.Specialite
                        },
                        Reclamation = new Reclamation
                        {
                            Id = i.Reclamation.Id,
                            Description = i.Reclamation.Description,
                            DateReclamation = i.Reclamation.DateReclamation,
                            EtatId = i.Reclamation.EtatId,
                            ClientId = i.Reclamation.ClientId,
                            idArticleReclamation = i.Reclamation.idArticleReclamation,
                            article = i.Reclamation.article,
                            Client = i.Reclamation.Client
                        }
                    })
                    .ToList();
                _logger.LogInformation("Found {Count} interventions", interventions.Count);
                return interventions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all interventions");
                throw;
            }
        }

        public Intervention? GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid intervention ID", nameof(id));

                _logger.LogInformation("Getting intervention with ID {Id}", id);
                var intervention = _context.Interventions
                    .Include(i => i.Technicien)
                    .Include(i => i.Reclamation)
                        .ThenInclude(r => r.article)
                    .Include(i => i.Reclamation)
                        .ThenInclude(r => r.Client)
                    .FirstOrDefault(i => i.Id == id);

                if (intervention == null)
                {
                    _logger.LogWarning("Intervention with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found intervention for reclamation {ReclamationId}", intervention.ReclamationId);
                }

                return intervention;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting intervention with ID {Id}", id);
                throw;
            }
        }

        public Intervention Update(Intervention intervention)
        {
            try
            {
                if (intervention == null)
                    throw new ArgumentNullException(nameof(intervention));

                if (string.IsNullOrWhiteSpace(intervention.Description))
                    throw new ArgumentException("Description is required", nameof(intervention));

                if (intervention.TechnicienId <= 0)
                    throw new ArgumentException("TechnicienId must be greater than 0", nameof(intervention));

                if (intervention.ReclamationId <= 0)
                    throw new ArgumentException("ReclamationId must be greater than 0", nameof(intervention));

                _logger.LogInformation("Updating intervention with ID {Id}", intervention.Id);

                // Check if intervention exists
                var existing = _context.Interventions
                    .Include(i => i.Technicien)
                    .Include(i => i.Reclamation)
                    .FirstOrDefault(i => i.Id == intervention.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Intervention with ID {Id} not found", intervention.Id);
                    throw new KeyNotFoundException($"Intervention with ID {intervention.Id} not found");
                }

                // Check if technicien exists
                var technicien = _context.Techniciens.Find(intervention.TechnicienId);
                if (technicien == null)
                {
                    _logger.LogWarning("Technicien with ID {TechnicienId} not found", intervention.TechnicienId);
                    throw new KeyNotFoundException($"Technicien with ID {intervention.TechnicienId} not found");
                }

                // Check if reclamation exists
                var reclamation = _context.Reclamations.Find(intervention.ReclamationId);
                if (reclamation == null)
                {
                    _logger.LogWarning("Reclamation with ID {ReclamationId} not found", intervention.ReclamationId);
                    throw new KeyNotFoundException($"Reclamation with ID {intervention.ReclamationId} not found");
                }

                existing.DateIntervention = intervention.DateIntervention;
                existing.Description = intervention.Description;
                existing.MontantFacture = intervention.MontantFacture;
                existing.TechnicienId = intervention.TechnicienId;
                existing.ReclamationId = intervention.ReclamationId;

                _context.Interventions.Update(existing);
                _context.SaveChanges();
                _logger.LogInformation("Successfully updated intervention with ID {Id}", intervention.Id);

                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating intervention with ID {Id}", intervention.Id);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid intervention ID", nameof(id));

                _logger.LogInformation("Deleting intervention with ID {Id}", id);
                var intervention = _context.Interventions.Find(id);
                if (intervention == null)
                {
                    _logger.LogWarning("Intervention with ID {Id} not found", id);
                    return false;
                }

                // Update associated reclamation
                var reclamation = _context.Reclamations.FirstOrDefault(r => r.InterventionId == id);
                if (reclamation != null)
                {
                    reclamation.EtatId = 1; // En attente
                    reclamation.InterventionId = null;
                    _context.Update(reclamation);
                    _logger.LogInformation("Updated reclamation {ReclamationId} status to En attente", reclamation.Id);
                }

                _context.Interventions.Remove(intervention);
                _context.SaveChanges();
                _logger.LogInformation("Successfully deleted intervention with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting intervention with ID {Id}", id);
                throw;
            }
        }
    }
}
