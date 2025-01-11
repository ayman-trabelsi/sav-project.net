using MiniProjet.Models;

namespace MiniProjet.ModelsDto
{
    public class InterventionRequestDto 
    {
        public Intervention intervention { get; set; }
      public  List<PieceRechange>? pieceRechanges { get; set; }
    }
}
