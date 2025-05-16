using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TypingGame
{
    internal abstract class GameType
    {
        public StreamReader Reader;
        public string ReaderLocation;
        private readonly Random rnd = new();
        public IEnumerable<string> Words = [];

        public GameType() { }

        public virtual string NextLine() 
            => Words.Skip((int)(rnd.NextDouble() * Words.Count())).Take(1).First();
    }

    internal class LoremGame : GameType
    {
        public LoremGame() : base() {
            Reader = new StreamReader("../../../lorem.txt");
            ReaderLocation = "../../../lorem.txt";
            Words = File.ReadLines(ReaderLocation);
        }
    }

    internal class CommonGame : GameType
    {
        public CommonGame() : base()
        {
            Reader = new StreamReader("../../../commonwords.txt");
            ReaderLocation = "../../../commonwords.txt";
            Words = File.ReadLines(ReaderLocation);
        }
    }
}
