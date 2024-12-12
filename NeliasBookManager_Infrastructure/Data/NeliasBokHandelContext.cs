using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeliasBookManager.Domain.ModelsDb;

namespace NeliasBookManager.Infrastructure.Data;

public partial class NeliasBokHandelContext : DbContext
{
    public NeliasBokHandelContext()
    {
    }

    public NeliasBokHandelContext(DbContextOptions<NeliasBokHandelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BokFormat> BokFormats { get; set; }

    public virtual DbSet<BokFörlag> BokFörlags { get; set; }

    public virtual DbSet<Butiker> Butikers { get; set; }

    public virtual DbSet<Böcker> Böckers { get; set; }

    public virtual DbSet<Författare> Författares { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldos { get; set; }

    public virtual DbSet<Språk> Språks { get; set; }

 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       var config = new ConfigurationBuilder().AddUserSecrets<NeliasBokHandelContext>().Build();
        var connectionString = config["ConnectionString"];
         optionsBuilder.UseSqlServer("Initial Catalog=NeliasBokHandel;Integrated Security=True;Trust Server Certificate=True;Server SPN=localhost");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BokFormat>(entity =>
        {
            entity.HasKey(e => e.FormatId).HasName("PK_Format");

            entity.ToTable("BokFormat");

            entity.Property(e => e.FormatId)
                .ValueGeneratedNever()
                .HasColumnName("FormatID");
            entity.Property(e => e.Format).HasMaxLength(20);
        });

        modelBuilder.Entity<BokFörlag>(entity =>
        {
            entity.HasKey(e => e.FörlagId);

            entity.ToTable("BokFörlag");

            entity.Property(e => e.Adress).HasMaxLength(50);
            entity.Property(e => e.BokFörlagNamn).HasMaxLength(50);
            entity.Property(e => e.Postkod).HasMaxLength(20);
            entity.Property(e => e.Stad).HasMaxLength(20);
        });

        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.ButikId);

            entity.ToTable("Butiker");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Adress).HasMaxLength(50);
            entity.Property(e => e.ButikNamn).HasMaxLength(50);
            entity.Property(e => e.Postkod).HasMaxLength(20);
            entity.Property(e => e.Postort).HasMaxLength(20);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK_Böcker_1");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.FormatId).HasColumnName("FormatID");
            entity.Property(e => e.FörlagId).HasColumnName("FörlagID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.SpråkId).HasColumnName("SpråkID");
            entity.Property(e => e.Titel).HasMaxLength(50);

            entity.HasOne(d => d.Format).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Böcker_Format");

            entity.HasOne(d => d.Förlag).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörlagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Böcker_BokFörlag1");

            entity.HasOne(d => d.Genre).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Böcker_Genre1");

            entity.HasOne(d => d.Språk).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.SpråkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Böcker_Språk");

            entity.HasMany(d => d.Författars).WithMany(p => p.Isbns)
                .UsingEntity<Dictionary<string, object>>(
                    "FörfattareTillBok",
                    r => r.HasOne<Författare>().WithMany()
                        .HasForeignKey("FörfattarId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FörfattareTillBok_Författare"),
                    l => l.HasOne<Böcker>().WithMany()
                        .HasForeignKey("Isbn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FörfattareTillBok_Böcker1"),
                    j =>
                    {
                        j.HasKey("Isbn", "FörfattarId");
                        j.ToTable("FörfattareTillBok");
                        j.IndexerProperty<string>("Isbn")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("ISBN");
                        j.IndexerProperty<int>("FörfattarId").ValueGeneratedOnAdd();
                    });
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.FörfattarId);

            entity.ToTable("Författare");

            entity.Property(e => e.FörfattarId).HasColumnName("FörfattarID");
            entity.Property(e => e.Efternamn).HasMaxLength(20);
            entity.Property(e => e.Förnamn).HasMaxLength(20);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Genre1)
                .HasMaxLength(20)
                .HasColumnName("Genre");
        });

       
        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn });

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.ButikId).ValueGeneratedOnAdd();
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LagerSaldo_Butiker");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LagerSaldo_Böcker");
        });

        modelBuilder.Entity<Språk>(entity =>
        {
            entity.ToTable("Språk");

            entity.Property(e => e.SpråkId).HasColumnName("SpråkID");
            entity.Property(e => e.Språk1)
                .HasMaxLength(20)
                .HasColumnName("Språk");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
