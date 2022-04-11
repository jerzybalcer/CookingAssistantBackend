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
    public class RecipesController : CustomController
    {
        private readonly CookingAssistantContext _context;

        public RecipesController(CookingAssistantContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var result = await _context.Recipes.OrderByDescending(x => x.Name).ToListAsync();
            return Ok(result);
        }

        // GET: api/Recipes/SearchById/?id=2
        [HttpGet("SearchById")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Where(rec => rec.RecipeId == id)
                .Include(ing => ing.Ingredients)
                .Include(steps => steps.Steps)
                .Include(tg=> tg.Tags)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        // GET: api/Recipes/SearchByName/?name=kanapka
        [HttpGet("SearchByName")]
        public async Task<IActionResult> GetRecipe(string name)
        {
            var recipe = await _context.Recipes
                .Where(rec => rec.Name.Contains(name))
                .OrderByDescending(x => x.Name)
                .ToListAsync(); ;

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        // GET: api/Recipes/SearchByName/?name=kanapka
        [HttpPost("SearchByTags")]
        public async Task<IActionResult> GetRecipe(List<string> tagsList)
        {
            var Recipes = await _context.Recipes
                .Where(r => r.Tags.Any() && r.Tags.All(tag => tagsList.Any(x => tag.Name == x)))
                .Include(t => t.Tags)
                .OrderByDescending(x => x.Name)
                .ToListAsync();

            if (Recipes == null)
            {
                return NotFound();
            }

            return Ok(Recipes);
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        // POST: api/Recipes
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.RecipeId }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }
    }
}
