using System;


namespace FizzBuzz
{
    public interface IFizzBuzzWriter : IFizzBuzzDelegate
    {
        void Run(int upperLimit, int maxLoops, Action<int, int, IFizzBuzzDelegate> run);
    }

    public interface IFizzBuzzWriterMetadata
    {
        string Description { get; }
    }
}
