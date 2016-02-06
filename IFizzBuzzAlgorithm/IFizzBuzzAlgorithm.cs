using System;

namespace FizzBuzz
{
    public interface IFizzBuzzAlgorithm
    {
        void Run(int upperRange, Action<int, string> returnResult);
    }

    public interface IFizzBuzzAlgorithmMetadata
    {
        string Description { get; }
    }
}
