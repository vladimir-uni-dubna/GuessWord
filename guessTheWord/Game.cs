using System;
using System.Collections.Generic;

namespace guessTheWord
{
    /// <summary>
    /// Игра — хранит загаданное слово, его состояние и результат
    /// </summary>
    internal class Game
    {
        private static readonly Random rng = new Random();

        private static readonly string[] wordStorage = {
            "зима", "источник", "пенал", "молоток",
            "клавиатура", "монитор", "программа", "калькулятор",
            "велосипед", "путешествие", "шоколад", "библиотека"
        };

        private string word;           // загаданное слово
        private char[] revealed;       // открытые буквы ( '_' = скрытая )
        private int attemptsLeft;      // оставшиеся попытки
        private List<char> triedLetters; // уже названные буквы
        private bool isWon;

        public string Word { get => word; }
        public char[] Revealed { get => revealed; }
        public int AttemptsLeft { get => attemptsLeft; }
        public List<char> TriedLetters { get => triedLetters; }
        public bool IsWon { get => isWon; }
        public bool IsOver { get => isWon || attemptsLeft <= 0; }

        public Game()
        {
            // Выбираем случайное слово
            word = wordStorage[rng.Next(wordStorage.Length)];
            revealed = new char[word.Length];
            for (int i = 0; i < word.Length; i++)
                revealed[i] = '_';

            attemptsLeft = 6; // как в виселице — 6 ошибок
            triedLetters = new List<char>();
            isWon = false;
        }

        /// <summary>
        /// Открыть букву в слове по индексу
        /// </summary>
        public void RevealAt(int index)
        {
            if (index >= 0 && index < word.Length)
                revealed[index] = word[index];
        }

        /// <summary>
        /// Проверить, открыты ли все буквы
        /// </summary>
        public bool AllRevealed()
        {
            foreach (char c in revealed)
                if (c == '_') return false;
            return true;
        }

        /// <summary>
        /// Текущее состояние слова для отображения
        /// </summary>
        public string GetDisplayWord()
        {
            return new string(revealed);
        }

        /// <summary>
        /// Уменьшить количество попыток
        /// </summary>
        public void LoseAttempt()
        {
            attemptsLeft--;
        }

        /// <summary>
        /// Отметить победу
        /// </summary>
        public void Win()
        {
            isWon = true;
            // Раскрываем всё слово
            for (int i = 0; i < word.Length; i++)
                revealed[i] = word[i];
        }
    }
}
