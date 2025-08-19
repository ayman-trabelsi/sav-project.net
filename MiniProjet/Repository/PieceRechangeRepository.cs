using Microsoft.EntityFrameworkCore;
using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Repository
{
    public class PieceRechangeRepository : IPieceRechangeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PieceRechangeRepository> _logger;

        public PieceRechangeRepository(ApplicationDbContext context, ILogger<PieceRechangeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<PieceRechange> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all pieces rechange");
                var pieces = _context.PiecesRechange
                    .Include(p => p.Article)
                    .AsNoTracking()
                    .ToList();
                _logger.LogInformation("Found {Count} pieces rechange", pieces.Count);
                return pieces;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all pieces rechange");
                throw;
            }
        }

        public List<PieceRechange> GetByArticleId(int articleId)
        {
            try
            {
                if (articleId <= 0)
                    throw new ArgumentException("Invalid article ID", nameof(articleId));

                _logger.LogInformation("Getting pieces rechange for article ID {ArticleId}", articleId);
                var pieces = _context.PiecesRechange
                    .Include(p => p.Article)
                    .Where(p => p.ArticleId == articleId)
                    .AsNoTracking()
                    .ToList();
                
                _logger.LogInformation("Found {Count} pieces rechange for article {ArticleId}", pieces.Count, articleId);
                return pieces;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pieces rechange for article {ArticleId}", articleId);
                throw;
            }
        }

        public PieceRechange? GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid piece rechange ID", nameof(id));

                _logger.LogInformation("Getting piece rechange with ID {Id}", id);
                var piece = _context.PiecesRechange
                    .Include(p => p.Article)
                    .FirstOrDefault(p => p.Id == id);
                
                if (piece == null)
                {
                    _logger.LogWarning("Piece rechange with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("Found piece rechange: {Nom}", piece.Nom);
                }
                
                return piece;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting piece rechange {Id}", id);
                throw;
            }
        }

        public PieceRechange? AddPieceRechange(PieceRechange pieceRechange)
        {
            try
            {
                if (pieceRechange == null)
                    throw new ArgumentNullException(nameof(pieceRechange));

                if (string.IsNullOrWhiteSpace(pieceRechange.Nom))
                    throw new ArgumentException("Nom is required", nameof(pieceRechange));

                if (pieceRechange.Prix < 0)
                    throw new ArgumentException("Price cannot be negative", nameof(pieceRechange));

                if (pieceRechange.ArticleId <= 0)
                    throw new ArgumentException("Article ID is required", nameof(pieceRechange));

                _logger.LogInformation("Adding new piece rechange: {Nom}", pieceRechange.Nom);
                _context.PiecesRechange.Add(pieceRechange);
                _context.SaveChanges();

                var savedPiece = _context.PiecesRechange
                    .Include(p => p.Article)
                    .FirstOrDefault(p => p.Id == pieceRechange.Id);

                if (savedPiece == null)
                {
                    _logger.LogError("Failed to retrieve saved piece rechange");
                    throw new InvalidOperationException("Failed to retrieve saved piece rechange");
                }

                _logger.LogInformation("Successfully added piece rechange with ID {Id}", savedPiece.Id);
                return savedPiece;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding piece rechange");
                throw;
            }
        }

        public bool Update(PieceRechange pieceRechange)
        {
            try
            {
                if (pieceRechange == null)
                    throw new ArgumentNullException(nameof(pieceRechange));

                if (pieceRechange.Id <= 0)
                    throw new ArgumentException("Invalid piece rechange ID", nameof(pieceRechange));

                if (string.IsNullOrWhiteSpace(pieceRechange.Nom))
                    throw new ArgumentException("Nom is required", nameof(pieceRechange));

                if (pieceRechange.Prix < 0)
                    throw new ArgumentException("Price cannot be negative", nameof(pieceRechange));

                if (pieceRechange.ArticleId <= 0)
                    throw new ArgumentException("Article ID is required", nameof(pieceRechange));

                _logger.LogInformation("Updating piece rechange with ID {Id}", pieceRechange.Id);
                var existingPiece = _context.PiecesRechange.Find(pieceRechange.Id);
                if (existingPiece == null)
                {
                    _logger.LogWarning("Piece rechange with ID {Id} not found", pieceRechange.Id);
                    return false;
                }

                _context.Entry(existingPiece).CurrentValues.SetValues(pieceRechange);
                _context.SaveChanges();
                _logger.LogInformation("Successfully updated piece rechange with ID {Id}", pieceRechange.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating piece rechange {Id}", pieceRechange.Id);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid piece rechange ID", nameof(id));

                _logger.LogInformation("Deleting piece rechange with ID {Id}", id);
                var piece = _context.PiecesRechange.Find(id);
                if (piece == null)
                {
                    _logger.LogWarning("Piece rechange with ID {Id} not found", id);
                    return false;
                }

                _context.PiecesRechange.Remove(piece);
                _context.SaveChanges();
                _logger.LogInformation("Successfully deleted piece rechange with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting piece rechange {Id}", id);
                throw;
            }
        }
    }
}