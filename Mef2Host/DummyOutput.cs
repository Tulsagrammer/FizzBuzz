using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzBuzz;

namespace WriterDummy
{
    [Export(typeof(IFizzBuzzWriter))]
    [ExportMetadata("Description", "Embedded Dummy Writer")]
    public class DummyOutput : IFizzBuzzWriter
    {
        public void ResultsFinish()
        {
            throw new NotImplementedException();
        }

        public void ResultsItem(TimeSpan timeSpan, string testFunction)
        {
            throw new NotImplementedException();
        }

        public void ResultsStart()
        {
            throw new NotImplementedException();
        }

        public void Run(int upperLimit, int maxLoops, Action<int, int, IFizzBuzzDelegate> run)
        {
            throw new NotImplementedException();
        }

        public void TestFinish()
        {
            throw new NotImplementedException();
        }

        public void TestItem(int value, string tag)
        {
            throw new NotImplementedException();
        }

        public void TestStart(string testFunction)
        {
            throw new NotImplementedException();
        }
    }
}
