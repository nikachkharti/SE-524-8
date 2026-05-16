using Microsoft.EntityFrameworkCore;
using SportLeague.Data;
using SportLeague.DTOS;
using SportLeague.Entities;

namespace SportLeague
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SportLeagueDbContext context = new SportLeagueDbContext();
        }

        private static PlayerHeightDTO HighestPlayer(SportLeagueDbContext context)
        {
            return context.PlayerProfiles.
                Include(p => p.Player)
                .OrderByDescending(p => p.Height_cm)
                .Select(p =>
                    new PlayerHeightDTO
                    {
                        PlayerId = p.PlayerId,
                        FullName = p.Player.FullName,
                        Height = p.Height_cm
                    })
                .FirstOrDefault();
        }
        private static List<PlayerFromTeamDTO> PlayersOrderedByName(SportLeagueDbContext context)
        {

            var playersWithNames = context.Players
            .OrderBy(p => p.FullName)

            .Select(p => new PlayerFromTeamDTO { PlayerId = p.PlayerId, FullName = p.FullName, Position = p.Position });

            return playersWithNames.ToList();
        }
        private static List<PlayerWithTeamNameDTO> PlayerWithTeamName(SportLeagueDbContext context)
        {
            var a = context.Players.Include(p => p.Team).Select(p => new PlayerWithTeamNameDTO
            {
                PlayerId = p.PlayerId,
                FullName = p.FullName,
                Position = p.Position,
                TeamName = p.Team.TeamName
            }).ToList();
            return a;
        }
        private static IQueryable<PlayerFromTeamDTO> GetPlayersOfTeam(SportLeagueDbContext context)
        {
            return context.Players
                .Where(x => x.TeamId == 1)
                .Select(p => new PlayerFromTeamDTO { PlayerId = p.PlayerId, FullName = p.FullName, Position = p.Position }
            );
        }
        private static IQueryable<TeamNameWithCityDto> GetTeamNameAndCity(SportLeagueDbContext context)
        {
            return context.Teams.Select(t => new TeamNameWithCityDto { TeamName = t.TeamName, City = t.City });
        }


    }
}
