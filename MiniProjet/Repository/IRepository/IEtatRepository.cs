using Shared.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IEtatRepository
    {
        List<Etat> GetAll();
        Etat? GetById(int id);
        Etat Add(Etat etat);
        bool Update(Etat etat);
        bool Delete(int id);
    }
} 