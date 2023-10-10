using System;


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

static void isPriemGetal()
{
    int x = 9;
    int test;
    int test2;
    for (int i = 2; i <= x; i++)
    {
        test = x / i;
        test2 = i * test;
        if (test2 == x)
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