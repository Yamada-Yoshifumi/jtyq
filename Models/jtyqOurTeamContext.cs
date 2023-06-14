using MySqlConnector;
using System;
using Microsoft.EntityFrameworkCore;
namespace jtyq.Models;

public class jtyqContext: DbContext
{
    public MySqlConnection Connection{get;}
    public jtyqContext(string connectionString)
    {
        Connection = new MySqlConnection(connectionString);
    }
    public override void Dispose() => Connection.Dispose();
    public DbSet<OurTeam> OurTeam { get; set; } = null!;
    public DbSet<UserProfilePicture> UserProfilePicture { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OurTeam>()
            .HasMany(e => e.ProfilePicture)
            .WithOne(e => e.OurTeam)
            .HasForeignKey(e => e.OurTeamId)
            .IsRequired(false);
    }
}

