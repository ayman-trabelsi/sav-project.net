using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class PieceRechangeRepository : IPieceRechangeRepository
    {
        private readonly ApplicationDbContext _context;
        public PieceRechangeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        PieceRechange IPieceRechangeRepository.AddPieceRechange(PieceRechange piece)
        {
            try
            {
                _context.Add(piece);
                _context.SaveChanges(); 
                return piece;   

            }
            catch {

                return null;
            }
        }

        List<PieceRechange> IPieceRechangeRepository.GetAll()
        {
            return _context.PiecesRechange.ToList();
        }
    }
}
