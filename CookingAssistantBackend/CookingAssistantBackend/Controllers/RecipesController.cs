﻿#nullable disable
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
    public class RecipesController : ControllerBase
    {
        private readonly CookingAssistantContext _context;

        public RecipesController(CookingAssistantContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes(int count, int offset)
        {
            return await _context.Recipes.Skip(offset).Take(count).OrderByDescending(x => x.Name).ToListAsync();
        }

        // GET: api/Recipes/SearchById/?id=2
        [HttpGet("SearchById")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Where(rec => rec.RecipeId == id)
                .Include(ing => ing.Ingredients)
                .Include(steps => steps.Steps)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // GET: api/Recipes/SearchByName/?name=kanapka
        [HttpGet("SearchByName")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipe(string name, int count, int offset)
        {
            var recipe = await _context.Recipes.Skip(offset).Take(count).Where(rec => rec.Name.Contains(name)).OrderByDescending(x => x.Name).ToListAsync(); ;

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // GET: api/Recipes/SearchByName/?name=kanapka
        [HttpPost("SearchByTags")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipe(List<string> tagsList, int count, int offset)
        {
            var Recipes = await _context.Recipes
                .Where(r => r.Tags.Any() && r.Tags
                .All(tag => tagsList
                .Any(x => tag.Name == x)))
                .Include(t => t.Tags)
                .Skip(offset)
                .Take(count)
                .OrderByDescending(x => x.Name)
                .ToListAsync();

            if (Recipes == null)
            {
                return NotFound();
            }

            return Recipes;

            return NotFound();
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
