using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Tests
{
    internal sealed class RunsTest : Test
    {
        private double m_PValueThreshold = 0.05;
        private string m_PassedComment = "";
        private string m_FailedComment = "";

        internal RunsTest(IEnumerable<EncriptionData> pairs)
        {
            this.Pairs = pairs;
            this.Type = TestTypes.FrequencyTest;
        }

        internal override void Perform()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var cypherTexts = Pairs.Select(x => x.CypherText);
            double onesCount = cypherTexts.Select(x => AdditionalFunctions.CountOnes(x)).Sum();
            double allCount = cypherTexts.Select(x => x.Count() * 8).Sum();
            double onesRatio = onesCount / allCount;
            double pValue = 0;
            var test2 = (Math.Abs(onesRatio - 1 / 2));
            var test1 = (2 / Math.Abs(allCount));
            //if (Math.Abs(onesRatio - 1 / 2) < (2 / Math.Abs(allCount)))
            {
                long vObs = 0;
                bool previous = false;
                foreach (byte b in cypherTexts.SelectMany(x => x))
                {
                    for (int i = 7; i >= 0; i--)
                    {
                        int now = (b >> i);
                        bool current = now % 2 == 0;
                        if (current != previous)
                        {
                            vObs++;
                        }

                        previous = current;
                    }
                }

                pValue = AdditionalFunctions.Erfc(Math.Abs(vObs - 2 * allCount * onesRatio * (1 - onesRatio)) / (2 * Math.Sqrt(2 * allCount) * onesRatio * (1 - onesRatio)));
            }

            bool passed = pValue > m_PValueThreshold;
            sw.Stop();
            Result = new TestResult(this, Pairs.Count(), sw.Elapsed, passed, pValue, (passed ? m_PassedComment : m_FailedComment));
        }
    }
}
