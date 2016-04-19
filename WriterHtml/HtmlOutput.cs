using System;
using System.Diagnostics;
using System.ComponentModel.Composition;


namespace FizzBuzz
{
    [Export(typeof(IFizzBuzzWriter))]
    [ExportMetadata("Description", "HTML Writer")]
    public class HtmlOutput : IFizzBuzzDelegate, IFizzBuzzWriter
    {
        private int _upperLimit;
        private int _maxLoops;

        [ImportingConstructor]
        public HtmlOutput(string message)
        {
            var msg = string.Format(@"HtmlOutput constructor says: {0}", message);
            Trace.WriteLine(msg);
            Console.Error.WriteLine(msg);
        }

        public void Run(int upperLimit, int maxLoops, Action<int, int, IFizzBuzzDelegate> run)
        {
            _upperLimit = upperLimit;
            _maxLoops = maxLoops;

            Console.WriteLine(@"<!DOCTYPE html>");
            Console.WriteLine(@"<html>");
            Console.WriteLine(@"<head>");
            Console.WriteLine(@"    <title>FizzBuzz Fun!</title>");
            Console.WriteLine(@"</head>");
            Console.WriteLine(@"<body>");

            run(upperLimit, maxLoops, this);

            Console.WriteLine(@"</body>");
            Console.WriteLine(@"</html>");
        }

        public void TestStart(string testFunction)
        {
            Console.WriteLine(@"    <h1>{0}</h1>", testFunction);
            Console.WriteLine(@"    <table>");
        }

        public void TestItem(int value, string tag)
        {
            Console.WriteLine(@"        <tr><td>{0}</td><td>{1}</td></tr>", value, tag);
        }

        public void TestFinish()
        {
            Console.WriteLine(@"    </table>");
        }

        public void ResultsStart()
        {
            Console.WriteLine(@"    <h1>Results:</h1>");
            Console.WriteLine(@"    <table>");
        }

        public void ResultsItem(TimeSpan timeSpan, string testFunction)
        {
            Console.WriteLine(@"        <tr><td>{0}</td><td>{1}</td></tr>", timeSpan, testFunction);
        }

        public void ResultsFinish()
        {
            Console.WriteLine(@"    </table>");
            Console.WriteLine(@"    <p>Each test performed {0} times with max range of {1}.<p>",
                _maxLoops, _upperLimit);
        }
    }
}