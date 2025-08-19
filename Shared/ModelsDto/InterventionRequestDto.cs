using Shared.Models;


namespace Shared.ModelsDto
{
    public class InterventionRequestDto 
    {
        public Intervention intervention { get; set; } = new();
        public List<PieceRechange>? pieceRechanges { get; set; }
        public List<int>? pieceRechangeIds { get; set; }
    }
}
