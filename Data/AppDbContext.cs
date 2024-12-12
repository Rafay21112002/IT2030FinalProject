using Microsoft.EntityFrameworkCore;
using IT2030FinalProject.Models;

namespace IT2030FinalProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial character data
            modelBuilder.Entity<Character>().HasData(
                new Character
                {
                    Id = 1,
                    Name = "Charles Zi Britanina",
                    About = "Empereror of The Holy Empire Of Britanina.",
                    Powers = "His geass allows him to erase and modify a person's memories."
                }
            );

            // Seed initial organization data
            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = 1,
                    Name = "Holy Empire Of Britanina",
                    Description = "One of the dominant nations in the world of Code Geass. Is rifed with corruption.",
                    LeaderId = 1
                }
            );

            // Configure relationships
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.Leader)
                .WithMany()
                .HasForeignKey(o => o.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Character)
                .WithMany(ch => ch.Comments)
                .HasForeignKey(c => c.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Organization)
                .WithMany(o => o.Comments)
                .HasForeignKey(c => c.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}