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
    public class TagsController : CustomController
    {
        private readonly CookingAssistantContext _context;
        private readonly IMapper _mapper;

        public TagsController(CookingAssistantContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _context.Tags.ToListAsync();

            return Ok(_mapper.Map<List<TagDto>>(tags));
        }

        [HttpGet("GetByRecipe/{recipeId}")]
        public async Task<IActionResult> GetByRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.Include(r => r.Tags).FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }

            return Ok(_mapper.Map<List<TagDto>>(recipe.Tags));
        }

        [HttpPost("AddToRecipe/{recipeId}")]
        public async Task<IActionResult> AddToRecipe(int recipeId, TagDto tagDto)
        {
            var recipe = await _context.Recipes.Include(r => r.Tags).FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if(tagDto == null)
            {
                return BadRequest("Invalid tag object");
            }

            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }

            var tag = _mapper.Map<Tag>(tagDto);

            var existingTag = await _context.Tags.Include(t => t.Recipes).Where(t => t.Name == tag.Name).FirstOrDefaultAsync();

            if (existingTag == null)
            {
                tag.Recipes = new List<Recipe> { recipe };
                _context.Tags.Add(tag);
            }
            else
            {
                existingTag.Recipes.Add(recipe);
                tag = existingTag;
            }

            recipe.Tags.Add(tag);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetByRecipe", new { recipeId = recipe.RecipeId }, _mapper.Map<TagDto>(tag));
        }

        [HttpDelete("DeleteAtId/{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
