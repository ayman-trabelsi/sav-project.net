using MiniProjet.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IPieceRechangeRepository
    {
        PieceRechange AddPieceRechange(PieceRechange piece);

        List<PieceRechange> GetAll();
    }
}
