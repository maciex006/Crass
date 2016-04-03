using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyrptoAssessment.Tests
{
    internal sealed class RandomnessTest : Test
    {
        internal RandomnessTest(IEnumerable<EncriptionData> pairs)
        {
            this.Pairs = pairs;
            this.Type = TestTypes.Randomness;
        }

        internal override void Perform()
        {
            Stopwatch sw = Stopwatch.StartNew();
            int minBlock = 15; // min n = 120 (15*8)
            var ctCombined = Pairs.SelectMany(x => x.CypherText);
            // Tak albo może wywołać juz ze wszystkimi
            int counter = 0;
            double result = 0;
            for (int i = 0; i < ctCombined.Count(); i = i + minBlock)
            {
                result += RandomnessTestUtil.FrequencyTest(ctCombined.Skip(i).Take(minBlock).ToArray(), 120);
                counter++;
            }
            sw.Stop();
            Result = new TestResult(this, Pairs.Count(), sw.Elapsed, result / counter);
        }
    }
}
