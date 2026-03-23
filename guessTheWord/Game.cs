using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guessTheWord
{
    internal class Game
    {

        string[] wordStorage = { "зима", "три", "источник", "пенал" };

        public Game()
        {
            Random random = new Random();
            while (true)
            {

                int i = random.Next(wordStorage.Length);
                
                string wordText = wordStorage[i];

                Word word = new Word(wordText);

                Run run = new Run(word);
            }
        }
    }
}
