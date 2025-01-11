using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Article IArticleRepository.AddArticle(Article article)
        {
            try { 

                _context.Articles.Add(article);
                _context.SaveChanges();
                return article;
            }
            catch {

                return null;
            }
        }

        List<Article> IArticleRepository.GetArticle()
        {
            return _context.Articles.ToList();  
        }

        Article IArticleRepository.GetArticleById(int id)
        {
            throw new NotImplementedException();
        }

        Article IArticleRepository.UpdateArticle(Article article)
        {
            throw new NotImplementedException();
        }
    }
}
