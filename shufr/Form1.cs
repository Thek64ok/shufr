using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace shufr
{
    public partial class Form1 : Form
    {
        //нужно для возврата назад и вперед, в первую записываем результат, второе для контроля назад и вперед
        List<string> textFor = new List<string>();
        int iterator = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            changeFunction();
        }

        private void doOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            List<string> UniqSymbols = new List<string>();
            int sum = 0;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = "c:\\";
            openFile.Filter = "All files (*.*)|*.*";
            openFile.RestoreDirectory = true;

            if(openFile.ShowDialog() == DialogResult.OK)
            {

                var fileStream = openFile.OpenFile();

                StreamReader reader = new StreamReader(fileStream, Encoding.UTF8);

                fileContent = reader.ReadToEnd().ToLower();

                reader.Close();
                //нужно для получения уникальных букв
                foreach(var s in fileContent.Distinct())
                {
                    if (s.ToString() != "\r" && s.ToString() != "\n")
                    {
                        UniqSymbols.Add(s.ToString());
                    }
                }
                //подсчет количества букв без энтеров и переносов
                foreach (var s in fileContent)
                {
                    if (s.ToString() != "\r" && s.ToString() != "\n")
                        sum++;
                }
                //нужно для подсчета количества каждой из букв
                List<int> CountOfSymbols = new List<int>();
                foreach (var i in UniqSymbols)
                {
                    int count = 0;
                    foreach (var j in fileContent)
                    {
                        if (i == j.ToString() && "\r" != j.ToString())
                            count++;
                    }
                    CountOfSymbols.Add(count);
                }
                //создаем кнопки с преопределенным расположением
                RadioButton[] radio = new RadioButton[UniqSymbols.Count];
                int k = 1;
                int x = 6;
                for (int i = 0; i < UniqSymbols.Count; i++)
                {
                    float s = Convert.ToSingle(CountOfSymbols[i]) / sum;

                    radio[i] = new RadioButton();
                    radio[i].Name = "radio" + i.ToString();
                    radio[i].Text = UniqSymbols[i] + " - " + Math.Round(s, 4);
                    int y = 28 * k;
                    if (y > 448)
                    {
                        x = 147;
                        y = 28;
                        k = 1;
                    }
                    radio[i].Location = new Point(x, y);
                    radio[i].Size = new Size(120, 25);
                    //groupBox3.Controls.Add(radio);
                    k++;
                    //Controls.Add(radio);
                    //textFiels += UniqSymbols[i] + " - " + CountOfSymbols[i] + " - " + Math.Round(s,4) + Environment.NewLine;
                }
                //при повторном открытии нового файла старые кнопки нужно подчистить
                groupBox3.Controls.Clear();
                for (int i = 0; i < UniqSymbols.Count; i++)
                {
                    groupBox3.Controls.Add(radio[i]);
                }
                //помещаем содержимое файла в поле
                textOfUniq.Text = fileContent;
                //нулевой элемент "истории"
                textFor.Add(fileContent);

            }        

        }

        private void doClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void doBack_Click(object sender, EventArgs e)
        {
            iterator--;
            if (iterator >= 0)
            {
                textOfUniq.Text = textFor[iterator];
                updateRadio();
            }
            else
                iterator++;
        }

        private void doNext_Click(object sender, EventArgs e)
        {
            iterator++;
            if (iterator < textFor.Count && textFor.Count != 0)
            {
                textOfUniq.Text = textFor[iterator];
                updateRadio();
            }
            else
                iterator--;
        }

        //самописная, нужно для динамического обновления кнопок
        //практически ничем не отличается от функции doOpen_Click
        private void updateRadio()
        {
            List<string> UniqSymbols = new List<string>();
            int sum = 0;

            foreach (var s in textOfUniq.Text.Distinct())
            {
                if (s.ToString() != "\r" && s.ToString() != "\n")
                {
                    UniqSymbols.Add(s.ToString());
                }
            }

            foreach (var s in textOfUniq.Text)
            {
                if (s.ToString() != "\r" && s.ToString() != "\n")
                    sum++;
            }

            List<int> CountOfSymbols = new List<int>();
            foreach (var i in UniqSymbols)
            {
                int count = 0;
                foreach (var j in textOfUniq.Text)
                {
                    if (i == j.ToString() && "\r" != j.ToString())
                        count++;
                }
                CountOfSymbols.Add(count);
            }

            RadioButton[] radio = new RadioButton[UniqSymbols.Count];
            int k = 1;
            int x = 6;
            for (int i = 0; i < UniqSymbols.Count; i++)
            {
                float s = Convert.ToSingle(CountOfSymbols[i]) / sum;

                radio[i] = new RadioButton();
                radio[i].Name = "radio" + i.ToString();
                radio[i].Text = UniqSymbols[i] + " - " + Math.Round(s, 4);
                int y = 28 * k;
                if (y > 448)
                {
                    x = 147;
                    y = 28;
                    k = 1;
                }
                radio[i].Location = new Point(x, y);
                radio[i].Size = new Size(120, 25);
                //groupBox3.Controls.Add(radio);
                k++;
                //Controls.Add(radio);
                //textFiels += UniqSymbols[i] + " - " + CountOfSymbols[i] + " - " + Math.Round(s,4) + Environment.NewLine;
            }

            groupBox3.Controls.Clear();
            for (int i = 0; i < UniqSymbols.Count; i++)
            {
                groupBox3.Controls.Add(radio[i]);
            }
        }

        //самописная функция для замены одной буквы на другие
        private void changeFunction()
        {
            string whatChange = string.Empty;
            string toChange = string.Empty;
            if (textOfUniq.Text == "")
            {
                MessageBox.Show("Не загружен текст", "Так дела не делаются");
                return;
            }
            foreach (RadioButton element in groupBox2.Controls)
            {
                if (element.Checked)
                    whatChange = element.Text.Substring(0, 1);
            }
            foreach (RadioButton element in groupBox3.Controls)
            {
                if (element.Checked)
                    toChange = element.Text.Substring(0, 1);
            }
            if (whatChange != "" && toChange != "")
            {
                if (whatChange == "-")
                    whatChange = " ";
                string textChange = textOfUniq.Text;
                textOfUniq.Text = textChange.Replace(toChange, whatChange);
                textFor.Add(textOfUniq.Text);
                iterator++;
                updateRadio();
            }
            else
            {
                MessageBox.Show("Что-то не указано.", "Что же это такое?");
            }
        }

        private void clearField_Click(object sender, EventArgs e)
        {
            textOfUniq.Clear();
            groupBox3.Controls.Clear();
        }

        //функции сделаны ради удобства и опыта создателя
        private void textOfUniq_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string pathToFile = string.Empty;
            //textOfUniq.Text = e.Data.GetData(DataFormats.FileDrop).ToString();
            if(files != null)         
                pathToFile = files[0];            
            else
            {
                MessageBox.Show("Файл нужно передвигать из своего ПК", "Исключение");
                return;
            }   
            if (pathToFile.EndsWith(".txt"))
            {
                StreamReader reader = new StreamReader(pathToFile, System.Text.Encoding.GetEncoding(1251));
                string fileContent = reader.ReadToEnd().ToLower();
                textOfUniq.Text = fileContent;
                updateRadio();
                reader.Close();
            }
        }

        private void textOfUniq_DragLeave(object sender, EventArgs e)
        {
            textOfUniq.Clear();
            groupBox3.Controls.Clear();
        }

        //стало скучно на основной работе, вот и решил немного убить время
        private void About_Click(object sender, EventArgs e)
        {
            string message = "Copyright OOO '1337', с момента рождения. Все права отобраны.\n";
            message += "Версия: 3.2.2 | Версия не проблема\n";
            message += "Режим: Тоталитарный\n";
            message += "Конфигурация: Дешифратор универсальный (только под Цезаря), редакция 4.20\n";
            message += "Copyright OOO '1337', 1939-1945. Все права ЗАЩИЩЕННЫ.";
            MessageBox.Show(message, "О программе");
        }

        private void replaceLetter_Click(object sender, EventArgs e)
        {
            changeFunction();
        }
    }
}