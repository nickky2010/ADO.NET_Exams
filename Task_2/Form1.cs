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
            buttonShowAll_Click(new object(), new EventArgs());
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // all very easy: adapter.DeleteCommand.ExecuteNonQuery();
                connection.Open();
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.DeleteCommand = new SqlCommand("sp_deleteDog", connection);
                adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                adapter.DeleteCommand.Parameters.AddWithValue("@p", int.Parse(textBox1.Text));
                SqlParameter parameter = adapter.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                parameter.Direction = ParameterDirection.Output;
                adapter.DeleteCommand.ExecuteNonQuery();
                buttonShowAll_Click(new object(), new EventArgs());
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
