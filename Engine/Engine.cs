using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizzBuzz
{
    public class Engine
    {
        private int _upperLimit;
        private int _maxLoops;

        private readonly List<Tuple<TimeSpan, string>>
            _procTimes = new List<Tuple<TimeSpan, string>>();

        private IFizzBuzzDelegate _driver;

        public void Run(int upperLimit, int maxLoops, IFizzBuzzDelegate driver)
        {
            _upperLimit = upperLimit;
            _maxLoops   = maxLoops;
            _driver     = driver;

            TestRunner(EricsFineSolution1, "EricsFineSolution1");
            TestRunner(EricsFineSolution2, "EricsFineSolution2");
            TestRunner(MildlyCleverSolution1, "MildlyCleverSolution1");
            TestRunner(MildlyCleverSolution2, "MildlyCleverSolution2");
            TestRunner(GrotesquelyOverengineeredSolution,
                        "GrotesquelyOverengineeredSolution");

            driver.ResultsStart();
            _procTimes.ForEach(t => _driver.ResultsItem(t.Item1, t.Item2));
            driver.ResultsFinish();
        }

        private void TestRunner(Action<int> testAction, string tag)
        {
            _driver.TestStart(tag);
            var start = Process.GetCurrentProcess().UserProcessorTime;
            for (var i = 0; i < _maxLoops; i++)
                testAction(_upperLimit);
            var procTime = Process.GetCurrentProcess().UserProcessorTime.Subtract(start);
            _procTimes.Add(Tuple.Create(procTime, tag));
            _driver.TestFinish();
        }


        #region Eric's Fine Solutions

        private static readonly string[] Tags =
        {
            @"FizzBuzz", @"Fizz", @"Fizz", @"Fizz", @"Fizz",    // V mod 3 = 0
            @"Buzz",     @"{0}",  @"{0}",  @"{0}",  @"{0}",     // V mod 3 = 1
            @"Buzz",     @"{0}",  @"{0}",  @"{0}",  @"{0}"      // V mod 3 = 2
        };

        private static readonly byte[,] TagsIndex =
        {
            {3, 1, 1, 1, 1},        // V mod 3 = 0
            {2, 0, 0, 0, 0},        // V mod 3 = 1
            {2, 0, 0, 0, 0}         // V mod 3 = 2
        };

        private static readonly string[] Tags2 = { "{0}", "Fizz", "Buzz", "FizzBuzz" };

        private void EricsFineSolution1(int upperRange)
        {
            // Index directly into the "Tags" table.
            for (var i = 1; i <= upperRange; i++)
                _driver.TestItem(i, String.Format(Tags[(i % 3) * 5 + i % 5], i));
        }

        private void EricsFineSolution2(int upperRange)
        {
            // Index into the "Tags2" table via the "TagsIndex" table.
            // Inspired by a suggestion from Sean W.
            for (var i = 1; i <= upperRange; i++)
                _driver.TestItem(i, String.Format(Tags2[TagsIndex[i % 3, i % 5]], i));
        }

        #endregion

        #region A Mildly-Clever Solution

        private static readonly string[][] Fizzbuzz =
        {
            new[] { "FizzBuzz", "Fizz" },
            new[] { "Buzz", "" }
        };

        private void MildlyCleverSolution1(int upperRange)
        {
            // This is another alternative, suggested by Sean W.
            for (var i = 1; i <= upperRange; ++i)
            {
                Fizzbuzz[1][1] = "" + i;
                var f = (int)Math.Ceiling((i % 3) / (double)100);
                var b = (int)Math.Ceiling((i % 5) / (double)100);
                _driver.TestItem(i, Fizzbuzz[f][b]);
            }
        }

        #endregion

        #region A Modified Mildly-Clever Solution

        private static readonly string[][] Fizzbuzz2 =
        {
            new[] { @"FizzBuzz", @"Fizz" },
            new[] { @"Buzz",     @"{0}"  }
        };

        private void MildlyCleverSolution2(int upperRange)
        {
            // This is another alternative, suggested by Sean W.
            for (var i = 1; i <= upperRange; ++i)
            {
                var f = (int)Math.Ceiling((i % 3) / (double)100);
                var b = (int)Math.Ceiling((i % 5) / (double)100);
                _driver.TestItem(i, String.Format(Fizzbuzz2[f][b], i));
            }
        }

        #endregion

        #region Grotesquely Over-Engineered Solution

        private readonly string[] _textArray = { "Beer!!!", "Buzz", "Fizz", "" };

        private void GrotesquelyOverengineeredSolution(int upperRange)
        {
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
            _driver.TestItem(i, text);
        }

        #endregion
    }
}
