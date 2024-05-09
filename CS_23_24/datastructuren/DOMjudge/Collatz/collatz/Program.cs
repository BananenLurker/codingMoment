using System;
using System.Collections.Generic;
internal class Program
{
    private static void Main()
    {
        List<long> input = new List<long>();
        input.Add(long.Parse(Console.ReadLine()));
        input.Add(long.Parse(Console.ReadLine()));
        input.Add(long.Parse(Console.ReadLine()));
        input.Add(long.Parse(Console.ReadLine()));

        for (int i = 0; i < 4; i++)
        {
            long n = input[i];
            int k = 0;

            while (n != 1)
            {
                if (n % 2 == 0)
                {
                    n /= 2;
                }
                else
                {
                    n = 3 * n + 1;
                }
                k++;
            }
            Console.WriteLine(k);
        }
    }
}