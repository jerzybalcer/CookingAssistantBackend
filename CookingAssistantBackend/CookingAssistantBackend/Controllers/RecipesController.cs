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
    public class RecipesController : CustomController
    {
        private readonly CookingAssistantContext _context;
        private readonly IMapper _mapper;

        public RecipesController(CookingAssistantContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Recipes
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Steps).ThenInclude(s => s.Comments).ThenInclude(c => c.WrittenBy).ThenInclude(c => c.Likes)
                .Include(r => r.Ingredients)
                .Include(r => r.Tags)
                .Include(r => r.User).OrderByDescending(r => r.Name).ToListAsync();

            return Ok(_mapper.Map<List<RecipeDto>>(recipes));
        }

        // GET: api/Recipes/SearchById/?id=2
        [HttpGet("SearchById")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Where(rec => rec.RecipeId == id)
                .Include(ing => ing.Ingredients)
                .Include(steps => steps.Steps).ThenInclude(s => s.Comments).ThenInclude(c => c.WrittenBy).ThenInclude(c => c.Likes)
                .Include(tg => tg.Tags)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RecipeDto>(recipe));
        }

        // GET: api/Recipes/SearchByName/?name=kanapka
        [HttpGet("SearchByName")]
        public async Task<IActionResult> GetRecipe(string name)
        {
            var recipes = await _context.Recipes
                .Include(r => r.Steps).ThenInclude(s => s.Comments).ThenInclude(c => c.WrittenBy).ThenInclude(c => c.Likes)
                .Include(r => r.Ingredients)
                .Include(r => r.Tags)
                .Include(r => r.User)
                .Where(rec => rec.Name.Contains(name))
                .OrderByDescending(x => x.Name)
                .ToListAsync();

            if (recipes == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<RecipeDto>>(recipes));
        }

        // GET: api/Recipes/SearchByName/?name=kanapka
        [HttpPost("SearchByTags")]
        public async Task<IActionResult> GetRecipe(List<string> tagsList)
        {
            var recipes = await _context.Recipes
                .Include(r => r.Steps).ThenInclude(s => s.Comments).ThenInclude(c => c.WrittenBy).ThenInclude(c => c.Likes)
                .Include(r => r.Ingredients)
                .Include(r => r.Tags)
                .Include(r => r.User)
                .Where(r => r.Tags.Any() && r.Tags.All(tag => tagsList.Any(x => tag.Name == x)))
                .OrderByDescending(x => x.Name)
                .ToListAsync();

            if (recipes == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<RecipeDto>>(recipes));
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, RecipeDto recipeDto)
        {
            if (id != recipeDto.RecipeId)
            {
                return BadRequest();
            }

            var oldRecipe = await _context.Recipes.AsTracking().FirstOrDefaultAsync(r => r.RecipeId == id);

            if(oldRecipe == null)
            {
                return NotFound("Recipe not found");
            }

            var newRecipe = _mapper.Map<Recipe>(recipeDto);

            oldRecipe.Name = newRecipe.Name;
            oldRecipe.Steps = newRecipe.Steps;
            oldRecipe.Ingredients = newRecipe.Ingredients;
            oldRecipe.Tags = newRecipe.Tags;

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
        public async Task<ActionResult<Recipe>> PostRecipe(RecipeDto recipeDto)
        {
            var recipe = _mapper.Map<Recipe>(recipeDto);
            
            _context.Attach(recipe);

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.RecipeId }, _mapper.Map<RecipeDto>(recipe));
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
