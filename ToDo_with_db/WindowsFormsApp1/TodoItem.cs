using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class TodoItem : Control
    {
        SqliteConnection connection;
        DatabaseEntry entry;
        TextBox name = new TextBox();
        TextBox description = new TextBox();
        Button moveLeft = new Button();
        Button moveRight = new Button();
        Button save = new Button();
        int priority;
        int state;

        public TodoItem(DatabaseEntry entry, int state, SqliteConnection connection)
        {
            this.connection = connection;

            this.entry = entry;
            this.name.Text = entry.name;
            this.description.Text = entry.description;
            this.priority = entry.priority;
            this.state = state;

            InitializeItem();
        }

        private void InitializeItem()
        {
            moveLeft.Size = new Size(25, 25);
            moveLeft.Location = new Point(60, 65);
            moveLeft.Text = "<-";
            moveLeft.Click += new EventHandler(this.MoveLeft_Click);

            moveRight.Size = moveLeft.Size;
            moveRight.Location = new Point(moveLeft.Left + 50, moveLeft.Top);
            moveRight.Text = "->";
            moveRight.Click += new EventHandler(this.MoveRight_Click);

            if (state > 1) { this.Controls.Add(moveLeft); }
            if (state < 3) { this.Controls.Add(moveRight); }

            save.Size = new Size(50, 25);
            save.Location = new Point(moveRight.Left + 35, moveRight.Top);
            save.Text = "save";

            name.Size = new Size(100, 20);
            name.Font = new Font(name.Font.FontFamily, name.Font.Size + 10);
            name.Location = new Point(50, 0);

            description.Location = new Point(5, 40);
            description.Size = new Size(190, 50);

            this.Controls.Add(this.name);
            this.Controls.Add(this.description);
            this.Controls.Add(save);
            save.Click += new EventHandler(this.Save_Click);
        }

        private void Save_Click(object o, EventArgs e)
        {
            entry.name = name.Text;
            entry.description = description.Text;
            entry.priority = this.priority;
            // entry.state = this.state;

            try
            {
                string sql = "UPDATE todo SET name = @name, description = @description, priority = @priority WHERE id = @id";
                connection.Open();
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@name", entry.name);
                command.Parameters.AddWithValue("@description", entry.description);
                command.Parameters.AddWithValue("@priority", entry.priority);
                command.Parameters.AddWithValue("@id", entry.id);

                int amountUpdated = command.ExecuteNonQuery();
                Console.WriteLine($"Updated {amountUpdated} lines.");
            }
            catch(SqliteException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        private void MoveLeft_Click(object o, EventArgs e)
        {
            this.state--;
        }

        private void MoveRight_Click(object o, EventArgs e) 
        {
            this.state++;
        }
    }
}
