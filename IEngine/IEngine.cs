using System;
using System.Collections.Generic;

namespace FizzBuzz
{
    public interface IEngine
    {
        IEnumerable<Lazy<IFizzBuzzAlgorithm, IFizzBuzzAlgorithmMetadata>> Algorithms { get; set; }
        void Run(int upperLimit, int maxLoops, IFizzBuzzDelegate driver);
    }
}
