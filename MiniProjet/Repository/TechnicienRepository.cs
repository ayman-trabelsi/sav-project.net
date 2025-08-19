using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class TechnicienRepository : ITechnicienRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TechnicienRepository> _logger;

        public TechnicienRepository(ApplicationDbContext context, ILogger<TechnicienRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Technicien AddTechnicien(Technicien technicien)
        {
            try
            {
                if (technicien == null)
                    throw new ArgumentNullException(nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Nom))
                    throw new ArgumentException("Nom is required", nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Email))
                    throw new ArgumentException("Email is required", nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Telephone))
                    throw new ArgumentException("Telephone is required", nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Specialite))
                    throw new ArgumentException("Specialite is required", nameof(technicien));

                // Check if email already exists
                var existingTechnicien = _context.Techniciens
                    .FirstOrDefault(t => t.Email == technicien.Email);

                if (existingTechnicien != null)
                {
                    _logger.LogWarning("Technicien with email {Email} already exists", technicien.Email);
                    throw new InvalidOperationException("Email already exists");
                }

                _logger.LogInformation("Adding new technicien: {Nom}", technicien.Nom);
                _context.Add(technicien);
                _context.SaveChanges();
                _logger.LogInformation("Successfully added technicien with ID {Id}", technicien.Id);
                return technicien;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding technicien");
                throw;
            }
        }

        public List<Technicien> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all techniciens");
                var techniciens = _context.Techniciens.ToList();
                _logger.LogInformation("Found {Count} techniciens", techniciens.Count);
                return techniciens;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all techniciens");
                throw;
            }
        }

        public Technicien? GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid technicien ID", nameof(id));

                _logger.LogInformation("Getting technicien with ID {Id}", id);
                var technicien = _context.Techniciens.Find(id);
                
                if (technicien == null)
                {
                    _logger.LogWarning("Technicien with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found technicien: {Nom}", technicien.Nom);
                }
                
                return technicien;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting technicien with ID {Id}", id);
                throw;
            }
        }

        public Technicien? Update(Technicien technicien)
        {
            try
            {
                if (technicien == null)
                    throw new ArgumentNullException(nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Nom))
                    throw new ArgumentException("Nom is required", nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Email))
                    throw new ArgumentException("Email is required", nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Telephone))
                    throw new ArgumentException("Telephone is required", nameof(technicien));

                if (string.IsNullOrWhiteSpace(technicien.Specialite))
                    throw new ArgumentException("Specialite is required", nameof(technicien));

                _logger.LogInformation("Updating technicien with ID {Id}", technicien.Id);
                var existing = _context.Techniciens.Find(technicien.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Technicien with ID {Id} not found", technicien.Id);
                    return null;
                }

                // Check if email already exists for other techniciens
                var duplicateTechnicien = _context.Techniciens
                    .FirstOrDefault(t => t.Email == technicien.Email && t.Id != technicien.Id);

                if (duplicateTechnicien != null)
                {
                    _logger.LogWarning("Another technicien with email {Email} already exists", technicien.Email);
                    throw new InvalidOperationException("Email already exists");
                }

                existing.Nom = technicien.Nom;
                existing.Email = technicien.Email;
                existing.Telephone = technicien.Telephone;
                existing.Specialite = technicien.Specialite;

                _context.SaveChanges();
                _logger.LogInformation("Successfully updated technicien with ID {Id}", technicien.Id);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating technicien with ID {Id}", technicien.Id);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid technicien ID", nameof(id));

                _logger.LogInformation("Deleting technicien with ID {Id}", id);
                var technicien = _context.Techniciens.Find(id);
                if (technicien == null)
                {
                    _logger.LogWarning("Technicien with ID {Id} not found", id);
                    return false;
                }

                // Check if technicien has any interventions
                var hasInterventions = _context.Interventions.Any(i => i.TechnicienId == id);
                if (hasInterventions)
                {
                    _logger.LogWarning("Cannot delete technicien with ID {Id} because they have interventions", id);
                    throw new InvalidOperationException("Cannot delete technicien that has interventions");
                }

                _context.Techniciens.Remove(technicien);
                _context.SaveChanges();
                _logger.LogInformation("Successfully deleted technicien with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting technicien with ID {Id}", id);
                throw;
            }
        }

        public List<Intervention> GetInterventions(int technicienId)
        {
            try
            {
                if (technicienId <= 0)
                    throw new ArgumentException("Invalid technicien ID", nameof(technicienId));

                _logger.LogInformation("Getting interventions for technicien with ID {Id}", technicienId);
                var interventions = _context.Interventions
                    .Include(i => i.Reclamation)
                    .Where(i => i.TechnicienId == technicienId)
                    .ToList();
                _logger.LogInformation("Found {Count} interventions for technicien {Id}", interventions.Count, technicienId);
                return interventions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting interventions for technicien {Id}", technicienId);
                throw;
            }
        }
    }
}
