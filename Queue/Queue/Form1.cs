using System;
using System.Collections;
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
using System.Xml.Serialization;
using Queue;

namespace Queue
{
    public partial class Form1 : Form
    {
        static private int number = 0;
        static private int score = 0;
        static private string userAnswer;
        static private List<XmlData> randomList = new List<XmlData>();
        public Form1()
        {
            InitializeComponent();
            loadQuestions();
            loadText();
        }

        public void loadQuestions()
        {
            QuestionsList.list.Clear();
            var xDoc = XDocument.Load("ques.xml");
            foreach (var item in xDoc.Element("ArrayOfProblem").Elements("Problem"))
            {
                QuestionsList.list.Add(new XmlData() 
                {
                    question = item.Element("question").Value,
                    trueAnswer = item.Element("trueAnswer").Value,
                    answer1 = item.Element("answer1").Value,
                    answer2 = item.Element("answer2").Value,
                    answer3 = item.Element("answer3").Value,
                    answer4 = item.Element("answer4").Value
                });
            }
            Random rnd = new Random();
            randomList = QuestionsList.list.OrderBy(s => rnd.Next()).Take(5).ToList();
        }

        private void loadText()
        {
            richTextBox2.Text = randomList[number].question;
            radioButton1.Text = randomList[number].answer1;
            radioButton2.Text = randomList[number].answer2;
            radioButton3.Text = randomList[number].answer3;
            radioButton4.Text = randomList[number].answer4;
        }

        Queue<QQ> Q = new Queue<QQ>();
        QQ miqueueQQ = new QQ();

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label1.Visible = true;
                label3.Visible = true;
                label1.Text = Q.Count.ToString();
            }
            else
            {
                label1.Visible = false;
                label3.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("Очередь пуста!", "Ошибка!");
                }
                else
                {
                    label2.Visible = true;
                    label4.Visible = true;
                    label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
            }
            else
            {
                label2.Visible = false;
                label4.Visible = false;
            }       
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (textAdd.Text == "")
            {
                errorProvider1.SetError(textAdd, "Не должны быть пустыми");
                textAdd.Focus();
                return;
            }
            errorProvider1.SetError(textAdd, "");
            QQ miqueueQQ = new QQ();
            miqueueQQ.Ochered = textAdd.Text;
            Q.Enqueue(miqueueQQ);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Q.ToList();
            label1.Text = Q.Count.ToString();
            label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textAdd.Clear();
            textAdd.Focus();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (Q.Count != 0)
            {
                QQ miqueueQQ = new QQ();
                miqueueQQ = Q.Dequeue();
                textAdd.Text = miqueueQQ.Ochered;
                dataGridView1.DataSource = Q.ToList();
                label1.Text = Q.Count.ToString();
                int c = Q.Count;
                if (c == 0)
                {
                    label2.Text=("Очеред пуста");
                }
                else
                {
                    label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
                textAdd.Clear();
            }
        }

        private void textAdd_KeyPress_1(object sender, KeyPressEventArgs e)
        {
    if (!char.IsDigit(e.KeyChar) & (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                MessageBox.Show("Вы пытаетесь ввести другой символ! Пожалуйста, введите цифрy.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите фамилию");
            }
            else
            {
                if (File.Exists("TestFile.txt"))
                {
                    File.AppendAllText("TestFile.txt", "\r\n" + textBox1.Text + " " + score + " " + DateTime.Now.ToString());
                    MessageBox.Show("Данные записаны");
                }
                else
                {
                    StreamWriter file = new StreamWriter("TestFile.txt");
                    file.Write(textBox1.Text + " " + score + " " + DateTime.Now.ToString());
                    file.Close();
                    MessageBox.Show("Данные записаны");
                }
                textBox1.Clear();
                textBox1.Enabled = false;
                buttonConfirm.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAskAndContinue_Click(object sender, EventArgs e)
        {
            userAnswer = "";
           // int N = 5;
            if (radioButton1.Checked == true)
            {
                userAnswer = "OptionA";
            }
            else if (radioButton2.Checked == true)
            {
                userAnswer = "OptionB";
            }
            else if (radioButton3.Checked == true)
            {
                userAnswer = "OptionC";
            }
            else if (radioButton4.Checked == true)
            {
                userAnswer = "OptionD";
            }
            else
            {
                MessageBox.Show("Выберите ответ");
            }
            if (userAnswer != "")
            {
                if (randomList[number].trueAnswer == userAnswer)
                {
                    score += 1;//верный ответ
                    number += 1;//вопросы
                }
                else
                {
                    number += 1;//вопросы
                }

                if (randomList.Count == number)
                {
                    
                    MessageBox.Show("Тестирование окончено. Вы набрали " + score + " правильных ответа из 5" + " \nВведите фамилию для сохранения результата или начните сначала");
                    //MessageBox.Show((100 * score / N).ToString());
                    textBox1.Enabled = true;
                    buttonConfirm.Enabled = true;
                    buttonAskAndContinue.Enabled = false;
                    buttonrestartTest.Visible = true;
                    button3.Enabled = true;
                    number = 0;
                    textBox1.Enabled = true;
                    buttonConfirm.Enabled = true;
                }
                else
                {
                    loadText();
                }
            }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.LoadFile("Теория очереди.rtf");
            richTextBox1.Find("Text", RichTextBoxFinds.MatchCase);
            richTextBox1.SelectionFont = new Font("Verdana", 12, FontStyle.Bold);
        }

        private void buttonrestartTest_Click(object sender, EventArgs e)
        {
            score = 0;
            number = 0;
            loadQuestions();
            loadText();
            textBox1.Enabled = false;
            buttonConfirm.Enabled = false;
            buttonAskAndContinue.Enabled = true;
            button3.Enabled = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) | e.KeyChar == '\b' && e.KeyChar != (int)Keys.Space | e.KeyChar == '\0') return;
            else
                MessageBox.Show(textBox1, "Введите фамилию без пробела");
            e.Handled = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

