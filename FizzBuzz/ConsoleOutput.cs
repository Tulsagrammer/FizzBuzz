using System;
using FizzBuzz;

namespace FizzBuzz
{
    public class ConsoleOutput : IFizzBuzzDelegate, IFizzBuzzWriter
    {
        private int _upperLimit;
        private int _maxLoops;

        public void Run(int upperLimit, int maxLoops)
        {
            _upperLimit = upperLimit;
            _maxLoops = maxLoops;
            new Engine().Run(upperLimit, maxLoops, this);
        }

        public void TestStart(string testFunction)
        {
            var tag1 = String.Format(@"{0} tests:", testFunction);
            var tag2 = new String('=', tag1.Length);
            Console.WriteLine();
            Console.WriteLine(tag1);
            Console.WriteLine(tag2);
        }

        public void TestFinish()
        {
        }

        public void TestItem(int value, string tag)
        {
            Console.WriteLine(tag);
        }

        public void ResultsStart()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(@"Results:");
        }

        public void ResultsFinish()
        {
            Console.WriteLine(@"Each test performed {0} times with max range of {1}.",
                _maxLoops, _upperLimit);
        }

        public void ResultsItem(TimeSpan timeSpan, string testFunction)
        {
            Console.Error.WriteLine(@"{0}  {1}", timeSpan, testFunction);
        }
    }
}