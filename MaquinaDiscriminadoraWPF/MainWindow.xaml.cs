using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace MaquinaDiscriminadoraWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        string[] reserved = new string[] { "box","open","flip","keep","label","fold","otherwise"};
        string[] operators = new string[] { "+", "-", "*", "/", "AND", "OR", "and", "or", "==", "<", ">", ">=", "<="};
        string[] agrupadores = new string[] { "(", "[", "{", ")", "]", "}" };
        bool notResFlag, notOpFlag, notGrFlag, notDecFlag, notTerFlag;
        string declarativo = "<-";
        Microsoft.Win32.OpenFileDialog file;
        List<string> palabras = new List<string>();
        List<int> tokens = new List<int>();
        StreamReader reader;
        string fileName;
        string boxesCode;
        MaquinaSintactica ms;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void btn_LoadFile_Click(object sender, RoutedEventArgs e)
        {
            input_Block.Text = "";
            output_Box.Text = "";
            file = new Microsoft.Win32.OpenFileDialog();
            file.ShowDialog();
            fileName = file.FileName;
            if (fileName != null) { reader = new StreamReader(fileName); }
            input_Block.Text = reader.ReadToEnd();
            
            //MessageBox.Show(fileName); /*Uncomment for debug purposes.*/
        }

        private void btn_Translate_Click(object sender, RoutedEventArgs e)
        {
            string line;
            reader = new StreamReader(fileName);
            int lineCount = 0;

            while ((line = reader.ReadLine()) != null)
            {
                translate(line);
                lineCount++;
            }

            notResFlag = false;
            notOpFlag = false;
            notTerFlag = false;
            notGrFlag = false;
            notDecFlag = false;
            /*
            line = reader.ReadLine();
            MessageBox.Show(line);
            LINES USED FOR DEBUGGING
            */
            MessageBox.Show("Número de líneas: " + lineCount);
            ms = new MaquinaSintactica(tokens, palabras, lineCount);
        }

        private void translate(string line)
        {
            string translation = "";
            StringBuilder word = new StringBuilder();

            for (int i = 0; i < line.Length - 1; i++)
            {
                word.Append(line[i]);
                if (line[i + 1] == ' ' || line[i + 1] == '\n' || i == line.Length-1)
                {
                    //revisa si es palabra reservada                 
                    foreach (string w in reserved)
                    {
                        if (word.ToString() == w)
                        {
                            translation = "[3, reservada]";
                            output_Box.Text += translation;
                            tokens.Add(3);
                            palabras.Add(word.ToString());
                            translation = String.Empty;
                            word.Clear();
                        }
                        else
                        {
                            notResFlag = true;
                        }
                    }
                    //revisa si es operador
                    foreach (string o in operators)
                    {
                        if (word.ToString() == o)
                        {
                            translation = "[4, operador]";
                            output_Box.Text += translation;
                            tokens.Add(4);
                            palabras.Add(word.ToString());
                            translation = String.Empty;
                            word.Clear();
                        }
                        else
                        {
                            notOpFlag = true;
                        }
                    }
                    //revisa por agrupadores
                    foreach (string a in agrupadores)
                    {
                        if (word.ToString() == a)
                        {
                            translation = "[2, agrupador]";
                            output_Box.Text += translation;
                            tokens.Add(2);
                            palabras.Add(word.ToString());
                            translation = String.Empty;
                            word.Clear();
                        }
                        else
                        {
                            notGrFlag = true;
                        }
                    }
                    //revisa si es declarativo
                    if (word.ToString() == declarativo)
                    {
                        translation = "[1, declarativo]";
                        output_Box.Text += translation;
                        tokens.Add(1);
                        palabras.Add(word.ToString());
                        translation = String.Empty;
                        word.Clear();
                    }
                    else
                    {
                        notDecFlag = true;
                    }
                    if (line[i] == '&')
                    {
                        translation = "[7, terminador]";
                        output_Box.Text += translation;
                        tokens.Add(7);
                        palabras.Add(word.ToString());
                        translation = String.Empty;
                        word.Clear();
                    }
                    else
                    {
                        notTerFlag = true;
                    }
                    
                }//if statement for space terminator
                else if (line[i] == ' ' || line[i] == '#')
                {
                    translation = "[6, omision]";
                    output_Box.Text += translation;
                    tokens.Add(6);
                    palabras.Add(word.ToString());
                    translation = String.Empty;
                    word.Clear();
                }
                else if(notDecFlag && notGrFlag && notTerFlag && notOpFlag && notResFlag)
                {
                    translation = "[5, identificador]";
                    output_Box.Text += translation;
                    tokens.Add(5);
                    palabras.Add(word.ToString());
                    translation = String.Empty;
                    word.Clear();
                }
            }
        }
    }
}
