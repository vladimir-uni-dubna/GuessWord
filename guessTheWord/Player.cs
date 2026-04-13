using System;
using System.Collections.Generic;

namespace guessTheWord
{
    /// <summary>
    /// Игрок — хранит логин, пароль и историю игр
    /// </summary>
    internal class Player
    {
        private string login;
        private string password;
        private List<Game> games;

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public List<Game> Games { get => games; }

        public Player(string login, string password)
        {
            this.login = login;
            this.password = password;
            this.games = new List<Game>();
        }

        /// <summary>
        /// Добавить игру в историю
        /// </summary>
        public void AddGame(Game game)
        {
            games.Add(game);
        }

        /// <summary>
        /// Количество сыгранных игр
        /// </summary>
        public int GamesPlayed()
        {
            return games.Count;
        }

        /// <summary>
        /// Количество выигранных игр
        /// </summary>
        public int GamesWon()
        {
            int won = 0;
            foreach (var g in games)
            {
                if (g.IsWon) won++;
            }
            return won;
        }

        public override string ToString()
        {
            return $"{login} (игр: {GamesPlayed()}, побед: {GamesWon()})";
        }
    }
}
