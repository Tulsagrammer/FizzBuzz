using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace FizzBuzz
{
    public class Engine
    {
        private int _upperLimit;
        private int _maxLoops;

        private readonly List<Tuple<TimeSpan, string>>
            _procTimes = new List<Tuple<TimeSpan, string>>();

        private IFizzBuzzDelegate _driver;

        public IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> Algorithms { get; set; }

        public void Run(int upperLimit, int maxLoops, IFizzBuzzDelegate driver)
        {
            _upperLimit = upperLimit;
            _maxLoops   = maxLoops;
            _driver     = driver;

            foreach (var a in Algorithms)
                TestRunner(a.Value.Run, a.Metadata.Description.ToString());

            driver.ResultsStart();
            _procTimes.ForEach(t => _driver.ResultsItem(t.Item1, t.Item2));
            driver.ResultsFinish();
        }

        private void TestRunner(Action<int, Action<int, string>> testAction, string tag)
        {
            _driver.TestStart(tag);
            var start = Process.GetCurrentProcess().UserProcessorTime;
            for (var i = 0; i < _maxLoops; i++)
                testAction(_upperLimit, _driver.TestItem);
            var procTime = Process.GetCurrentProcess().UserProcessorTime.Subtract(start);
            _procTimes.Add(Tuple.Create(procTime, tag));
            _driver.TestFinish();
        }
    }
}
