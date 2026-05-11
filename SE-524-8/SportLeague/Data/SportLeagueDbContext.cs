using Microsoft.EntityFrameworkCore;
using SportLeague.Entities;

namespace SportLeague.Data
{
    public class SportLeagueDbContext : DbContext
    {
        private const string _connectionString = @"Server=DESKTOP-SCSHELD\SQLEXPRESS;Database=SportLeagueDB;Trusted_Connection=True;TrustServerCertificate=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<PlayerProfile> PlayerProfiles => Set<PlayerProfile>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<PlayerGame> PlayerGames => Set<PlayerGame>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //-----------------------------------------
            // TEAM
            //-----------------------------------------
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(x => x.TeamId);

                entity.Property(x => x.TeamName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.City)
                    .HasMaxLength(100);
            });

            //-----------------------------------------
            // PLAYER (1:M Team -> Players)
            //-----------------------------------------
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(x => x.PlayerId);

                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Position)
                    .HasMaxLength(50);

                entity.HasOne(x => x.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(x => x.TeamId);
            });

            //-----------------------------------------
            // PLAYER PROFILE (1:1)
            //-----------------------------------------
            modelBuilder.Entity<PlayerProfile>(entity =>
            {
                entity.HasKey(x => x.ProfileId);

                entity.Property(x => x.Height_cm)
                    .HasColumnType("decimal(5,2)");

                entity.Property(x => x.Weight_kg)
                    .HasColumnType("decimal(5,2)");

                entity.Property(x => x.Nationality)
                    .HasMaxLength(80);

                entity.HasIndex(x => x.PlayerId)
                    .IsUnique();

                entity.HasOne(x => x.Player)
                    .WithOne(p => p.PlayerProfile)
                    .HasForeignKey<PlayerProfile>(x => x.PlayerId);
            });

            //-----------------------------------------
            // GAME
            //-----------------------------------------
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(x => x.GameId);

                entity.HasOne(x => x.HomeTeam)
                    .WithMany(t => t.HomeGames)
                    .HasForeignKey(x => x.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AwayTeam)
                    .WithMany(t => t.AwayGames)
                    .HasForeignKey(x => x.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //-----------------------------------------
            // PLAYER GAME (M:M)
            //-----------------------------------------
            modelBuilder.Entity<PlayerGame>(entity =>
            {
                entity.HasKey(x => new
                {
                    x.PlayerId,
                    x.GameId
                });

                entity.Property(x => x.GoalsScored)
                    .HasDefaultValue(0);

                entity.HasOne(x => x.Player)
                    .WithMany(p => p.PlayerGames)
                    .HasForeignKey(x => x.PlayerId);

                entity.HasOne(x => x.Game)
                    .WithMany(g => g.PlayerGames)
                    .HasForeignKey(x => x.GameId);
            });

            //-----------------------------------------
            // DATA SEED
            //-----------------------------------------

            // Teams
            modelBuilder.Entity<Team>().HasData(
                new Team { TeamId = 1, TeamName = "Real Madrid", City = "Madrid", Founded = 1902 },
                new Team { TeamId = 2, TeamName = "Barcelona", City = "Barcelona", Founded = 1899 },
                new Team { TeamId = 3, TeamName = "Manchester City", City = "Manchester", Founded = 1880 }
            );

            // Players
            modelBuilder.Entity<Player>().HasData(
                new Player { PlayerId = 1, FullName = "Vinicius Junior", Position = "Forward", TeamId = 1 },
                new Player { PlayerId = 2, FullName = "Jude Bellingham", Position = "Midfielder", TeamId = 1 },

                new Player { PlayerId = 3, FullName = "Robert Lewandowski", Position = "Forward", TeamId = 2 },
                new Player { PlayerId = 4, FullName = "Pedri", Position = "Midfielder", TeamId = 2 },

                new Player { PlayerId = 5, FullName = "Erling Haaland", Position = "Forward", TeamId = 3 },
                new Player { PlayerId = 6, FullName = "Kevin De Bruyne", Position = "Midfielder", TeamId = 3 }
            );

            // Profiles
            modelBuilder.Entity<PlayerProfile>().HasData(
                new PlayerProfile { ProfileId = 1, PlayerId = 1, Height_cm = 176, Weight_kg = 73, Nationality = "Brazil" },
                new PlayerProfile { ProfileId = 2, PlayerId = 2, Height_cm = 186, Weight_kg = 75, Nationality = "England" },
                new PlayerProfile { ProfileId = 3, PlayerId = 3, Height_cm = 185, Weight_kg = 80, Nationality = "Poland" },
                new PlayerProfile { ProfileId = 4, PlayerId = 4, Height_cm = 174, Weight_kg = 70, Nationality = "Spain" },
                new PlayerProfile { ProfileId = 5, PlayerId = 5, Height_cm = 194, Weight_kg = 88, Nationality = "Norway" },
                new PlayerProfile { ProfileId = 6, PlayerId = 6, Height_cm = 181, Weight_kg = 76, Nationality = "Belgium" }
            );

            // Games
            modelBuilder.Entity<Game>().HasData(
                new Game { GameId = 1, GameDate = new DateTime(2026, 5, 1), HomeTeamId = 1, AwayTeamId = 2, Score = "2-1" },
                new Game { GameId = 2, GameDate = new DateTime(2026, 5, 2), HomeTeamId = 2, AwayTeamId = 3, Score = "1-3" },
                new Game { GameId = 3, GameDate = new DateTime(2026, 5, 3), HomeTeamId = 3, AwayTeamId = 1, Score = "2-2" },
                new Game { GameId = 4, GameDate = new DateTime(2026, 5, 4), HomeTeamId = 1, AwayTeamId = 3, Score = "1-0" }
            );

            // PlayerGame
            modelBuilder.Entity<PlayerGame>().HasData(
                new PlayerGame { PlayerId = 1, GameId = 1, GoalsScored = 1, MinutesPlayed = 90 },
                new PlayerGame { PlayerId = 2, GameId = 1, GoalsScored = 1, MinutesPlayed = 90 },

                new PlayerGame { PlayerId = 3, GameId = 2, GoalsScored = 1, MinutesPlayed = 90 },
                new PlayerGame { PlayerId = 4, GameId = 2, GoalsScored = 0, MinutesPlayed = 85 },

                new PlayerGame { PlayerId = 5, GameId = 3, GoalsScored = 2, MinutesPlayed = 90 },
                new PlayerGame { PlayerId = 6, GameId = 3, GoalsScored = 0, MinutesPlayed = 90 },

                new PlayerGame { PlayerId = 1, GameId = 4, GoalsScored = 1, MinutesPlayed = 90 },
                new PlayerGame { PlayerId = 5, GameId = 4, GoalsScored = 0, MinutesPlayed = 90 }
            );
        }
    }
}