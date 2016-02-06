using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;


namespace FizzBuzz
{
    [Export(typeof(IFizzBuzzAlgorithm))]
    [ExportMetadata("Description", "A Mildly-Clever Solution")]
    public class MildlyClever1 : IFizzBuzzAlgorithm
    {
        private static readonly string[][] Fizzbuzz =
        {
            new[] { "FizzBuzz", "Fizz" },
            new[] { "Buzz", "" }
        };

        public void Run(int upperRange, Action<int, string> returnResult)
        {
            // This is another alternative, suggested by Sean W.
            for (var i = 1; i <= upperRange; ++i)
            {
                Fizzbuzz[1][1] = "" + i;
                var f = (int)Math.Ceiling((i % 3) / (double)100);
                var b = (int)Math.Ceiling((i % 5) / (double)100);
                returnResult(i, Fizzbuzz[f][b]);
            }
        }
    }


    [Export(typeof(IFizzBuzzAlgorithm))]
    [ExportMetadata("Description", "A Modified Mildly-Clever Solution")]
    public class MildlyClever2 : IFizzBuzzAlgorithm
    {
        private static readonly string[][] Fizzbuzz2 =
        {
            new[] { @"FizzBuzz", @"Fizz" },
            new[] { @"Buzz",     @"{0}"  }
        };

        public void Run(int upperRange, Action<int, string> returnResult)
        {
            // This is another alternative, suggested by Sean W.
            for (var i = 1; i <= upperRange; ++i)
            {
                var f = (int)Math.Ceiling((i % 3) / (double)100);
                var b = (int)Math.Ceiling((i % 5) / (double)100);
                returnResult(i, String.Format(Fizzbuzz2[f][b], i));
            }
        }
    }


    [Export(typeof(IFizzBuzzAlgorithm))]
    [ExportMetadata("Description", "Grotesquely Over-Engineered Solution")]
    public class OverEngineered : IFizzBuzzAlgorithm
    {
        private readonly string[] _textArray = { "Beer!!!", "Buzz", "Fizz", "" };
        private Action<int, string> _returnResult;

        public void Run(int upperRange, Action<int, string> returnResult)
        {
            _returnResult = returnResult;
            var foo = Enumerable.Range(1, upperRange);

            foreach (var i in foo)
            {
                _textArray[3] = i.ToString(CultureInfo.InvariantCulture);
                var a = GetArrayIndexes(i, upperRange);
                PrintValue(i, GetText(a));
            }
        }

        public Tuple<int, int> GetArrayIndexes(int i, int upperRange)
        {
            var a = (int)Math.Ceiling((double)i % 3 / 100);
            var b = (int)Math.Ceiling((double)i % 5 / 100) * 2;
            return new Tuple<int, int>(a, b);
        }

        private string GetText(Tuple<int, int> a)
        {
            return _textArray[a.Item1 + a.Item2];
        }

        private void PrintValue(int i, string text)
        {
            _returnResult(i, text);
        }
    }
}
