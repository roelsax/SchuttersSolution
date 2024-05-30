using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Schutters.Models;

public partial class SchuttersContext : DbContext
{
    public SchuttersContext()
    {
    }

    public SchuttersContext(DbContextOptions<SchuttersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Lid> Leden { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Schutters;Trusted_Connection=Yes;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Stamnummer);

            entity.Property(e => e.Stamnummer).ValueGeneratedNever();
            entity.Property(e => e.Adres).HasMaxLength(50);
            entity.Property(e => e.Gemeente).HasMaxLength(50);
            entity.Property(e => e.Naam).HasMaxLength(50);
            entity.Property(e => e.Postcode).HasMaxLength(4);
        });

        modelBuilder.Entity<Lid>(entity =>
        {
            entity.HasKey(e => e.Lidnummer);

            entity.Property(e => e.Lidnummer).ValueGeneratedNever();
            entity.Property(e => e.Geslacht).HasMaxLength(1);
            entity.Property(e => e.Naam).HasMaxLength(50);
            entity.Property(e => e.Niveau).HasMaxLength(1);
            entity.Property(e => e.Voornaam).HasMaxLength(50);
            entity.Property(e => e.ClubStamnummer).HasColumnName("Club");

            entity.HasOne(d => d.Club).WithMany(p => p.Leden)
                .HasForeignKey(d => d.ClubStamnummer)
                .HasConstraintName("Leden$ClubsLeden");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
