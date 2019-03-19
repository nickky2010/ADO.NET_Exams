using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_1
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlConnectionStringBuilder builder;
        SqlCommand selectCommand;
        public Form1()
        {
            InitializeComponent();
            string dir = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName;
            string attachDBFilename = dir + @"\DataSolution\Dogs.mdf";

            builder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\mssqllocaldb",
                InitialCatalog = "Dogs",
                AttachDBFilename = attachDBFilename,
                IntegratedSecurity = true
            };
            selectCommand = new SqlCommand();
            buttonShowAll_Click(new object(), new EventArgs());
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            using (connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    selectCommand.Connection = connection;
                    dataGridView1.Rows.Clear();
                    dataGridView1.ColumnCount = 2;
                    dataGridView1.Columns[0].HeaderText = "Breed";
                    dataGridView1.Columns[1].HeaderText = "Count";
                    selectCommand.CommandText = "select b.Breed, count(b.Breed) from Dogs d left join Breeds b on d.BreedId=b.Id where b.Breed = @br group by b.Breed";
                    selectCommand.Parameters.AddWithValue("@br", textBox1.Text);
                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            dataGridView1.Rows.Add(dataReader[0], dataReader[1]);
                        }
                    }
                    selectCommand.Parameters.RemoveAt("@br");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            using (connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    selectCommand.Connection = connection;
                    dataGridView1.ColumnCount = 3;
                    dataGridView1.Columns[0].HeaderText = "Nickname";
                    dataGridView1.Columns[1].HeaderText = "Breed";
                    dataGridView1.Columns[2].HeaderText = "Height";
                    selectCommand.CommandText = "select d.Nickname, b.Breed, d.Height from Dogs d left join Breeds b on d.BreedId=b.Id";
                    using (SqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            dataGridView1.Rows.Add(dataReader[0], dataReader[1], dataReader[2]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private async void buttonFindDogHeightMoreThen30_Click(object sender, EventArgs e)
        {
            using (connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    selectCommand.Connection = connection;
                    dataGridView1.ColumnCount = 3;
                    dataGridView1.Columns[0].HeaderText = "Nickname";
                    dataGridView1.Columns[1].HeaderText = "Breed";
                    dataGridView1.Columns[2].HeaderText = "Height";
                    selectCommand.CommandText = "select d.Nickname, b.Breed, d.Height from Dogs d left join Breeds b on d.BreedId=b.Id where d.Height>30";
                    Task<SqlDataReader> task = selectCommand.ExecuteReaderAsync();
                    using (SqlDataReader dataReader = await task)
                    {
                        dataGridView1.Rows.Clear();
                        while (await dataReader.ReadAsync())
                        {
                            dataGridView1.Rows.Add(dataReader[0], dataReader[1], dataReader[2]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }
    }
}
