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
    public partial class Kanali : Form
    {
        BindingSource source1 = new BindingSource();
        SqlConnection sqlConnect;
        DataTable dataTable = new DataTable();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter;
        SqlCommandBuilder sqlBuilder=null;
        DataGridViewRow selectedRow = null;
        DataSet dataSet = null;
        Form mainForm = null;
        public Kanali(SqlConnection sqlConnect, Form f)
        {
            this.sqlConnect = sqlConnect;
            mainForm = f;
            InitializeComponent();
        }

        private void Kanali_Load(object sender, EventArgs e)
        {
           
            cmd.Connection = sqlConnect;
            source1.DataSource = dataTable;
            advancedDataGridView1.DataSource = source1;
            
           
            load();

        }
        void load()
        {

            if (this.dataTable != null)
            {
                string C = "SELECT id,[Название канала] as [Название канала] from Каналы";
                adapter = new SqlDataAdapter(C, this.sqlConnect);
                this.dataTable.Clear();
                adapter.Fill(dataTable);
                advancedDataGridView1.Columns[0].Visible = false;
            }
        }

        private void Kanali_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Закрыть форму ''Каналы?''", "Завершение программы", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {

                mainForm.Show();

            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            load();
           
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox1.Text != " ")
            {
                cmd.CommandText = "INSERT INTO [Каналы] ([Название канала]) values ('" + textBox1.Text + "')";
                await cmd.ExecuteNonQueryAsync();
                load();
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Заполните поле ввода");
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(selectedRow.Cells[0].Value);
            cmd.CommandText = "delete from [Каналы] where id='" + id + "'";
            await cmd.ExecuteNonQueryAsync();
            load();

            cls();
        }
        void cls()
        {
            textBox1.Text = "";
            selectedRow = null;
            button4.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button2.Enabled = true;
            advancedDataGridView1.ClearSelection();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cls();
        }

        private void advancedDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                selectedRow = advancedDataGridView1.Rows[e.RowIndex];
                textBox1.Text = selectedRow.Cells[1].Value.ToString();
                button4.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button5.Enabled = true;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(selectedRow.Cells[0].Value);

            cmd.CommandText = "Update [Каналы] set [Название канала]='" + textBox1.Text + "' where id = '" + id + "'";
            await cmd.ExecuteNonQueryAsync();

            load();

            textBox1.Text = "";
            button4.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
            button5.Enabled = false;
        }
    }
}
