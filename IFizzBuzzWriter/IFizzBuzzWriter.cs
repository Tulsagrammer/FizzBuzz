namespace FizzBuzz
{
    public interface IFizzBuzzWriter
    {
        void Run(int upperLimit, int maxLoops);
    }

    public interface IFizzBuzzWriterMetadata
    {
        string Description { get; }
    }
}
