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
using AutoMapper;
using CookingAssistantBackend.Models.DTOs;

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeStepsController : CustomController
    {
        private readonly CookingAssistantContext _context;
        private readonly IMapper _mapper;

        public RecipeStepsController(CookingAssistantContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetRecipeStep(int id)
        {
            var recipeStep = await _context.RecipeSteps.FindAsync(id);

            if (recipeStep == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RecipeStepDto>(recipeStep));
        }

        [HttpPut("UpdateAtId/{id}")]
        public async Task<IActionResult> PutRecipeStep(int id, RecipeStepDto recipeStepDto)
        {
            if (id != recipeStepDto.RecipeStepId)
            {
                return BadRequest();
            }

            var step = await _context.RecipeSteps.AsNoTracking().FirstOrDefaultAsync(rs => rs.RecipeStepId == id);

            if(step == null)
            {
                return NotFound("Cannot find step");
            }

            step = _mapper.Map<RecipeStep>(recipeStepDto);
            _context.Entry(step).State = EntityState.Modified;

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
        [HttpPost("AddToRecipe")]
        public async Task<IActionResult> PostRecipeStep(int recipeId, RecipeStepDto recipeStepDto)
        {
            var recipeStep = _mapper.Map<RecipeStep>(recipeStepDto);

            recipeStep.Comments = new List<Comment>();

            var recipe = await _context.Recipes.Include(r => r.Steps).FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if(recipe == null)
            {
                return BadRequest("Recipe not found");
            }

            _context.RecipeSteps.Add(recipeStep);

            recipe.Steps.Add(recipeStep);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipeStep", new { id = recipeStep.RecipeStepId }, _mapper.Map<RecipeStepDto>(recipeStep));
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