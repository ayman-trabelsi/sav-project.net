using MiniProjet.ModelsDto;

namespace MiniProjet.Repository.IRepository
{
    public interface IAuthentificationRepository
    {
        public string Authentifier(UtilisateurDto user);

    }
}
