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
using System.Data;

namespace Teleprogramma
{
    public partial class Programma : Form
    {
        

        BindingSource source1 = new BindingSource();
        SqlConnection sqlConnect;
        DataTable dataTable = new DataTable();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter;
        SqlCommandBuilder sqlBuilder = null;
        DataGridViewRow selectedRow = null;
        DataSet dataSet = null;
        Form mainForm = null;
        Sqlcon sql = new Sqlcon();
        public Programma(SqlConnection sqlConnect, Form f)
        {
            this.sqlConnect = sqlConnect;
            mainForm = f;
            InitializeComponent();
        }
        public class SelectedData
        {
            public string id { get; set; }
            public string title { get; set; }
        }


        private async void Programma_Load(object sender, EventArgs e)
        {
            cmd.Connection = sqlConnect;
            source1.DataSource = dataTable;
            advancedDataGridView1.DataSource = source1;
            await UpdateInfo();

            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;


            load();

        }
        public async Task UpdateInfo()
        {


            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;

            List<SelectedData> janr_data = new List<SelectedData>();
            List<SelectedData> janr_data2 = new List<SelectedData>();
            List<SelectedData> kanal_data = new List<SelectedData>();
            List<SelectedData> kanal_data2 = new List<SelectedData>();




           
            var tov = await sql.CommnadWithQuery("SELECT id, [Название жанра] as [Название жанра] from Жанры");
            var tov_a = tov.Select();
            for (int i = 0; i < tov_a.Length; i++)
            {
                janr_data.Add(new SelectedData() { id = tov_a[i]["id"].ToString(), title = tov_a[i]["Название жанра"].ToString() });
            }
            var tov2 = await sql.CommnadWithQuery("SELECT id, [Название жанра] as [Название жанра] from Жанры");
            var tov_a2 = tov.Select();
            for (int i = 0; i < tov_a.Length; i++)
            {
                janr_data2.Add(new SelectedData() { id = tov_a2[i]["id"].ToString(), title = tov_a2[i]["Название жанра"].ToString() });
            }


            var kat = await sql.CommnadWithQuery("SELECT id, [Название канала] as [Название канала] from Каналы");
            var kat_a = kat.Select();
            for (int i = 0; i < kat_a.Length; i++)
            {
                kanal_data.Add(new SelectedData() { id = kat_a[i]["id"].ToString(), title = kat_a[i]["Название канала"].ToString() });
            }

            var kat2 = await sql.CommnadWithQuery("SELECT id, [Название канала] as [Название канала] from Каналы");
            var kat_a2 = kat2.Select();
            for (int i = 0; i < kat_a2.Length; i++)
            {
                kanal_data2.Add(new SelectedData() { id = kat_a2[i]["id"].ToString(), title = kat_a2[i]["Название канала"].ToString() });
            }


            comboBox3.DataSource = kanal_data2;
            comboBox3.DisplayMember = "title";
            comboBox3.ValueMember = "id";
            comboBox3.SelectedIndex = -1;

            comboBox4.DataSource = janr_data2;
            comboBox4.DisplayMember = "title";
            comboBox4.ValueMember = "id";
            comboBox4.SelectedIndex = -1;





        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Kanali form = new Kanali(this.sqlConnect, this);
            this.Hide();
            form.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Janr form = new Janr(this.sqlConnect, this);
            this.Hide();
            form.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" &&comboBox4.Text != "" && textBox2.Text != "" &&comboBox3.Text != "" )
            {
                cmd.CommandText = "INSERT INTO [Программа] ([Название канала],[Жанр],[Дата],[Время начала],[Время окончания]) values ('" + comboBox3.Text + "','" +comboBox4.Text+ "','"+dateTimePicker1.Value+ "','" + textBox1.Text+ "','"+textBox2.Text+ "')";
                await cmd.ExecuteNonQueryAsync();
                load();
                textBox1.Text = "";
                textBox2.Text = "";
                dateTimePicker1.Value = Convert.ToDateTime("01,01,1999");
                comboBox3.SelectedIndex= -1;
                comboBox4.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }
        void load()
        {

            if (this.dataTable != null)
            {
                
                string C = "SELECT id,[Название канала] as [Название канала],[Жанр] as [Жанр],[Дата] as [Дата],[Время начала] as [Время начала],[Время окончания] as [Время окончания] from Программа";
                adapter = new SqlDataAdapter(C, this.sqlConnect);
                this.dataTable.Clear();
                adapter.Fill(dataTable);
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                
                advancedDataGridView1.Columns[0].Visible = false;

             
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(selectedRow.Cells[0].Value);
            cmd.CommandText = "delete from [Программа] where id='" + id + "'";
            await cmd.ExecuteNonQueryAsync();
            load();

            cls();
        }
        void cls()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox4.Text = "";
            comboBox3.Text = "";
            dateTimePicker1.Value = Convert.ToDateTime("01,01,1999");
            button4.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
            button5.Enabled = false;
            advancedDataGridView1.ClearSelection();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(selectedRow.Cells[0].Value);

            cmd.CommandText = "Update [Программа] set [Название канала]='" + comboBox3.Text + "',[Жанр]=  '" + comboBox4.Text + "',[Дата]=  '" + dateTimePicker1.Value + "',[Время начала]=  '" + textBox1.Text + "',[Время окончания]=  '" + textBox2.Text+"' where id = '" + id + "'";
            await cmd.ExecuteNonQueryAsync();

            load();

            textBox1.Text = "";
            textBox2.Text = "";
            comboBox4.Text = "";
            comboBox3.Text = "";
            dateTimePicker1.Value = Convert.ToDateTime("01,01,1999");
            button4.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
            button5.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cls();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            UpdateInfo();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void advancedDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                selectedRow = advancedDataGridView1.Rows[e.RowIndex];
                comboBox3.Text = selectedRow.Cells[1].Value.ToString();
                comboBox4.Text = selectedRow.Cells[2].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells[3].Value.ToString());
                textBox1.Text = selectedRow.Cells[4].Value.ToString();
                textBox2.Text = selectedRow.Cells[5].Value.ToString();
                button4.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button5.Enabled = true;
            }
        }

      
    }
}
