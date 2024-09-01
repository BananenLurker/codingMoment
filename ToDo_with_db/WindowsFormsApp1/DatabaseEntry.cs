using System;

namespace WindowsFormsApp1
{
    public class DatabaseEntry
    {
        public int id;
        public string name;
        public string description;
        public int priority;

        public DatabaseEntry(int id, string name, string description, int priority)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.priority = priority;
        }
    }
}
