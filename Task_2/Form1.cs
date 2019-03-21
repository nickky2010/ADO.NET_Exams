using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Task_2
{
    public partial class Form1 : Form
    {
        DataSet dataSet;
        SqlDataAdapter adapter;
        SqlConnection connection;
        string connectionString;
        public Form1()
        {
            InitializeComponent();
            dataSet = new DataSet();
            connectionString = ConfigurationManager.ConnectionStrings["Dogs"].ConnectionString;            // from app.config
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("select d.Id, d.Nickname, b.Breed, d.Height from Dogs d left join Breeds b on d.BreedId=b.Id", connection);
            adapter.Fill(dataSet);
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["Id"] };
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = dataSet.Tables[0].Rows.Find(Convert.ToInt32(textBox1.Text));
                row.Delete();
                SqlCommand delete = new SqlCommand();
                delete.Connection = adapter.SelectCommand.Connection;
                delete.CommandText = "delete from Dogs where Id=@id";
                delete.Parameters.Add("@id", SqlDbType.Int);
                delete.Parameters["@id"].SourceColumn = "Id";
                adapter.DeleteCommand = delete;
                adapter.Update(dataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            dataSet.Clear();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }
    }
}
