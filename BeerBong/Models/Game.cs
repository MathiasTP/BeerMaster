using System.Collections.Generic;

namespace BeerBong.Models
{
    public class Game
    {
        public int gameId { get; set; }
        public List<Player> players { get; set; }
    }
}