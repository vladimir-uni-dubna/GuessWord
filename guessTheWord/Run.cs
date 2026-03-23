using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace guessTheWord
{
    internal class Run
    {

        TextBox[] textForms;

        public Run(Word word) 
        {
            TextBox[] textForms = new TextBox[word.Length()];

            int width = 10;
            int height = 10;

            int x = width / word.Length();
            int y = height / word.Length();

            for (int i = 0; i < textForms.Length; i++)
            {
                textForms[i] = new TextBox();
                textForms[i].Location = new Point(x*i, y*i);

            }

        }
    }
}
