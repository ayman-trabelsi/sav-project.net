using MiniProjet.Models;
using MiniProjet.ModelsDto;

namespace MiniProjet.Repository.IRepository
{
    public interface IInterventionRepository
    {
        Intervention Add(InterventionRequestDto intervention);    

        Intervention Update(Intervention intervention);

        List<Intervention> GetAll();
    }
}
