/*  FizzBuzz Without A Single Conditional!!!

Eric Chevalier    14-Feb-2015

Given any number X, there are three possible values of X mod 3 and
five possible values of X mod 5. This gives 15 possible combinations
of the two. Three of these combinations indicate a number divisible
by 3 but NOT 5; two of these combinations indicate a number
divisible by 5 but NOT 3 and a single combination indicates a number
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FizzBuzz
{
    class FizzBuzz
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }
    }

    public class Program
    {
#if MEF
        [ImportMany]
        IEnumerable<Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata>> writers;
        [ImportMany]
        IEnumerable<Lazy<IDummy, IDummyMetadata>> dummies;
#endif

        public void Run(string[] args)
        {
            string pluginDir;

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

            if (args.Count() > 2)
                pluginDir = args[2];
            else
                pluginDir = @"..\..\..\Plugins";

            var upperLimit = Convert.ToInt32(args[0]);
            var maxLoops = Convert.ToInt32(args[1]);
            #endregion

            #region Reflection-based logic
#if REFLECTION
            try
            {
                // Collect all of the output writers built into ourself.
                var type = typeof(IFizzBuzzWriter);
                var typeList = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                    .ToList();

                // Now collect output writers that are supplied as plugin modules.
                var dInfo = new DirectoryInfo(pluginDir);
                var files = dInfo.GetFiles("*.dll");
                if (files.Any())
                    foreach (FileInfo file in files)
                    {
                        var pluginTypes = Assembly.LoadFile(file.FullName)
                            .GetTypes().Where(p => type.IsAssignableFrom(p) && p.IsClass);
                        foreach (var pluginType in pluginTypes)
                            typeList.Add(pluginType);
                    }

                // Invoke each writer object to produce some data!
                foreach (var writerType in typeList)
                    ((IFizzBuzzWriter)Activator.CreateInstance(writerType)).Run(upperLimit, maxLoops);
            }
            catch (Exception e)
            {
                Pause(string.Format("Exception: {0}", e.Message));
            }
#endif
            #endregion

            #region MEF-based logic
#if MEF
            try
            {
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new AssemblyCatalog(typeof(FizzBuzz).Assembly));
                catalog.Catalogs.Add(new DirectoryCatalog(pluginDir));

                using (var container = new CompositionContainer(catalog))
                {
                    container.ComposeParts(this);

                    // Invoke each writer object to produce some data!
                    foreach (Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata> w in writers)
                        w.Value.Run(upperLimit, maxLoops);
                    // Invoke our dummy function(s) just for fun!
                    foreach (Lazy<IDummy, IDummyMetadata> d in dummies)
                        d.Value.DummyFunction();
                }
            }
            catch (Exception exception)
            {
                Pause(exception.ToString());
            }
#endif
            #endregion

            Pause("");
        }

        private static void Pause(string Message)
        {
            Console.Error.WriteLine(Message);
            Console.Error.Write(@"Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
