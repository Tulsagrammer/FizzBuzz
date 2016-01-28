using System;
using System.ComponentModel.Composition;


namespace FizzBuzz
{
    public interface IDummy
    {
        int DummyFunction();
    }

    public interface IDummyMetadata
    {
        string Description { get; }
    }


    [Export(typeof(IDummy))]
    [ExportMetadata("Description", "Silly Dummy Function!")]
    public class DummyClass : IDummy
    {
        public int DummyFunction()
        {
            Console.WriteLine("Hello from the dummy function!");
            return 0xdead;
        }
    }
}
