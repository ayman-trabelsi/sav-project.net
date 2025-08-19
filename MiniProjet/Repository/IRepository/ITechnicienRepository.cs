using Shared.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface ITechnicienRepository
    {
        Technicien AddTechnicien(Technicien technicien);

        List<Technicien> GetAll();

        Technicien? GetById(int id);

        Technicien? Update(Technicien technicien);

        bool Delete(int id);

        List<Intervention> GetInterventions(int technicienId);
    }
}
