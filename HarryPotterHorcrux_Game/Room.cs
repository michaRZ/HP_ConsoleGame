using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPotter_Game
{
    public class Room
    {
        // CONSTRUCTOR
        public Room(string description, List<string> exits, List<DeathlyHallow> deathlyHallows)
        {
            Description = description;
            Exits = exits;
            DeathlyHallows = deathlyHallows;
        }


        public string Description { get; } 
        public List<string> Exits { get; }
        public List<DeathlyHallow> DeathlyHallows { get; }
        public void TakeHallow(DeathlyHallow hallow)
        {
            if (DeathlyHallows.Contains(hallow))
            {
                DeathlyHallows.Remove(hallow);
            }
        }
    }
}
