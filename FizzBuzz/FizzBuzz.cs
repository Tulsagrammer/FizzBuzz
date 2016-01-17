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

            var upperLimit = Convert.ToInt32(args[0]);
            var maxLoops   = Convert.ToInt32(args[1]);

            // Collect all of the output writers built into ourself.
            var type = typeof(IFizzBuzzWriter);
            var typeList = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .ToList();

            // Now collect output writers that are supplied as plugin modules.
            var dInfo = new DirectoryInfo(Properties.Settings.Default.PluginFolder);
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
                ((IFizzBuzzWriter) Activator.CreateInstance(writerType)).Run(upperLimit, maxLoops);

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
