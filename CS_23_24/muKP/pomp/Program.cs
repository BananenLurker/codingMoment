using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main()
    {
        string[] part = Console.ReadLine().Split(" ");
        int x = int.Parse(part[0]);
        int y = int.Parse(part[1]);
        int z = int.Parse(part[2]);

        Node lowest = new Node(0, 0, 10);

        Dictionary<(int, int), Node> dict = new Dictionary<(int, int), Node>();
        for(int i = 0; i < y; i++)
        {
            string input = Console.ReadLine();
            for(int n = 0; n < x; n++)
            {
                string c = "" + input[n];
                Node node = new Node(n, i, int.Parse(c));
                if (node.value < lowest.value)
                {
                    lowest.value = node.value;
                    lowest.x = node.x;
                    lowest.y = node.y;
                }
                dict.Add((n, i), node);
            }
        }
        Console.WriteLine(BFS(lowest, z, dict));
    }

    public static int BFS(Node start, int height, Dictionary<(int, int), Node> dict)
    {
        int startX = start.x; int startY = start.y;
        int count = 0;
        if (start.value < height)
        {
            count += height - start.value;
        }
        else
        {
            return 0;
        }
        dict.Remove((startX, startY));
        if (dict.ContainsKey((startX, startY - 1)))
        {
            count += BFS(dict[(startX, startY -1)], height, dict);
        }
        if (dict.ContainsKey((startX - 1, startY)))
        {
            count += BFS(dict[(startX - 1, startY)], height, dict);
        }
        if (dict.ContainsKey((startX, startY + 1)))
        {
            count += BFS(dict[(startX, startY + 1)], height, dict);
        }
        if (dict.ContainsKey((startX + 1, startY)))
        {
            count += BFS(dict[(startX + 1, startY)], height, dict);
        }
        return count;
    }

    public class Node
    {
        public int value;
        public int x;
        public int y;

        public Node(int x, int y, int value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }
    }
}