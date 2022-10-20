using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Activated(object sender, EventArgs e)
        {
            label1.Text = " Администратор может добавлять удалять вопросы и смотреть результаты студентов";
            label2.Text = "Студент может читать теорию,  на вкладке демонстрация студент может изучить и понять как работает очередь, а на вкладке тестирование он может проверить свои знания";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Администратор")
            {
                textBox1.Visible = true;
                button1.Text = "Войти";
                textBox1.Focus();
            }else if (button1.Text == "Войти")
            {
                if (textBox1.Text == "123")
                {
                    Admin admin = new Admin();
                    admin.Show();
                    textBox1.Clear();
                    textBox1.Visible = false;
                    button1.Text = "Администратор";
                }
                else
                {
                    MessageBox.Show("Неправильный пароль");
                    textBox1.Clear();
                    textBox1.Focus();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
