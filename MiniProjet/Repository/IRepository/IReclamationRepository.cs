using MiniProjet.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IReclamationRepository
    {
        Reclamation AddReclamation(Reclamation reclamation);

        List<Reclamation> GetAll();
    }

}
