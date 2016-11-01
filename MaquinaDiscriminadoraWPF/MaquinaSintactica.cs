using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace MaquinaDiscriminadoraWPF
{
    class MaquinaSintactica
    {
        public MaquinaSintactica(List<int> tokens,List<string> palabras, int lineCount)
        {
            List<List<string>> parseTrees = new List<List<string>>();
            for (int i = 0; i < tokens.Count - 1; i++)
            {
                if (i == 0)
                {
                    switch (tokens[i])//checks for identifier or reserved word
                    {
                        case 1:
                            continue;
                        case 3:
                            continue;
                        default:
                            MessageBox.Show("Error in line: "+lineCount);
                            break;
                    }

                }

                if (i != ' ')//checks for spaces
                {
                    MessageBox.Show("Error in line: "+lineCount);
                    break;
                }

            }//for loop bracket
        }
    }
}
