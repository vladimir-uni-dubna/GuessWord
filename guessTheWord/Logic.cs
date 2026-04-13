using System;
using System.Collections.Generic;

namespace guessTheWord
{
    /// <summary>
    /// Логика — правила игры, проверка букв, ходы
    /// </summary>
    internal class Logic
    {
        private Game game;

        public Game CurrentGame { get => game; }

        public Logic()
        {
            game = null;
        }

        /// <summary>
        /// Начать новую игру
        /// </summary>
        public Game StartNewGame()
        {
            game = new Game();
            return game;
        }

        /// <summary>
        /// Попробовать угадать букву. Возвращает true если буква есть в слове.
        /// </summary>
        public bool GuessLetter(char letter)
        {
            if (game == null || game.IsOver)
                return false;

            letter = char.ToLower(letter);

            // Уже пробовали эту букву
            if (game.TriedLetters.Contains(letter))
                return false;

            game.TriedLetters.Add(letter);

            bool found = false;
            for (int i = 0; i < game.Word.Length; i++)
            {
                if (game.Word[i] == letter)
                {
                    game.RevealAt(i);
                    found = true;
                }
            }

            if (!found)
                game.LoseAttempt();

            // Проверяем победу
            if (game.AllRevealed())
                game.Win();

            return found;
        }

        /// <summary>
        /// Попробовать угадать слово целиком
        /// </summary>
        public bool GuessWord(string guess)
        {
            if (game == null || game.IsOver)
                return false;

            if (guess.ToLower() == game.Word.ToLower())
            {
                game.Win();
                return true;
            }
            else
            {
                game.LoseAttempt();
                return false;
            }
        }

        /// <summary>
        /// Получить подсказку — открыть первую скрытую букву (стоит 1 попытку)
        /// </summary>
        public char Hint()
        {
            if (game == null || game.IsOver)
                return '\0';

            for (int i = 0; i < game.Revealed.Length; i++)
            {
                if (game.Revealed[i] == '_')
                {
                    game.RevealAt(i);
                    game.LoseAttempt();

                    if (game.AllRevealed())
                        game.Win();

                    return game.Word[i];
                }
            }
            return '\0';
        }
    }
}
