using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary.Models
{
    //0 = enpty, 1 = ship, 2 = miss, 3 = hit, 4 = sunk
   public enum GridSpotStatus
    {
        Empty,
        Ship,
        Miss,
        Hit,
        Sunk
    }

}
