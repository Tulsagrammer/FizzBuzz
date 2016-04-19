/*  Good overfiew of pros/cons mef1 vs. mef2:
 *      http://www.codeproject.com/Tips/488513/MEF-in-NET
 * 
 */


using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Linq;
using FizzBuzz;

namespace Mef2Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var pluginDir = args.Any() ? args[0] : @"..\..\..\Plugins";

            // Add ourself to the ContainerConfiguration because we might have
            // embedded components.
            var partConfig = new ContainerConfiguration()
                    .WithAssembly(Assembly.GetExecutingAssembly());

            // Now add in assemblies in the plugins directory.
            var dInfo = new DirectoryInfo(pluginDir);
            var files = dInfo.GetFiles("*.dll");
            foreach (var file in files)
                partConfig.WithAssembly(Assembly.LoadFile(file.FullName));

            object description;
            var compositionHost = partConfig.CreateContainer();
            var algorithms = compositionHost.GetExports
                    <ExportFactory<IFizzBuzzAlgorithm, IDictionary<string, object>>>();
            var writers = compositionHost.GetExports
                    <ExportFactory<IFizzBuzzWriter, IDictionary<string, object>>>();
            Console.WriteLine();
            Console.WriteLine("Algorithm plugins:");
            if (algorithms != null)
                foreach (var a in algorithms)
                {
                    a.Metadata.TryGetValue("Description", out description);
                    Console.WriteLine(@"  {0}", description);
                }
            Console.WriteLine();
            Console.WriteLine("Writer plugins:");
            if (writers != null)
                foreach (var w in writers)
                {
                    w.Metadata.TryGetValue("Description", out description);
                    Console.WriteLine(@"  {0}", description);
                }

            Utility.Pause("");
        }
    }

    internal class Utility
    {
        public static void Pause(string message)
        {
            Console.Error.WriteLine(message);
            Console.Error.Write(@"Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
