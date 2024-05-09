using System;
using System.Collections.Generic;
using System.Linq;

namespace bestelbus
{
    internal class Program
    {
        static void Main()
        {
            string nrString = Console.ReadLine();
            if (nrString == "")
            {
                Console.WriteLine("0");
                return;
            }
            string[] nr = nrString.Split(" ");
            long n = long.Parse(nr[0]);
            long r = long.Parse(nr[1]);
            if(r == 0 || n == 0)
            {
                Console.WriteLine("0");
                return;
            }

            List<long> gewichtList = new List<long>();
            string line;
            while (gewichtList.Count < n)
            {
                line = Console.ReadLine();
                if(line == "")
                    break;

                string[] parts = line.Split(" ");
                foreach(string s in parts)
                {
                    bool success = long.TryParse(s, out long number);
                    if (success)
                        gewichtList.Add(number);
                    else
                        gewichtList.Add(0);
                }
            }

            long[] gewicht = gewichtList.Where(getal => getal != 0).ToArray();
            if(gewicht.Length == 0)
            {
                Console.WriteLine("0");
                return;
            }

            long highest = 0;
            long max = 0;
            foreach (long g in gewicht)
            {
                if(g > highest)
                {
                    highest = g;
                }
                max += g;
            }

            long min = 1;
            long endSize = 1;

            while (min <= max)
            {
                long busSize = (min + max) / 2;
                long temp = 0;
                long count = 0;

                foreach (long getal in gewicht)
                {
                    if (temp + getal <= busSize)
                    {
                        temp += getal;
                    }
                    else
                    {
                        temp = getal;
                        count++;
                    }
                }

                if (count < r)
                {
                    endSize = busSize;
                    max = busSize - 1;
                }
                else
                {
                    endSize = busSize;
                    min = busSize + 1;
                }
            }
            if (endSize < highest)
                Console.WriteLine(highest);
            else
                Console.WriteLine(endSize);
        }
    }
}
