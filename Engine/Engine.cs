using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace FizzBuzz
{
    //[Export(typeof(FizzBuzz.IEngine))]
    [Export("Engine")]
    public class Engine : IEngine
    {
        public IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> Algorithms { get; set; }

        public void Run(int upperLimit, int maxLoops, IFizzBuzzDelegate driver)
        {
            var procTimes = new List<Tuple<TimeSpan, string>>();

            foreach (var a in Algorithms)
            {
                var tag = a.Metadata.Description;
                driver.TestStart(tag);
                var start = Process.GetCurrentProcess().UserProcessorTime;
                for (var i = 0; i < maxLoops; i++)
                    a.Value.Run(upperLimit, driver.TestItem);
                var procTime = Process.GetCurrentProcess().UserProcessorTime.Subtract(start);
                procTimes.Add(Tuple.Create(procTime, tag));
                driver.TestFinish();
            }

            driver.ResultsStart();
            procTimes.ForEach(t => driver.ResultsItem(t.Item1, t.Item2));
            driver.ResultsFinish();
        }
    }
}
