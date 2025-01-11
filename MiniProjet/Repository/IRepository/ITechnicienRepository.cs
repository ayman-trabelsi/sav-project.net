using MiniProjet.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface ITechnicienRepository
    {
        Technicien AddTechnicien(Technicien technicien);

        List<Technicien> GetAll();
    }
}
