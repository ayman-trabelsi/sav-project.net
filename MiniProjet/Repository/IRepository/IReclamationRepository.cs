using Shared.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IReclamationRepository
    {
        List<Reclamation> GetAll();
        List<Reclamation> GetReclamationsByClientId(int clientId);
        Reclamation AddReclamation(Reclamation reclamation);
        Reclamation UpdateReclamation(Reclamation reclamation);
        bool DeleteReclamation(int id);
        Reclamation GetReclamation(int id);
    }
}