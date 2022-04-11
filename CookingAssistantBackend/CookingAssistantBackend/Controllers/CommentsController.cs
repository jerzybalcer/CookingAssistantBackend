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

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : CustomController
    {
        private readonly CookingAssistantContext _context;

        public CommentsController(CookingAssistantContext context)
        {
            _context = context;
        }

        // GET: api/Comments/ById?id=1
        [HttpGet("ById")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _context.Comments
                .Where(r => r.CommentId == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // GET: api/Comments/ByStepId?stepId=1
        [HttpGet("ByStepId")]
        public async Task<IActionResult> GetStepComments(int stepId)
        {
            var recipeStep = await _context.RecipeSteps
                .Where(r => r.RecipeStepId == stepId)
                .FirstOrDefaultAsync();

            if (recipeStep == null)
            {
                return NotFound();
            }

            return Ok(recipeStep.Comments.ToList());
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateAtId")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddToStep")]
        public async Task<ActionResult<Comment>> PostComment(int stepId, Comment comment)
        {
            var recipeStep = await _context.RecipeSteps
                .Where(r => r.RecipeStepId == stepId)
                .FirstOrDefaultAsync();

            recipeStep.Comments.Add(comment);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, comment);
        }

        // DELETE: api/Comments/5
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

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
