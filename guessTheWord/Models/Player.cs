using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace guessTheWord
{
    [DataContract]
    internal class Player
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public List<Game> Games { get; set; }

        public Player()
        {
            Games = new List<Game>();
        }

        public Player(string login, string password) : this()
        {
            Login = login;
            Password = password;
        }

        public void AddGame(Game game)
        {
            Games.Add(game);
        }

        public int GamesPlayed => Games.Count;

        public int GamesWon => Games.Count(g => g.IsWon);

        public override string ToString()
        {
            return $"{Login} (игр: {GamesPlayed}, побед: {GamesWon})";
        }
    }
}
