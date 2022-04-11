using Microsoft.EntityFrameworkCore;

namespace CookingAssistantBackend.Models.Database
{
    public class CookingAssistantContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public CookingAssistantContext(DbContextOptions<CookingAssistantContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:tab-projekt.database.windows.net,1433;Initial Catalog=cooking-assistant;Persist Security Info=False;User ID=tab;Password=Projekt1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasOne(c => c.WrittenBy).WithMany(u => u.Comments).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Like>().HasOne(l => l.Comment).WithMany(c => c.Likes).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Like>().HasOne(l => l.LikedBy).WithMany(u => u.Likes).OnDelete(DeleteBehavior.NoAction);
        }
    }
}