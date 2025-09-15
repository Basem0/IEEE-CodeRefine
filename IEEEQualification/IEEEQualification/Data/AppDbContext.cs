using IEEEQualification.Models;
using Microsoft.EntityFrameworkCore;

namespace IEEEQualification.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ieeequalification.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.Watchlist)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.Movie)
                .WithMany(m => m.WatchlistItems)
                .HasForeignKey(w => w.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Category);

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Author);

            modelBuilder.Entity<Review>()
                .HasIndex(r => r.MovieId);

            modelBuilder.Entity<Review>()
                .HasIndex(r => r.UserId);

            modelBuilder.Entity<WatchlistItem>()
                .HasIndex(w => w.UserId);

            modelBuilder.Entity<WatchlistItem>()
                .HasIndex(w => new { w.UserId, w.MovieId })
                .IsUnique();
        }
    }
}