using MiniProjet.Models;
using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class InterventionRepository : IInterventionRepository
    {
        private readonly ApplicationDbContext _context;

        public InterventionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Intervention IInterventionRepository.Add(InterventionRequestDto intervention)
        {
            try
            {
                Reclamation reclamation = _context.Reclamations.First(e => e.Id == intervention.intervention.ReclamationId);

                Article article = _context.Articles.First(e => e.Id == reclamation.idArticleReclamation);

                if (article.EstSousGarantie)
                {
                    intervention.intervention.MontantFacture = 0;
                }
                else
                {
                    intervention.intervention.MontantFacture = intervention.pieceRechanges.Sum(e => e.Prix);
                }
                _context.Add(intervention.intervention);
                _context.SaveChanges();

                //Reclamation reclamation = _context.Reclamations.First(e=>e.Id== intervention.intervention.ReclamationId);
                reclamation.EtatId = 2;
                reclamation.InterventionId = intervention.intervention.Id;
                _context.Update(reclamation);
                _context.SaveChanges();
                return intervention.intervention;
            }
            catch (Exception ex) {
                return null;
            }
        }

        List<Intervention> IInterventionRepository.GetAll()
        {
            return _context.Interventions.ToList();
        }

        Intervention IInterventionRepository.Update(Intervention intervention)
        {
            throw new NotImplementedException();
        }
    }
}
