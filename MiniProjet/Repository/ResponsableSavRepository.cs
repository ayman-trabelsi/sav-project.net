using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class ResponsableSavRepository : IResponsableSAVRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ResponsableSavRepository> _logger;

        public ResponsableSavRepository(ApplicationDbContext context, ILogger<ResponsableSavRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<ResponsableSAV> GetResponsableSAVs()
        {
            try
            {
                _logger.LogInformation("Getting all ResponsableSAV users");
                var responsables = _context.ResponsableSAV.ToList();
                _logger.LogInformation("Found {Count} ResponsableSAV users", responsables.Count);
                return responsables;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ResponsableSAV users");
                throw;
            }
        }

        public ResponsableSAV GetResponsableSAVById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid ResponsableSAV ID", nameof(id));

                _logger.LogInformation("Getting ResponsableSAV with ID {Id}", id);
                var responsable = _context.ResponsableSAV.Find(id);
                
                if (responsable == null)
                {
                    _logger.LogWarning("ResponsableSAV with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found ResponsableSAV: {Username}", responsable.Username);
                }
                
                return responsable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ResponsableSAV with ID {Id}", id);
                throw;
            }
        }

        public ResponsableSAV AddResponsableSAV(ResponsableSAV responsableSAV)
        {
            try
            {
                if (responsableSAV == null)
                    throw new ArgumentNullException(nameof(responsableSAV));

                if (string.IsNullOrWhiteSpace(responsableSAV.Username))
                    throw new ArgumentException("Username is required", nameof(responsableSAV));

                if (string.IsNullOrWhiteSpace(responsableSAV.Email))
                    throw new ArgumentException("Email is required", nameof(responsableSAV));

                if (string.IsNullOrWhiteSpace(responsableSAV.PasswordHash))
                    throw new ArgumentException("Password is required", nameof(responsableSAV));

                // Check if username or email already exists
                var existingResponsable = _context.ResponsableSAV
                    .FirstOrDefault(r => r.Username == responsableSAV.Username || r.Email == responsableSAV.Email);

                if (existingResponsable != null)
                {
                    _logger.LogWarning("ResponsableSAV with username {Username} or email {Email} already exists", 
                        responsableSAV.Username, responsableSAV.Email);
                    throw new InvalidOperationException("Username or email already exists");
                }

                _logger.LogInformation("Adding new ResponsableSAV: {Username}", responsableSAV.Username);
                _context.ResponsableSAV.Add(responsableSAV);
                _context.SaveChanges();
                _logger.LogInformation("Successfully added ResponsableSAV with ID {Id}", responsableSAV.Id);
                return responsableSAV;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ResponsableSAV");
                throw;
            }
        }

        public ResponsableSAV UpdateResponsableSAV(ResponsableSAV responsableSAV)
        {
            try
            {
                if (responsableSAV == null)
                    throw new ArgumentNullException(nameof(responsableSAV));

                if (string.IsNullOrWhiteSpace(responsableSAV.Username))
                    throw new ArgumentException("Username is required", nameof(responsableSAV));

                if (string.IsNullOrWhiteSpace(responsableSAV.Email))
                    throw new ArgumentException("Email is required", nameof(responsableSAV));

                _logger.LogInformation("Updating ResponsableSAV with ID {Id}", responsableSAV.Id);
                var existing = _context.ResponsableSAV.Find(responsableSAV.Id);
                if (existing == null)
                {
                    _logger.LogWarning("ResponsableSAV with ID {Id} not found", responsableSAV.Id);
                    throw new KeyNotFoundException($"ResponsableSAV with ID {responsableSAV.Id} not found");
                }

                // Check if username or email already exists for other responsables
                var duplicateResponsable = _context.ResponsableSAV
                    .FirstOrDefault(r => (r.Username == responsableSAV.Username || r.Email == responsableSAV.Email) && r.Id != responsableSAV.Id);

                if (duplicateResponsable != null)
                {
                    _logger.LogWarning("Another ResponsableSAV with username {Username} or email {Email} already exists", 
                        responsableSAV.Username, responsableSAV.Email);
                    throw new InvalidOperationException("Username or email already exists");
                }

                existing.Username = responsableSAV.Username;
                existing.Email = responsableSAV.Email;
                if (!string.IsNullOrWhiteSpace(responsableSAV.PasswordHash))
                {
                    existing.PasswordHash = responsableSAV.PasswordHash;
                }

                _context.SaveChanges();
                _logger.LogInformation("Successfully updated ResponsableSAV with ID {Id}", responsableSAV.Id);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ResponsableSAV with ID {Id}", responsableSAV.Id);
                throw;
            }
        }
    }
}
