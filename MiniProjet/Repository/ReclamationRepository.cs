using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class ReclamationRepository : IReclamationRepository
    {
        private readonly ApplicationDbContext _context;
      public   ReclamationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Reclamation IReclamationRepository.AddReclamation(Reclamation reclamation)
        {
            try
            {
                _context.Reclamations.Add(reclamation);
                _context.SaveChanges();
                return reclamation;
            }
            catch
            {
                return null;
            }
        }

        List<Reclamation> IReclamationRepository.GetAll()
        {
            return _context.Reclamations.ToList();
        }
    }
}
