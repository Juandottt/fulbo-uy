using FulboUY.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FulboUY.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PlayerProfile> PlayerProfiles => Set<PlayerProfile>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<MatchParticipant> MatchParticipants => Set<MatchParticipant>();
    public DbSet<InviteLink> InviteLinks => Set<InviteLink>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(256);
            e.Property(u => u.PasswordHash).IsRequired();
        });

        // PlayerProfile
        modelBuilder.Entity<PlayerProfile>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<PlayerProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Match
        modelBuilder.Entity<Match>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Location).IsRequired().HasMaxLength(200);
            e.Property(m => m.FieldCost).HasColumnType("decimal(10,2)");
        });

        // MatchParticipant
        modelBuilder.Entity<MatchParticipant>(e =>
        {
            e.HasKey(mp => mp.Id);
            e.HasIndex(mp => new { mp.MatchId, mp.PlayerProfileId }).IsUnique();
            e.HasOne(mp => mp.Match)
                .WithMany(m => m.Participants)
                .HasForeignKey(mp => mp.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(mp => mp.PlayerProfile)
                .WithMany(p => p.Participations)
                .HasForeignKey(mp => mp.PlayerProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // InviteLink
        modelBuilder.Entity<InviteLink>(e =>
        {
            e.HasKey(il => il.Id);
            e.HasIndex(il => il.Token).IsUnique();
            e.HasOne(il => il.Match)
                .WithMany(m => m.InviteLinks)
                .HasForeignKey(il => il.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Product
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired();
            e.Property(p => p.Description).IsRequired();
            e.Property(p => p.Price).HasColumnType("decimal(10,2)");
        });
    }
}
