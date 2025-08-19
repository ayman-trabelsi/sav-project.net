using Shared.Models;
using System.Collections.Generic;

namespace MiniProjet.Repository.IRepository
{
    public interface IPieceRechangeRepository
    {
        List<PieceRechange> GetAll();
        PieceRechange? GetById(int id);
        List<PieceRechange> GetByArticleId(int articleId);
        PieceRechange? AddPieceRechange(PieceRechange pieceRechange);
        bool Update(PieceRechange pieceRechange);
        bool Delete(int id);
    }
}