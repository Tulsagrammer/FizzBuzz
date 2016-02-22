/*  FizzBuzz Without A Single Conditional!!!

Eric Chevalier    14-Feb-2015

Given any number X, there are three possible values of X mod 3 and
five possible values of X mod 5. This gives 15 possible combinations
of the two. Three of these combinations indicate a number divisible
by 3 but NOT 5; two of these combinations indicate a number
divisible by 5 but NOT 3 and a single combination indicates a  number
divisible by BOTH 3 and 5.

Using this phenomena, we can easily construct a Fizz Buzz program
containing NO conditionals.

Useful tutorial for MEF:
http://www.codeproject.com/Articles/376033/From-Zero-to-Proficient-with-MEF

Command line usage:

    fizzbuzz high count [plugin_dir]

    Where
        high = high end of range to be tested
        count = number of times the tests are to be run
        plugin_dir = (optional) directory containing plugins

    Example
        fizzbuzz 20 1

*/


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace FizzBuzz
{
    internal class FizzBuzz
    {
        private static void Main(string[] args)
        {
            new Program().Run(args);
        }
    }

    public class Program
    {
        //[Import(typeof(IEngine))]
        //public IEngine engine { get; set; }
        [Import("Engine")]
        public dynamic engine { get; set; }
        [ImportMany]
        IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> algorithms;
        [ImportMany]
        IEnumerable<Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata>> writers;

        public void Run(string[] args)
        {
            #region Parameter validation and setup
            // Check for presence of command line parameters.
            if (!args.Any())
            {
                Pause(@"Yo, hoser! What's the upper range to test?");
                return;
            }
            if (args.Count() < 2)
            {
                Pause(@"Yo, hoser! How many iterations of each test?");
                return;
            }

            var upperLimit = Convert.ToInt32(args[0]);
            var maxLoops   = Convert.ToInt32(args[1]);
            var pluginDir  = args.Count() > 2 ? args[2] : @"..\..\..\Plugins";
            #endregion

            try
            {
                //var engine = new Engine();
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new AssemblyCatalog(typeof(FizzBuzz).Assembly));
                catalog.Catalogs.Add(new DirectoryCatalog(pluginDir));

                using (var container = new CompositionContainer(catalog))
                {
                    container.ComposeParts(this);
                    //engine.Algorithms = algorithms;
                    ((IEngine)engine).Algorithms = algorithms;

                    // Invoke each writer object to produce some data!
                    foreach (var w in writers)
                        //w.Value.Run(upperLimit, maxLoops, engine.Run);
                        w.Value.Run(upperLimit, maxLoops, ((IEngine)engine).Run);
                }
            }
            catch (Exception exception)
            {
                Pause(exception.ToString());
            }

            Pause("");
        }

        private static void Pause(string message)
        {
            Console.Error.WriteLine(message);
            Console.Error.Write(@"Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
