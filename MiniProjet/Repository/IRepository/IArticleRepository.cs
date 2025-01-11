using MiniProjet.Models;

namespace MiniProjet.Repository.IRepository
{
    public interface IArticleRepository
    {
        public List<Article> GetArticle();

        public Article GetArticleById(int id);

        public Article AddArticle(Article article);

        public Article UpdateArticle(Article article);

    }
}
