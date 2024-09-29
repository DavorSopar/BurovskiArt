using ArtistPortfolio.Models.Identity;
using ArtistPortfolio.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtistPortfolio.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Image>? Images { get; set; }
    public virtual DbSet<Biography>? Biography { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Biography>().Property(e => e.Id).ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}

