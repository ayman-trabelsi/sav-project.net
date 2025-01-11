using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Client IClientRepository.AddClient(Client client)
        {
            _context.Clients.Add(client);   
            _context.SaveChanges();
            return client;
        }

        Client IClientRepository.GetClientById(int id)
        {
           return _context.Clients.First(c => c.Id == id);  
        }

        List<Client> IClientRepository.GetClients()
        {
            return _context.Clients.ToList();
        }

        Client IClientRepository.UpdateClient(Client client)
        {
            _context.Clients.Update(client);
            _context.SaveChanges();
            return client;
        }
    }
}
