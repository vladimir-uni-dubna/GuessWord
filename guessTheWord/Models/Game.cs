using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace guessTheWord
{
    [DataContract]
    internal class Game
    {
        private static readonly Random rng = new Random();

        private static readonly string[] wordStorage = {
            "зима", "источник", "пенал", "молоток",
            "клавиатура", "монитор", "программа", "калькулятор",
            "велосипед", "путешествие", "шоколад", "библиотека"
        };

        [DataMember]
        public string Word { get; set; }

        [DataMember]
        public int AttemptsLeft { get; set; }

        [DataMember]
        public bool IsWon { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public List<char> TriedLetters { get; set; }

        [IgnoreDataMember]
        public char[] Revealed { get; set; }

        [IgnoreDataMember]
        public bool IsOver => IsWon || AttemptsLeft <= 0;

        [IgnoreDataMember]
        public string Result => IsWon ? "Победа" : "Поражение";

        [IgnoreDataMember]
        public int AttemptsUsed => 6 - AttemptsLeft;

        public Game()
        {
            Word = wordStorage[rng.Next(wordStorage.Length)];
            Revealed = new char[Word.Length];
            for (int i = 0; i < Word.Length; i++)
                Revealed[i] = '_';
            AttemptsLeft = 6;
            TriedLetters = new List<char>();
            IsWon = false;
            Date = DateTime.Now;
        }

        public void RevealAt(int index)
        {
            if (index >= 0 && index < Word.Length)
                Revealed[index] = Word[index];
        }

        public bool AllRevealed()
        {
            foreach (char c in Revealed)
                if (c == '_') return false;
            return true;
        }

        public string GetDisplayWord()
        {
            return new string(Revealed);
        }

        public void LoseAttempt()
        {
            AttemptsLeft--;
        }

        public void Win()
        {
            IsWon = true;
            for (int i = 0; i < Word.Length; i++)
                Revealed[i] = Word[i];
        }
    }
}
