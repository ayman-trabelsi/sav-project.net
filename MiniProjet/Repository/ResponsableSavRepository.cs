using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class ResponsableSavRepository : IResponsableSAVRepository
    {
        private readonly ApplicationDbContext _context;
        public ResponsableSavRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        ResponsableSAV IResponsableSAVRepository.AddResponsableSAV(ResponsableSAV ResponsableSAV)
        {
            try
            {
                _context.ResponsableSAV.Add(ResponsableSAV);
                _context.SaveChanges();
                return ResponsableSAV;
            }
            catch
            {
                return null;
            }
            
        }

        ResponsableSAV IResponsableSAVRepository.GetResponsableSAVById(int id)
        {
           return _context.ResponsableSAV.First(c => c.Id == id);  
        }

        List<ResponsableSAV> IResponsableSAVRepository.GetResponsableSAVs()
        {
            return _context.ResponsableSAV.ToList();
        }

        ResponsableSAV IResponsableSAVRepository.UpdateResponsableSAV(ResponsableSAV ResponsableSAV)
        {
            _context.ResponsableSAV.Update(ResponsableSAV);
            _context.SaveChanges();
            return ResponsableSAV;
        }
    }
}
