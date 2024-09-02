using System;
using System.Collections.Generic;

namespace ufo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] part = Console.ReadLine().Split(' ');
            int c = int.Parse(part[0]);
            int u = int.Parse(part[1]);

            List<Commissie> coms = new List<Commissie>();
            List<Ufo> ufos = new List<Ufo>();

            for (int i = 0; i < c; i++)
            {
                string[] input = Console.ReadLine().Split(' ');
                Commissie com = new Commissie(input[0], input[1]);
                coms.Add(com);
            }
            for(int n = 0; n < u; n++)
            {
                string[] input = Console.ReadLine().Split(' ');
                Ufo ufo = new Ufo(long.Parse(input[0]), input[1], input[2], int.Parse(input[3]));
                ufos.Add(ufo);
            }

            foreach(Ufo ufo1 in ufos)
            {
                foreach(Commissie com1 in coms)
                {
                    if(ufo1.ophaal == com1.locatie && (com1.id % ufo1.eersteOphaal == ufo1.sleutel))
                    {
                        com1.locatie = ufo1.bestemming;
                        com1.afstand += ufo1.afstand;
                    }
                }
            }
            foreach(Commissie com in coms)
            {
                Console.WriteLine(com.afstand);
            }
        }
    }

    public class Commissie
    {
        public long id;
        public string locatie;
        public int afstand = 0;

        public Commissie(string id, string locatie)
        {
            this.id = 1;
            for(int i = 0; i< id.Length; i++)
            {
                int index = char.ToUpper(id[i]) - 64;
                this.id *= index;
            }

            this.locatie = locatie;
        }
    }

    public class Ufo
    {
        public long sleutel;
        public long eersteOphaal;
        public string ophaal;
        public string bestemming;
        public int afstand;

        public Ufo(long sleutel, string ophaal, string bestemming, int afstand)
        {
            this.sleutel = sleutel;
            this.ophaal = ophaal;
            this.bestemming = bestemming;
            this.afstand = afstand;

            this.eersteOphaal = 1;
            for (int i = 0; i < ophaal.Length; i++)
            {
                int index = char.ToUpper(ophaal[i]) - 64;
                this.eersteOphaal *= index;
            }
        }
    }
}