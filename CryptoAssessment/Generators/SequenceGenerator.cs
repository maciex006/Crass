using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CyrptoAssessment.Generators
{
    static class SequenceGenerator
    {
        private static int seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> pseudoRandomGenerator =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        internal static void Invoke(byte[] input)
        {
            pseudoRandomGenerator.Value.NextBytes(input);
        }
    }
}
