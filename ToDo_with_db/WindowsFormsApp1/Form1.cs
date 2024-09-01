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
        List<DatabaseEntry> databaseItems = new List<DatabaseEntry>();
        List<TodoItem> todoItems = new List<TodoItem>();
        SqliteConnection connection = new SqliteConnection();
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
            this.Controls.Clear();
        }

        private void GetItemsFromDatabase()
        {
            try { connection = new SqliteConnection("Data Source=E:\\LocalRepo\\ToDo_with_db\\WindowsFormsApp1\\todo.db"); }
            catch { }
            try { connection = new SqliteConnection("Data Source=C:\\LocalRepo\\ToDo_with_db\\WindowsFormsApp1\\todo.db"); }
            catch { }
            try
            {
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
                        databaseItems.Add(new DatabaseEntry(id, name, description, priority));
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DontInsertItemsIntoForm()
        {
            for(int i = 0; i < databaseItems.Count; i++)
            {
                TextBox nameBox = new TextBox();
                nameBox.Location = new Point(20, 100 + 30 * i);
                nameBox.Size = new Size(100, 10);
                nameBox.Text = databaseItems[i].name;
                this.Controls.Add(nameBox);

                TextBox descriptionBox = new TextBox();
                descriptionBox.Location = new Point(125, 100 + 30 * i);
                descriptionBox.Size = new Size(300, 10);
                descriptionBox.Text = databaseItems[i].description;
                this.Controls.Add(descriptionBox);
            }
        }

        private void InsertItemsIntoForm()
        {
            for(int i = 0; i < databaseItems.Count; i++)
            {
                DatabaseEntry curr = databaseItems[i];
                TodoItem ti = new TodoItem(curr, 1, connection);
                ti.Location = new Point(20, 50 + 120 * i);
                ti.Size = new Size(200, 100);
                todoItems.Add(ti);
                this.Controls.Add(ti);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            using(Pen selPen = new Pen(Color.Blue, 3))
            {
                for(int i = 0; i < todoItems.Count; i++)
                {
                    TodoItem currentItem = todoItems[i];
                    g.DrawRectangle(selPen, currentItem.Location.X, currentItem.Location.Y, currentItem.Width, currentItem.Height);
                }
            }
        }
    }
}
