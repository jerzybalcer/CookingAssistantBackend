using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.Database;
using CookingAssistantBackend.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly CookingAssistantContext _context;

        public LikesController(CookingAssistantContext context)
        {
            _context = context;
        }

        [HttpGet("GetById/{likeId}", Name = nameof(GetById))]
        public async Task<ActionResult<LikeDto>> GetById(int likeId)
        {
            var like = await _context.Likes
                .Where(l => l.LikeId == likeId)
                .Include(l => l.Comment).Include(l => l.LikedBy)
                .Select(l => new LikeDto(l.LikeId, l.Comment.CommentId, l.LikedBy.UserId))
                .FirstOrDefaultAsync();

            if(like == null)
            {
                return NotFound("Like not found");
            }

            return Ok(like);
        }

        [HttpGet("GetByCommment/{commentId}", Name = "GetByComment")]
        public async Task<ActionResult<List<LikeDto>>> GetCommentLikes(int commentId)
        {
            var comment = await _context.Comments.Include(c => c.Likes).ThenInclude(l => l.LikedBy).FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            var likes = comment.Likes.Select(l => new LikeDto(l.LikeId, l.Comment.CommentId, l.LikedBy.UserId)).ToList();

            return Ok(likes);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddLike(LikeDto newLike)
        {
            if(newLike == null)
            {
                return BadRequest("Invalid like object");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == newLike.LikedById);

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == newLike.CommentId);

            if(user == null || comment == null)
            {
                return NotFound("Cannot assign like to user or comment");
            }

            var like = _context.Likes.Add(new Like(comment, user));
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { likeId = like.Entity.LikeId }, like.Entity);   
        }
    }
}
