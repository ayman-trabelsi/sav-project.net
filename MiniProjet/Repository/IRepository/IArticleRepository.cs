using Shared.Models;

public interface IArticleRepository
{
    List<Article> GetAll();
    Article? GetById(int id);
    Article Add(Article article);
    bool Update(Article article);
    bool Delete(int id);
}