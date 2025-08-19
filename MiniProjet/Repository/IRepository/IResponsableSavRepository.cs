using Shared.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IClientRepository
    {
        public List<Client> GetClients();

        public Client GetClientById(int id);

        public Client AddClient(Client client);

        public Client UpdateClient(Client client);

    }
}
