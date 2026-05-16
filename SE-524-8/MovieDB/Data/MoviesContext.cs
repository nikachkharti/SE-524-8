using Microsoft.EntityFrameworkCore;
using MovieDB.Entities;

namespace MovieDB.Data
{
    public class MoviesContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Studio> Studios { get; set; }


        // -----------------------------------------------------------------------
        // Connection string — points at the MDF via SQL Express LocalDB.
        // Adjust the AttachDbFilename path to wherever you place Movies.mdf.
        // -----------------------------------------------------------------------
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Actor ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("Actor");
                entity.HasKey(e => e.ActorId);
                entity.Property(e => e.ActorId).HasColumnName("ActorID").ValueGeneratedNever();
                entity.Property(e => e.FirstName).HasColumnType("nvarchar(4000)").IsRequired();
                entity.Property(e => e.FamilyName).HasColumnType("nvarchar(4000)").IsRequired();
                entity.Property(e => e.DoB).HasColumnName("DoB");
                entity.Property(e => e.DoD).HasColumnName("DoD");
                entity.Property(e => e.Gender).HasColumnType("nvarchar(4000)");
            });

            // ── Director ───────────────────────────────────────────────────────
            modelBuilder.Entity<Director>(entity =>
            {
                entity.ToTable("Director");
                entity.HasKey(e => e.DirectorId);
                entity.Property(e => e.DirectorId).HasColumnName("DirectorID").ValueGeneratedNever();
                entity.Property(e => e.FirstName).HasColumnType("nvarchar(4000)").IsRequired();
                entity.Property(e => e.FamilyName).HasColumnType("nvarchar(4000)").IsRequired();
                entity.Property(e => e.DoB).HasColumnName("DoB");
                entity.Property(e => e.DoD).HasColumnName("DoD");
                entity.Property(e => e.Gender).HasColumnType("nvarchar(4000)");
            });

            // ── Genre ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");
                entity.HasKey(e => e.GenreId);
                entity.Property(e => e.GenreId).HasColumnName("GenreID").ValueGeneratedNever();
                entity.Property(e => e.GenreName).HasColumnName("Genre").HasColumnType("nvarchar(4000)").IsRequired();
            });

            // ── Language ───────────────────────────────────────────────────────
            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");
                entity.HasKey(e => e.LanguageId);
                entity.Property(e => e.LanguageId).HasColumnName("LanguageID").ValueGeneratedNever();
                entity.Property(e => e.LanguageName).HasColumnName("Language").HasColumnType("nvarchar(4000)").IsRequired();
            });

            // ── Certificate ────────────────────────────────────────────────────
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");
                entity.HasKey(e => e.CertificateId);
                entity.Property(e => e.CertificateId).HasColumnName("CertificateID").ValueGeneratedNever();
                entity.Property(e => e.CertificateName).HasColumnName("Certificate").HasColumnType("nvarchar(4000)").IsRequired();
            });

            // ── Country ────────────────────────────────────────────────────────
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");
                entity.HasKey(e => e.CountryId);
                entity.Property(e => e.CountryId).HasColumnName("CountryID").ValueGeneratedNever();
                entity.Property(e => e.CountryName).HasColumnName("Country").HasColumnType("nvarchar(4000)").IsRequired();
            });

            // ── Studio ─────────────────────────────────────────────────────────
            modelBuilder.Entity<Studio>(entity =>
            {
                entity.ToTable("Studio");
                entity.HasKey(e => e.StudioId);
                entity.Property(e => e.StudioId).HasColumnName("StudioID").ValueGeneratedNever();
                entity.Property(e => e.StudioName).HasColumnName("Studio").HasColumnType("nvarchar(4000)").IsRequired();
            });

            // ── Film ───────────────────────────────────────────────────────────
            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("Film");
                entity.HasKey(e => e.FilmId);
                entity.Property(e => e.FilmId).HasColumnName("FilmID").ValueGeneratedNever();
                entity.Property(e => e.Title).HasColumnType("nvarchar(4000)").IsRequired();
                entity.Property(e => e.ReleaseDate).HasColumnName("ReleaseDate");
                entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
                entity.Property(e => e.StudioId).HasColumnName("StudioID");
                entity.Property(e => e.Review).HasColumnType("nvarchar(4000)");
                entity.Property(e => e.CountryId).HasColumnName("CountryID");
                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
                entity.Property(e => e.GenreId).HasColumnName("GenreID");
                entity.Property(e => e.RunTimeMinutes).HasColumnName("RunTimeMinutes");
                entity.Property(e => e.CertificateId).HasColumnName("CertificateID");
                entity.Property(e => e.BudgetDollars).HasColumnName("BudgetDollars");
                entity.Property(e => e.BoxOfficeDollars).HasColumnName("BoxOfficeDollars");
                entity.Property(e => e.OscarNominations).HasColumnName("OscarNominations");
                entity.Property(e => e.OscarWins).HasColumnName("OscarWins");

                // Foreign keys
                entity.HasOne(f => f.Director)
                      .WithMany(d => d.Films)
                      .HasForeignKey(f => f.DirectorId)
                      .HasConstraintName("FK_Film_Director");

                entity.HasOne(f => f.Studio)
                      .WithMany(s => s.Films)
                      .HasForeignKey(f => f.StudioId)
                      .HasConstraintName("FK_Film_Studio");

                entity.HasOne(f => f.Country)
                      .WithMany(c => c.Films)
                      .HasForeignKey(f => f.CountryId)
                      .HasConstraintName("FK_Film_Country");

                entity.HasOne(f => f.Language)
                      .WithMany(l => l.Films)
                      .HasForeignKey(f => f.LanguageId)
                      .HasConstraintName("FK_Film_Language");

                entity.HasOne(f => f.Genre)
                      .WithMany(g => g.Films)
                      .HasForeignKey(f => f.GenreId)
                      .HasConstraintName("FK_Film_Genre");

                entity.HasOne(f => f.Certificate)
                      .WithMany(c => c.Films)
                      .HasForeignKey(f => f.CertificateId)
                      .HasConstraintName("FK_Film_Certificate");
            });

            // ── Role ───────────────────────────────────────────────────────────
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleId).HasColumnName("RoleID").ValueGeneratedNever();
                entity.Property(e => e.RoleName).HasColumnName("Role").HasColumnType("nvarchar(4000)").IsRequired();
                entity.Property(e => e.FilmId).HasColumnName("FilmID");
                entity.Property(e => e.ActorId).HasColumnName("ActorID");

                entity.HasOne(r => r.Film)
                      .WithMany(f => f.Roles)
                      .HasForeignKey(r => r.FilmId)
                      .HasConstraintName("FK_Role_Film");

                entity.HasOne(r => r.Actor)
                      .WithMany(a => a.Roles)
                      .HasForeignKey(r => r.ActorId)
                      .HasConstraintName("FK_Role_Actor");
            });
        }
    }
}
