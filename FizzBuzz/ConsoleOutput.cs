using System;
using System.Diagnostics;
using System.ComponentModel.Composition;


namespace FizzBuzz
{
    [Export(typeof(IFizzBuzzWriter))]
    [ExportMetadata("Description","Console Writer")]
    public class ConsoleOutput : IFizzBuzzDelegate, IFizzBuzzWriter
    {
        private int _upperLimit;
        private int _maxLoops;

        [ImportingConstructor]
        public ConsoleOutput(string message)
        {
            var msg = string.Format(@"ConsoleOutput constructor says: {0}", message);
            Trace.WriteLine(msg);
            Console.Error.WriteLine(msg);
        }

        public void Run(int upperLimit, int maxLoops, Action<int, int, IFizzBuzzDelegate> run)
        {
            _upperLimit = upperLimit;
            _maxLoops = maxLoops;
            run(upperLimit, maxLoops, this);
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