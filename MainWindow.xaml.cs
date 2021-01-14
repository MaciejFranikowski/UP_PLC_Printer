using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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



namespace UP_PLC_printer
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonPrintImage_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.CheckPathExists)
            {
                using (var stream = File.Create("obrazek"))
                {
                    string code = "\x1B" + "*p400x400Y";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // Offset
                    code = "\x1B" + "*t100R";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // Resolution
                    code = "\x1B" + "*r0F";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // Presentation
                    code = "\x1B" + "*r400T";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // Height
                    code = "\x1B" + "*r400S";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // Width
                    code = "\x1B" + "*r1A";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // Start
                    //Bitmap bitmap = new Bitmap(dialog.FileName);
                    //ImageSourceConverter ic = new ImageSourceConverter();
                    //var a = (byte[])ic.ConvertTo(bitmap, typeof(byte[]));
                    var b = File.ReadAllBytes(dialog.FileName);
                    stream.Write(b, 0, b.Length);
                    code = "\x1B" + "*rC";
                    stream.Write(code.Select(x => (byte)x).ToArray(), 0, code.ToArray().Length); // End
                    stream.Close();
                }

                //File.Copy("file", "LPT3");
            }

        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxPrint.Text))
            {
                var stream = File.Create("tekstowy.txt");
                string marginTop = "\x1B" + "&l" + Int32.Parse(textBoxMargin.Text) + "E";
                stream.Write(marginTop.Select(x => (byte)x).ToArray(), 0, marginTop.ToArray().Length);

                if ((bool)checkBoxBold.IsChecked)
                {
                    string bold = "\x1B" + "(s3B";
                    stream.Write(bold.Select(x => (byte)x).ToArray(), 0, bold.ToArray().Length);
                }
                if ((bool)checkBoxUnderline.IsChecked)
                {
                    string underline = "\x1B" + "&d0D";
                    stream.Write(underline.Select(x => (byte)x).ToArray(), 0, underline.ToArray().Length);
                }
                if ((bool)checkBoxItalic.IsChecked)
                {
                    string italic = "\x1B" + "(s1S";
                    stream.Write(italic.Select(x => (byte)x).ToArray(), 0, italic.ToArray().Length);
                }

                string font = "";

                if (comboBoxFont.SelectedItem != null || comboBoxFont.SelectedItem == "")
                {
                    if ((string)((ComboBoxItem)comboBoxFont.SelectedItem).Content == "Arial")
                        font = "218";
                    else if ((string)((ComboBoxItem)comboBoxFont.SelectedItem).Content == "TimesNewRoman")
                        font = "517";
                    else if ((string)((ComboBoxItem)comboBoxFont.SelectedItem).Content == "CG")
                        font = "4101";
                }

                if (comboBoxSize.SelectedItem != null || comboBoxSize.SelectedItem == "")
                {
                    string look = "\x1B" + "(s1p" + (string)((ComboBoxItem)comboBoxSize.SelectedItem).Content + "v0s0b" + font + "T";
                    stream.Write(look.Select(x => (byte)x).ToArray(), 0, look.ToArray().Length);
                }

                stream.Write(textBoxPrint.Text.Select(x => (byte)x).ToArray(), 0, textBoxPrint.Text.ToArray().Length);
                string end = "\x1B" + "E";
                stream.Write(end.Select(x => (byte)x).ToArray(), 0, end.ToArray().Length);
                stream.Close();
                //File.Copy("file.txt", "LPT3");
                
            }
        }
    }
}
