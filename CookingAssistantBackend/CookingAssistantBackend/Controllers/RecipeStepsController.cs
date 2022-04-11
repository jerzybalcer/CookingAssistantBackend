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

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeStepsController : ControllerBase
    {
        private readonly CookingAssistantContext _context;

        public RecipeStepsController(CookingAssistantContext context)
        {
            _context = context;
        }


        [HttpGet("RecipeSteps")]
        public async Task<ActionResult<RecipeStep>> GetRecipeStep(int id)
        {
            var recipeStep = await _context.RecipeSteps.FindAsync(id);

            if (recipeStep == null)
            {
                return NotFound();
            }

            return recipeStep;
        }

        // PUT: api/RecipeSteps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeStep(int id, RecipeStep recipeStep)
        {
            if (id != recipeStep.RecipeStepId)
            {
                return BadRequest();
            }

            _context.Entry(recipeStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeStepExists(id))
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

        // POST: api/RecipeSteps
        [HttpPost]
        public async Task<ActionResult<RecipeStep>> PostRecipeStep(RecipeStep recipeStep)
        {
            _context.RecipeSteps.Add(recipeStep);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipeStep", new { id = recipeStep.RecipeStepId }, recipeStep);
        }

        // DELETE: api/RecipeSteps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeStep(int id)
        {
            var recipeStep = await _context.RecipeSteps.FindAsync(id);
            if (recipeStep == null)
            {
                return NotFound();
            }

            _context.RecipeSteps.Remove(recipeStep);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeStepExists(int id)
        {
            return _context.RecipeSteps.Any(e => e.RecipeStepId == id);
        }
    }
}