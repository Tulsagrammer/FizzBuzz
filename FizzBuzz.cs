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
        //[ImportMany]
        //static IEnumerable<Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata>> writers;

        static void Main(string[] args)
        {
            // Check for presence of command line parameters.
            if (! args.Any())
            {
                Pause(@"Yo, hoser! What's the upper range to test?");
                return;
            }
            if (args.Count() < 2)
            {
                Pause(@"Yo, hoser! How many iterations of each test?");
                return;
            }
            if (args.Count() < 3)
            {
                Pause(@"Yo, hoser! What is the plugin directory?");
                return;
            }

            var upperLimit  = Convert.ToInt32(args[0]);
            var maxLoops    = Convert.ToInt32(args[1]);
            var pluginDir = args[2];

            #region Reflection-based logic
#if Reflection
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
                Console.WriteLine("Exception: {0}", e.Message);
            }
#endif
            #endregion

            #region MEF-based logic
#if MEF
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(FizzBuzz).Assembly));
            var container = new CompositionContainer(catalog);
            var writers3 = new Writers();

            //Fill the imports of this object
            try
            {
                container.ComposeParts(writers3.writers);
            }
            catch (CompositionException compositionException)
            {
                Pause(compositionException.ToString());
            }

            //foreach (Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata> w in writers)
            //    Pause(w.Metadata.ToString());
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

    class Writers
    {
        [ImportMany]
        public IEnumerable<Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata>> writers { get; set; }
    }
}

