using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProjet.Models;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;

namespace MiniProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _repo;

        public ArticleController(IArticleRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(_repo.GetArticle());
        }
        [HttpPost]
        public IActionResult addArticle(Article article)
        {
            var result = _repo.AddArticle(article);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}