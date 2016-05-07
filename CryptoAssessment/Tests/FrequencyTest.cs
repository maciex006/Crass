using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Tests
{
    internal sealed class FrequencyTest : Test
    {
        private double m_PValueThreshold = 0.05;
        private string m_PassedComment = "";
        private string m_FailedComment = "";

        internal FrequencyTest(IEnumerable<EncriptionData> pairs)
        {
            this.Pairs = pairs;
            this.Type = TestTypes.FrequencyTest;
        }

        internal override void Perform()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var cypherTexts = Pairs.Select(x => x.CypherText);
            long onesCount = cypherTexts.Select(x => AdditionalFunctions.CountOnes(x)).Sum();
            long allCount = cypherTexts.Select(x => x.Count() * 8).Sum();
            long zerosCount = allCount - onesCount;
            double overload = Math.Abs(onesCount - zerosCount);
            double sObs = overload / Math.Sqrt(allCount);
            double pValue = AdditionalFunctions.Erfc(sObs / Contants.SQRT_2);
            bool passed = pValue > m_PValueThreshold;
            sw.Stop();
            Result = new TestResult(this, Pairs.Count(), sw.Elapsed, passed, pValue, (passed ? m_PassedComment : m_FailedComment));
        }
    }
}
