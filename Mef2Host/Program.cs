/*  Good overfiew of pros/cons mef1 vs. mef2:
 *      http://www.codeproject.com/Tips/488513/MEF-in-NET
 * 
 */


using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzBuzz;
using System.Composition;

namespace Mef2Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pluginDir;

            pluginDir = args.Any() ? args[0] : @"..\..\..\Plugins";

#if true
            var partConfig = new ContainerConfiguration()
                    .WithAssembly(Assembly.GetExecutingAssembly());
            var compositionHost = partConfig.CreateContainer();
            var algorithms = compositionHost.GetExports<IFizzBuzzAlgorithm>();
            var writers = compositionHost.GetExports<IFizzBuzzWriter>();

            Console.WriteLine("\nAlgorithm plugins:");
            if (algorithms != null)
                Console.WriteLine(@"{0,3} Algorithm plugins", algorithms.Count());
            Console.WriteLine("\nWriter plugins:");
            if (writers != null)
                Console.WriteLine(@"{0,3} Writer plugins", writers.Count());
#endif

#if false
            var partConfig = new ContainerConfiguration()
                    .WithAssembly(Assembly.GetExecutingAssembly());
            var compositionHost = partConfig.CreateContainer();
            var algorithms = compositionHost.GetExports<ExportFactory<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>>();
            var writers = compositionHost.GetExports<IFizzBuzzWriter>();

            Console.WriteLine("\nAlgorithm plugins:");
            if (algorithms != null)
                foreach (var a in algorithms)
                    Console.WriteLine(@"  {0}", a.Metadata.Description);
            Console.WriteLine("\nWriter plugins:");
            if (writers != null)
                Console.WriteLine(@"{0,3} Writer plugins", writers.Count());
#endif

            Utility.Pause("");
        }
    }

    internal class Utility
    {
        public static void Pause(string Message)
        {
            Console.Error.WriteLine(Message);
            Console.Error.Write(@"Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
