using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class TechnicienRepository : ITechnicienRepository
    {
        private readonly ApplicationDbContext _context;
        public TechnicienRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Technicien ITechnicienRepository.AddTechnicien(Technicien technicien)
        {
            try
            {
                _context.Add(technicien);
                _context.SaveChanges();
                return technicien;
            }
            catch
            {
                return null;
            }
        }

        List<Technicien> ITechnicienRepository.GetAll()
        {
            return _context.Techniciens.ToList();   
        }
    }
}
