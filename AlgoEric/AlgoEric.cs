using System;
using System.ComponentModel.Composition;


namespace FizzBuzz
{
    [Export(typeof(IFizzBuzzAlgorithm))]
    [ExportMetadata("Description", "Erics Fine Solution #1")]
    public class EricAlgorithm1 : IFizzBuzzAlgorithm
    {
        private static readonly string[] Tags =
        {
            @"FizzBuzz", @"Fizz", @"Fizz", @"Fizz", @"Fizz",    // V mod 3 = 0
            @"Buzz",     @"{0}",  @"{0}",  @"{0}",  @"{0}",     // V mod 3 = 1
            @"Buzz",     @"{0}",  @"{0}",  @"{0}",  @"{0}"      // V mod 3 = 2
        };

        public void Run(int upperRange, Action<int, string> returnResult)         // EricsFineSolution1
        {
            // Index directly into the "Tags" table.
            for (var i = 1; i <= upperRange; i++)
                returnResult(i, String.Format(Tags[(i % 3) * 5 + i % 5], i));
        }
    }

#if true
    [Export(typeof(IFizzBuzzAlgorithm))]
    [ExportMetadata("Description", "Erics Fine Solution #2")]
    public class EricAlgorithm2 : IFizzBuzzAlgorithm
    {
        private static readonly byte[,] TagsIndex =
        {
            {3, 1, 1, 1, 1},        // V mod 3 = 0
            {2, 0, 0, 0, 0},        // V mod 3 = 1
            {2, 0, 0, 0, 0}         // V mod 3 = 2
        };

        private static readonly string[] Tags2 = { "{0}", "Fizz", "Buzz", "FizzBuzz" };

        public void Run(int upperRange, Action<int, string> returnResult)
        {
            // Index into the "Tags2" table via the "TagsIndex" table.
            // Inspired by a suggestion from Sean W.
            for (var i = 1; i <= upperRange; i++)
                returnResult(i, String.Format(Tags2[TagsIndex[i % 3, i % 5]], i));
        }
    }
#endif
}
