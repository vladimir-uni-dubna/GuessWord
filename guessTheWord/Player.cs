using System;
using System.Collections.Generic;
using System.Linq;

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

        public void AddGame(Game game)
        {
            games.Add(game);
        }

        public int GamesPlayed()
        {
            return games.Count;
        }

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

    /// <summary>
    /// База игроков — хранит логины и пароли в коде
    /// </summary>
    internal static class PlayerDatabase
    {
        // Зарегистрированные игроки: логин → пароль
        private static Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "admin", "admin123" },
            { "игрок1", "пароль1" },
            { "гость", "гость" }
        };

        /// <summary>
        /// Проверить логин и пароль. Возвращает true если совпадают.
        /// </summary>
        public static bool Validate(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;

            if (!users.ContainsKey(login.ToLower()))
                return false;

            return users[login.ToLower()] == password;
        }

        /// <summary>
        /// Проверить, существует ли логин
        /// </summary>
        public static bool Exists(string login)
        {
            return !string.IsNullOrEmpty(login) && users.ContainsKey(login.ToLower());
        }

        /// <summary>
        /// Зарегистрировать нового игрока. Возвращает true если успешно.
        /// </summary>
        public static bool Register(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;

            if (login.Length < 2)
                return false;

            if (password.Length < 3)
                return false;

            if (users.ContainsKey(login.ToLower()))
                return false;

            users[login.ToLower()] = password;
            return true;
        }

        /// <summary>
        /// Получить список всех логинов
        /// </summary>
        public static string[] GetAllLogins()
        {
            return users.Keys.ToArray();
        }
    }
}
