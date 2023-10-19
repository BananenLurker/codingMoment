using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static void isEven()
{
    int x = 10;
    if (x % 2 == 0)
    {
        Console.WriteLine("Het is even");
    }
    else
    {
        Console.WriteLine("het is niet even");
    }
}

static void deelbaar()
{
    int x = 5;
    int y = 5;
    int test1 = x / y;
    int test2 = test1 * y;
    if (test2 == x)
    {
        Console.WriteLine("Het is deelbaar");
    }
    else
    {
        Console.WriteLine("het is niet deelbaar");
    }
}

static void kleinsteDeler()
{
    int test;
    int test2;
    int x = 59;
    for (int i = 2; i <= x; i++)
    {
        test = x / i;
        test2 = i * test;
        if (test2 == x)
        {
            Console.WriteLine($"het kleinst deelbare getal is {i}");
            return;
        }
    }
}

void isPriemGetal(int x)
{
    int test;
    for (int i = 2; i <= x; i++)
    {
        test = i * (x / i);
        if (test == x)
        {
            if (i == x)
            {
                Console.WriteLine("Het is wel een priemgetal");
            }
            else
            {
                Console.WriteLine("het is geen priemgetal");
                return;
            }
        }
    }
}

void VaakstVoorkomendeLengte(List<string> l)
{
    int[] teller = new int[51];
    foreach (string s in l)
    {
        teller[s.Length]++;
    }
    int x = 0; int y = 0; int aantal = 0;
    foreach (int i in teller)
    {
        aantal++;
        if (i > x)
        {
            x = i;
            y = aantal;
        }
    }
    Console.WriteLine($"Het eerste nummer is {teller[50]}");
    Console.WriteLine($"De string met {y - 1} letters komt het vaakst voor, namelijk {x} keer!");
}

List<string> oma = new List<string>();

void RandomString()
{
    for(int n = 0; n < 10; n++)
    {
        Random rnd = new Random();
        int num = rnd.Next(50, 50);
        Console.WriteLine(num);
        int length = num;

        StringBuilder build = new StringBuilder();
        Random random = new Random();

        char letter;

        for (int i = 0; i < length; i++)
        {
            double flt = random.NextDouble();
            int shift = Convert.ToInt32(Math.Floor(25 * flt));
            letter = Convert.ToChar(shift + 65);
            build.Append(letter);
        }
        oma.Add(build.ToString());
    }
    VaakstVoorkomendeLengte(oma);
}

Func<int, int> macht = x => x * x;

Action<string> groet = naam =>
{
    string groetjes = $"Hallo {naam}!";
    Console.WriteLine(groetjes);
};

isPriemGetal(7);