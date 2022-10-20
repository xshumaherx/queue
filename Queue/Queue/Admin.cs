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
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using Queue;
using System.Xml.Serialization;
using System.Globalization;

namespace Queue
{

    public partial class Admin : Form
    {
        public bool MisnamedProperty
        {
            get
            {
                return true;
            }
        }
        string ans;
        public Admin()
        {
            InitializeComponent();
            loadQuestions();
        }
        
        public void loadQuestions()
        {
            QuestionsList.list.Clear();
            var xDoc = XDocument.Load("ques.xml");
            foreach (var item in xDoc.Element("ArrayOfProblem").Elements("Problem"))
            {
                QuestionsList.list.Add(new XmlData()
                {
                    question= item.Element("question").Value,
                    trueAnswer = item.Element("trueAnswer").Value,
                    answer1 = item.Element("answer1").Value,
                    answer2 = item.Element("answer2").Value,
                    answer3 = item.Element("answer3").Value,
                    answer4 = item.Element("answer4").Value
                });
            }
        }

        private void button3_Click(object sender, EventArgs e)//Выход
        {
              Close();
        }

        private void button2_Click(object sender, EventArgs e)//Удаление
        {
            if (dataGridView1.Visible == true)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(rowIndex);
                File.Delete("ques.xml");
                XmlSerializer formatter = new XmlSerializer(typeof(List<XmlData>));
                using (FileStream fs = new FileStream("ques.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, QuestionsList.list);
                }
            }
            else { MessageBox.Show("Пожалуйста, закройте таблицу результат студентов"); }
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            loadQuestions();
            var list = new BindingList<XmlData>(QuestionsList.list);
            dataGridView1.DataSource = list;
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            var list = new BindingList<XmlData>(QuestionsList.list);
            dataGridView1.DataSource = list;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && (Char.IsNumber(e.KeyChar)) | e.KeyChar == '\b') return;
            else
                errorProvider1.SetError(comboBox1, "Выберите только то что есть в списке");
            e.Handled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                errorProvider1.SetError(richTextBox1, "Не должны быть пустыми");
                richTextBox1.Focus();
                return;
            }
            if (textBox1.Text == "")
            {
                errorProvider1.SetError(textBox1, "Не должны быть пустыми");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text == "")
            {
                errorProvider1.SetError(textBox2, "Не должны быть пустыми");
                textBox2.Focus();
                return;
            }
            if (textBox3.Text == "")
            {
                errorProvider1.SetError(textBox3, "Не должны быть пустыми");
                textBox3.Focus();
                return;
            }
            if (textBox4.Text == "")
            {
                errorProvider1.SetError(textBox4, "Не должны быть пустыми");
                textBox4.Focus();
                return;
            }
            if (comboBox1.Text == "")
            {
                errorProvider1.SetError(comboBox1, "Не должны быть пустыми");
                comboBox1.Focus();
                return;
            }
            if (comboBox1.SelectedIndex == 0)
            {
                ans = "OptionA";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                ans = "OptionB";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                ans = "OptionC";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                ans = "OptionD";
            }
            else
            {
                MessageBox.Show("Выберите ответ");
            }
            //Добвление в лист. Конструктор в классе создан
            QuestionsList.list.Add(new XmlData(richTextBox1.Text, ans, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text));
            //Сериализация
            XmlSerializer formatter = new XmlSerializer(typeof(List<XmlData>));

            using (FileStream fs = new FileStream("ques.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, QuestionsList.list);
            }
            MessageBox.Show("Вопрос добавлен");
            richTextBox1.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Admin_KeyPress(object sender, KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dataGridView1.Visible = false;
                dataGridView2.Visible = true;
                dataGridView2.Rows.Clear();
                foreach (var line in File.ReadLines("TestFile.txt"))
                 {                       
                     var array = line.Split();
                     dataGridView2.Rows.Add(array);
                 }
            }
            else
            {
                dataGridView2.Visible = false;
                dataGridView1.Visible = true;
            }
        }
    }
}
