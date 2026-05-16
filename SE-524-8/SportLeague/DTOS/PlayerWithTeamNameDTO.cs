using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportLeague.DTOS
{
    public class PlayerWithTeamNameDTO : PlayerFromTeamDTO
    {
        public string TeamName { get; set; }
    }
}
