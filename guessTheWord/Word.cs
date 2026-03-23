using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guessTheWord
{
    internal class Word
    {
        // поле с символами слова
        char[] wordSlice;

        // конструктор
        public Word(string word) {
            if (word == null || word.Length < 3) throw new ArgumentNullException("Word is required and more than 2 letters");
            for (int i = 0; i < word.Length; i++)
            {
                wordSlice[i] = word[i];
            }
        }

        // вывод слова
        public override string ToString()
        {
            string word = "";
            for (int i = 0; i < wordSlice.Length; i++)
            {
                word += Convert.ToString(wordSlice[i]);
            }
            return word;
        }

        public int Length()
        {
            return wordSlice.Length;
        }

        // метод угадывания слова по индексу и символу
        public bool Guess(int i, char ch)
        {
            if (wordSlice[i] == ch)
            {
                return true;
            }
            return false;
        }
    }
}
