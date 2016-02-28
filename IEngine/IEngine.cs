using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace FizzBuzz
{
    public interface IEngine
    {
        CompositionContainer Container { get; set; }

        void Run(int upperLimit, int maxLoops, IFizzBuzzDelegate driver);
    }
}
