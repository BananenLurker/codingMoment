using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<DatabaseEntry> items = new List<DatabaseEntry>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetItemsFromDatabase();
            InsertItemsIntoForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void GetItemsFromDatabase()
        {
            try
            {
                SqliteConnection connection = new SqliteConnection("Data Source=E:\\LocalRepo\\ToDo_with_db\\WindowsFormsApp1\\todo.db");
                connection.Open();

                string sql = "SELECT * FROM todo";
                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string description = reader.GetString(2);
                        int priority = reader.GetInt32(3);
                        items.Add(new DatabaseEntry(id, name, description, priority));
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertItemsIntoForm()
        {
            for(int i = 0; i < items.Count; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Location = new Point(20, 100 + 30 * i);
                textBox.Size = new Size(100, 10);
                this.Controls.Add(textBox);
            }
        }
    }
}
