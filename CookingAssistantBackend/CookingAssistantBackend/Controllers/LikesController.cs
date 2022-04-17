using AutoMapper;
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
        private readonly IMapper _mapper;

        public LikesController(CookingAssistantContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetById/{likeId}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int likeId)
        {
            var like = await _context.Likes
                .Where(l => l.LikeId == likeId)
                .Include(l => l.Comment).Include(l => l.LikedBy)
                .FirstOrDefaultAsync();

            if(like == null)
            {
                return NotFound("Like not found");
            }

            return Ok(_mapper.Map<LikeDto>(like));
        }

        [HttpGet("GetByCommment/{commentId}", Name = "GetByComment")]
        public async Task<IActionResult> GetCommentLikes(int commentId)
        {
            var comment = await _context.Comments.Include(c => c.Likes).ThenInclude(l => l.LikedBy).FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(_mapper.Map<List<LikeDto>>(comment.Likes.ToList()));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddLike(LikeDto newLikeDto)
        {
            if(newLikeDto == null)
            {
                return BadRequest("Invalid like object");
            }

            var like = _mapper.Map<Like>(newLikeDto);

            var user = _context.Attach(like.LikedBy);
            var comment = _context.Attach(like.Comment);

            if(user.State != EntityState.Unchanged || comment.State != EntityState.Unchanged)
            {
                return BadRequest("Cannot assign like to any user or comment");
            }

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { likeId = like.LikeId }, _mapper.Map<LikeDto>(like));   
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
