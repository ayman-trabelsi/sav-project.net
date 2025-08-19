using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(ApplicationDbContext context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Client> GetClients()
        {
            try
            {
                _logger.LogInformation("Getting all clients");
                var clients = _context.Clients.ToList();
                _logger.LogInformation("Found {Count} clients", clients.Count);
                return clients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all clients");
                throw;
            }
        }

        public Client GetClientById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid client ID", nameof(id));

                _logger.LogInformation("Getting client with ID {Id}", id);
                var client = _context.Clients.Find(id);
                
                if (client == null)
                {
                    _logger.LogWarning("Client with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found client: {Username}", client.Username);
                }
                
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client with ID {Id}", id);
                throw;
            }
        }

        public Client AddClient(Client client)
        {
            try
            {
                if (client == null)
                    throw new ArgumentNullException(nameof(client));

                if (string.IsNullOrWhiteSpace(client.Username))
                    throw new ArgumentException("Username is required", nameof(client));

                if (string.IsNullOrWhiteSpace(client.Email))
                    throw new ArgumentException("Email is required", nameof(client));

                if (string.IsNullOrWhiteSpace(client.PasswordHash))
                    throw new ArgumentException("Password is required", nameof(client));

                // Check if username or email already exists
                var existingClient = _context.Clients
                    .FirstOrDefault(c => c.Username == client.Username || c.Email == client.Email);

                if (existingClient != null)
                {
                    _logger.LogWarning("Client with username {Username} or email {Email} already exists", 
                        client.Username, client.Email);
                    throw new InvalidOperationException("Username or email already exists");
                }

                _logger.LogInformation("Adding new client: {Username}", client.Username);
                _context.Clients.Add(client);
                _context.SaveChanges();
                _logger.LogInformation("Successfully added client with ID {Id}", client.Id);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding client");
                throw;
            }
        }

        public Client UpdateClient(Client client)
        {
            try
            {
                if (client == null)
                    throw new ArgumentNullException(nameof(client));

                if (string.IsNullOrWhiteSpace(client.Username))
                    throw new ArgumentException("Username is required", nameof(client));

                if (string.IsNullOrWhiteSpace(client.Email))
                    throw new ArgumentException("Email is required", nameof(client));

                _logger.LogInformation("Updating client with ID {Id}", client.Id);
                var existing = _context.Clients.Find(client.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Client with ID {Id} not found", client.Id);
                    throw new KeyNotFoundException($"Client with ID {client.Id} not found");
                }

                // Check if username or email already exists for other clients
                var duplicateClient = _context.Clients
                    .FirstOrDefault(c => (c.Username == client.Username || c.Email == client.Email) && c.Id != client.Id);

                if (duplicateClient != null)
                {
                    _logger.LogWarning("Another client with username {Username} or email {Email} already exists", 
                        client.Username, client.Email);
                    throw new InvalidOperationException("Username or email already exists");
                }

                existing.Username = client.Username;
                existing.Email = client.Email;
                if (!string.IsNullOrWhiteSpace(client.PasswordHash))
                {
                    existing.PasswordHash = client.PasswordHash;
                }

                _context.SaveChanges();
                _logger.LogInformation("Successfully updated client with ID {Id}", client.Id);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client with ID {Id}", client.Id);
                throw;
            }
        }
    }
}
