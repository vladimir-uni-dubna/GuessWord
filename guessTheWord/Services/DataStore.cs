using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace guessTheWord
{
    internal static class DataStore
    {
        private static readonly string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GuessWord");

        private static readonly string filePath = Path.Combine(folderPath, "players.json");

        public static List<Player> LoadPlayers()
        {
            try
            {
                if (!File.Exists(filePath))
                    return GetDefaultPlayers();

                using (var fs = File.OpenRead(filePath))
                {
                    var serializer = new DataContractJsonSerializer(typeof(List<Player>));
                    var players = (List<Player>)serializer.ReadObject(fs);
                    return players ?? GetDefaultPlayers();
                }
            }
            catch
            {
                return GetDefaultPlayers();
            }
        }

        public static void SavePlayers(List<Player> players)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                using (var fs = File.Create(filePath))
                {
                    var serializer = new DataContractJsonSerializer(typeof(List<Player>));
                    serializer.WriteObject(fs, players);
                }
            }
            catch
            {
            }
        }

        private static List<Player> GetDefaultPlayers()
        {
            return new List<Player>
            {
                new Player("admin", "admin123"),
                new Player("игрок1", "пароль1"),
                new Player("гость", "гость")
            };
        }

        public static bool Validate(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;

            var players = LoadPlayers();
            return players.Any(p =>
                p.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                p.Password == password);
        }

        public static bool Exists(string login)
        {
            if (string.IsNullOrEmpty(login))
                return false;

            var players = LoadPlayers();
            return players.Any(p =>
                p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        public static bool Register(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || login.Length < 2)
                return false;
            if (string.IsNullOrEmpty(password) || password.Length < 3)
                return false;

            var players = LoadPlayers();

            if (players.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                return false;

            players.Add(new Player(login, password));
            SavePlayers(players);
            return true;
        }

        public static Player GetPlayer(string login)
        {
            var players = LoadPlayers();
            return players.FirstOrDefault(p =>
                p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        public static void SavePlayer(Player player)
        {
            var players = LoadPlayers();
            var existing = players.FirstOrDefault(p =>
                p.Login.Equals(player.Login, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                var index = players.IndexOf(existing);
                players[index] = player;
            }
            else
            {
                players.Add(player);
            }
            SavePlayers(players);
        }
    }
}
