using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Teleprogramma
{
    public partial class Form1 : Form
    {
        string log = "123";
        string pass = "123";
        SqlConnection sqlConnect = null;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            sqlConnect = new SqlConnection(@"Data Source=DESKTOP-8T9EGI7\SQLEXPRESS;Initial Catalog=" + "Teleproga" + ";Integrated Security=True");
            await sqlConnect.OpenAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == log && textBox2.Text == pass)
            {
                Programma form = new Programma(this.sqlConnect, this);
                this.Hide();
                form.Show();
            }
            else
            {
                if ((textBox1.Text == "" && textBox2.Text == "") || (textBox1.Text == " " && textBox2.Text == " "))
                {
                    MessageBox.Show("Заполнены не все поля. Пожалуйста заполните все и повторите попытку");
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль, убедитесь в написании и повторите попытку");
                }
            }
        }
    }
}
