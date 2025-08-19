using Shared.Models;
using Shared.ModelsDto;

namespace MiniProjet.Repository.IRepository
{
    public interface IInterventionRepository
    {
        Intervention Add(InterventionRequestDto intervention);    

        Intervention Update(Intervention intervention);

        List<Intervention> GetAll();

        Intervention? GetById(int id);

        bool Delete(int id);
    }
}
