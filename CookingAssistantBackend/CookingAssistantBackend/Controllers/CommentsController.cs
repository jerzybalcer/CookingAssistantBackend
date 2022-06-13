#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.Database;
using CookingAssistantBackend.Utilis;
using CookingAssistantBackend.Models.DTOs;
using AutoMapper;

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : CustomController
    {
        private readonly CookingAssistantContext _context;
        private readonly IMapper _mapper;

        public CommentsController(CookingAssistantContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(CommentDto), 200)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Likes)
                .Include(c => c.WrittenBy)
                .Where(c => c.CommentId == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [ProducesResponseType(typeof(List<CommentDto>), 200)]
        [HttpGet("GetByStep/{stepId}")]
        public async Task<IActionResult> GetStepComments(int stepId)
        {
            var step = await _context.RecipeSteps
                .Include(rs => rs.Comments)
                .ThenInclude(c => c.WrittenBy)
                .ThenInclude(c => c.Likes)
                .FirstOrDefaultAsync(rs => rs.RecipeStepId == stepId);

            if (step == null)
            {
                return NotFound("Step not found");
            }

            var comments = step.Comments.ToList();

            return Ok(_mapper.Map<List<CommentDto>>(comments));
        }

        [HttpPost("AddToStep")]
        public async Task<IActionResult> PostComment(NewCommentDto newComment)
        {
            var recipeStep = await _context.RecipeSteps
                .Include(rs => rs.Comments)
                .Where(r => r.RecipeStepId == newComment.StepId)
                .FirstOrDefaultAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == newComment.WrittenById);

            if(user == null)
            {
                return NotFound("Could find the author of the comment");
            }

            var fullComment = new Comment(newComment.CommentText, user);

            recipeStep.Comments.Add(fullComment);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = fullComment.CommentId }, _mapper.Map<CommentDto>(fullComment));
        }

        [HttpDelete("DeleteAtId")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
