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
using Task_3.Contexts;
using Task_3.Models;

namespace Task_3
{
    public partial class Form1 : Form
    {
        SqlConnectionStringBuilder builder;
        DogContext db;
        public Form1()
        {
            InitializeComponent();
            string dir = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName).FullName;
            string attachDBFilename = dir + @"\Task_1\DataSolution\Dogs.mdf";
            builder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\mssqllocaldb",
                InitialCatalog = "Dogs",
                AttachDBFilename = attachDBFilename,
                IntegratedSecurity = true
            };
            db = new DogContext(builder.ConnectionString);
            dataGridView1.DataSource = db.Dogs.Select(d => new { d.Nickname, Breed = d.Breed.BreedName, d.Height }).ToList();
            comboBox1.DataSource = db.Breeds.Select(b => b.BreedName).ToList();
            comboBox1.SelectedIndex = 0; 
            comboBox2.DataSource = db.Breeds.Select(b => b.BreedName).ToList();
            comboBox2.SelectedIndex = 0; 
            comboBox3.DataSource = db.Dogs.Select(b => b.Nickname).ToList();
            comboBox3.SelectedIndex = 0; 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = "Average height = "+ db.Dogs.Where(d=>d.Breed.BreedName==comboBox1.Text).Average(d=>d.Height).ToString("f2");
        }

        private void buttonLittleDog_Click(object sender, EventArgs e)
        {
            // terrible mockery of pupils))))))
            var little = db.Dogs.Include("Breed").ToList().Where(a=>a.Height<50).OrderByDescending(b=>b.Height).
                Select(d => new
                {
                    d.Breed.BreedName,
                    Nickname = d.Breed.Dogs.Select(u => u.Nickname).ToList().GetString(),
                    Height = d.Breed.Dogs.Select(u => u.Height.ToString()).ToList().GetString()
                }).Distinct().ToList();
            dataGridView1.DataSource = little;
        }

        private void buttonAddNewDog_Click(object sender, EventArgs e)
        {
            try
            {
                MyDog dog = new MyDog();
                if (textBox1.Text != "")
                    dog.Nickname = textBox1.Text;
                else
                    throw new Exception("Error! Empty field Nickname");
                if (textBox2.Text != "")
                    dog.Height = int.Parse(textBox2.Text);
                else
                    throw new Exception("Error! Wrong field height");
                Breed breed = db.Breeds.FirstOrDefault(a => a.BreedName == comboBox2.SelectedItem.ToString());
                if(breed!=null)
                    dog.BreedId = breed.Id;
                db.Dogs.Add(dog);
                db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonChangeHeight_Click(object sender, EventArgs e)
        {
            int height;
            if (textBox3.Text != "")
                height = int.Parse(textBox3.Text);
            else
                throw new Exception("Error! Wrong field height");
            MyDog dog = db.Dogs.FirstOrDefault(a => a.Nickname == comboBox3.SelectedItem.ToString());
            dog.Height = height;
            db.SaveChangesAsync();
        }

        private void buttonShowDb_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.Dogs.Select(d => new { d.Nickname, Breed = d.Breed.BreedName, d.Height }).ToList();
        }
    }
    static class ListExtention
    {
        // метод расширения
        public static string GetString(this List<string> list)
        {
            StringBuilder s = new StringBuilder();
            list.ForEach(u => { s.Append(u).Append(";"); });
            return s.ToString();
        }
    }

}
