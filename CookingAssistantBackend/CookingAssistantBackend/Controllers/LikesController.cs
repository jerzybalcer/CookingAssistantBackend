using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.Database;
using CookingAssistantBackend.Models.DTOs;
using CookingAssistantBackend.Utilis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : CustomController
    {
        private readonly CookingAssistantContext _context;

        public LikesController(CookingAssistantContext context)
        {
            _context = context;
        }

        [HttpGet("GetById/{likeId}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int likeId)
        {
            var like = await _context.Likes
                .Where(l => l.LikeId == likeId)
                .Include(l => l.Comment).Include(l => l.LikedBy)
                .Select(l => new LikeDto(l.LikeId, l.Comment.CommentId, l.LikedBy.UserId, l.LikedBy.Name))
                .FirstOrDefaultAsync();

            if(like == null)
            {
                return NotFound("Like not found");
            }

            return Ok(like);
        }

        [HttpGet("GetByCommment/{commentId}", Name = "GetByComment")]
        public async Task<IActionResult> GetCommentLikes(int commentId)
        {
            var comment = await _context.Comments.Include(c => c.Likes).ThenInclude(l => l.LikedBy).FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            var likes = comment.Likes.Select(l => new LikeDto(l.LikeId, l.Comment.CommentId, l.LikedBy.UserId, l.LikedBy.Name)).ToList();

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

            var fullNewLike = _context.Likes.Add(new Like(comment, user));
            await _context.SaveChangesAsync();

            newLike.LikeId = fullNewLike.Entity.LikeId;

            return CreatedAtAction(nameof(GetById), new { likeId = fullNewLike.Entity.LikeId }, newLike);   
        }

        [HttpDelete("DeleteAtId/{likeId}")]
        public async Task<IActionResult> Delete(int likeId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(c => c.LikeId == likeId);

            if(like == null)
            {
                return NotFound("Like not found");
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
