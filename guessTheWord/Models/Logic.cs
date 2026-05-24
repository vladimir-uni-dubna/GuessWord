using System;

namespace guessTheWord
{
    internal class Logic
    {
        private Game game;

        public Game CurrentGame => game;

        public Logic()
        {
            game = null;
        }

        public Game StartNewGame()
        {
            game = new Game();
            return game;
        }

        public bool GuessLetter(char letter)
        {
            if (game == null || game.IsOver)
                return false;

            letter = char.ToLower(letter);

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

            if (game.AllRevealed())
                game.Win();

            return found;
        }

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
