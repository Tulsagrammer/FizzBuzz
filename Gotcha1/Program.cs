using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using FizzBuzz;
using System.Linq;

namespace Gotcha1
{
    #region Gotcha: building CompositionContainer in a static method
#if false
    class Program
    {
        [ImportMany]
        static IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> algorithms;
        [ImportMany]
        static IEnumerable<Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata>> writers;

        static void Main(string[] args)
        {
            string pluginDir;

            if (args.Any())
                pluginDir = args[0];
            else
                pluginDir = @"..\..\..\Plugins";

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(pluginDir));

            using (var container = new CompositionContainer(catalog))
            {
                container.ComposeParts(container);
                Console.WriteLine();
                Console.WriteLine("Algorithm plugins:");
                if (algorithms != null)
                    foreach (var a in algorithms)
                        Console.WriteLine(@"  {0}", a.Metadata.Description);
                Console.WriteLine();
                Console.WriteLine("Writer plugins:");
                if (writers != null)
                    foreach (var w in writers)
                        Console.WriteLine(@"  {0}", w.Metadata.Description);
            }
            Utility.Pause("");
        }
    }
#endif
    #endregion

    #region Solution #1: building CompositionContainer in a dynamic method
#if false
    class Program
    {
        static void Main(string[] args)
        {
            string pluginDir;

            if (args.Any())
                pluginDir = args[0];
            else
                pluginDir = @"..\..\..\Plugins";

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(pluginDir));

            var id = new ImportDefinitions();
            using (var container = new CompositionContainer(catalog))
            {
                container.ComposeParts(id);
                Console.WriteLine();
                Console.WriteLine("Algorithm plugins:");
                if (id.algorithms != null)
                    foreach (var a in id.algorithms)
                        Console.WriteLine(@"  {0}", a.Metadata.Description);
                Console.WriteLine();
                Console.WriteLine("Writer plugins:");
                if (id.writers != null)
                    foreach (var w in id.writers)
                        Console.WriteLine(@"  {0}", w.Metadata.Description);
            }
            Utility.Pause("");
        }

        public class ImportDefinitions
        {
            [ImportMany]
            public IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> algorithms { get; set; }
            [ImportMany]
            public IEnumerable<Lazy<IFizzBuzzWriter, IFizzBuzzWriterMetadata>> writers { get; set; }
        }
    }
#endif
    #endregion

    #region Solution #2: explicit collection of desired objects
#if false
    class Program
    {
        static void Main(string[] args)
        {
            string pluginDir;

            if (args.Any())
                pluginDir = args[0];
            else
                pluginDir = @"..\..\..\Plugins";

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(pluginDir));

            using (var container = new CompositionContainer(catalog))
            {
                container.ComposeParts(container);
                var algorithms = container.GetExports<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>();
                var writers = container.GetExports<IFizzBuzzWriter, IFizzBuzzWriterMetadata>();
                Console.WriteLine();
                Console.WriteLine("Algorithm plugins:");
                if (algorithms != null)
                    foreach (var a in algorithms)
                        Console.WriteLine(@"  {0}", a.Metadata.Description);
                Console.WriteLine();
                Console.WriteLine("Writer plugins:");
                if (writers != null)
                    foreach (var w in writers)
                        Console.WriteLine(@"  {0}", w.Metadata.Description);
            }
            Utility.Pause("");
        }
    }
#endif
    #endregion

    #region Incorrect access to multiple components
#if true
    class Program
    {
        static void Main(string[] args)
        {
            string pluginDir;

            if (args.Any())
                pluginDir = args[0];
            else
                pluginDir = @"..\..\..\Plugins";

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(pluginDir));

            var id = new ImportDefinitions();
            using (var container = new CompositionContainer(catalog))
            {
                try
                {
                    container.ComposeParts(id);
                    Console.WriteLine(@"id.Algorithms.Count: {0}", id.Algorithms.Count());
                    if (id.Algorithm == null)
                        Console.WriteLine(@"id.Algorithm was allocated as null!!!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Utility.Pause("");
        }

        public class ImportDefinitions
        {
            [Import(typeof(IFizzBuzzAlgorithm))]
            public IFizzBuzzAlgorithm Algorithm { get; set; }
            [ImportMany]
            public IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> Algorithms { get; set; }
        }
    }
#endif
    #endregion

    class Utility
    {
        public static void Pause(string message)
        {
            Console.Error.WriteLine(message);
            Console.Error.Write(@"Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
