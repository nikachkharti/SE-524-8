using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportLeague.DTOS
{
    public class PlayerFromTeamDTO
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
