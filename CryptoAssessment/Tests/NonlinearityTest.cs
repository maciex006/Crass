using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Tests
{
    internal sealed class NonlinearityTest : Test
    {
        private byte[] m_CurrentPlainText = null;
        private byte[] m_CurrentCypherText = null;
        private Dictionary<int, int> m_invocationValues = new Dictionary<int, int>();
        private double[] m_ResultTable = null;

        internal NonlinearityTest(IEnumerable<EncriptionData> pairs)
        {
#if DEBUG
            Console.WriteLine(this.ToString() + " run with " + pairs.Count() + " samples.");
#endif
            this.Pairs = pairs;
            this.Type = TestTypes.Nonlinearity;
            if (pairs.Count() > 0)
            {
                FillInvocationValues(pairs.First().PlainText.Count());
            }
        }

        internal override void Perform()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var grouped = Pairs.GroupBy(x => x.UniqueKeyId);

            foreach (var group in grouped)
            {
                CalculateMetricForFixedKey(group);
            }

            sw.Stop();
            //Result = new TestResult(this, Pairs.Count(), sw.Elapsed, false, m_ResultTable.Max()); //TODO poprawić.
        }

        private void CalculateMetricForFixedKey(IGrouping<int?, EncriptionData> group)
        {
            int iter = 0;
            foreach (var p_invVal in m_invocationValues)
            {
                foreach (var c_invVal in m_invocationValues)
                {
                    CalculateMetricForShift(group, p_invVal.Key, p_invVal.Value, c_invVal.Key, c_invVal.Value, iter);
                    iter++;
                }
            }
        }

        private void FillInvocationValues(int blockSize)
        {
            for (int i = 1; i <= blockSize; i++)
            {
                if (blockSize % i == 0)
                {
                    m_invocationValues.Add(i, (blockSize / i));
                }
            }

            m_ResultTable = m_invocationValues.Count() > 0 ? new double[m_invocationValues.Count() * m_invocationValues.Count()] : null;
        }

        private void CalculateMetricForShift(IGrouping<int?, EncriptionData> group, int p_Size, int p_Offset, int c_Size, int c_Offset, int resultIndex)
        {
            int countOnes = 0;
            int countAll = 0;

            foreach (EncriptionData pair in group)
            {
                m_CurrentPlainText = pair.PlainText;
                m_CurrentCypherText = pair.CypherText;
                if (CalculateXor(p_Size, p_Offset, c_Size, c_Offset))
                {
                    countOnes++;
                }

                countAll++;
            }

            double currentResult = Math.Abs(0.5 - ((float)countOnes / countAll));
            m_ResultTable[resultIndex] = m_ResultTable[resultIndex] != 0 ? (m_ResultTable[resultIndex] + currentResult) / 2 : currentResult;
        }

        private bool CalculateXor(int p_Size, int p_Offset, int c_Size, int c_Offset)
        {
            p_Offset--;
            c_Offset--;
            bool result = false;

            for (int i = p_Offset; i < p_Size + p_Offset; i++)
            {
                result = result != XorByte(m_CurrentPlainText[i]);
            }

            for (int i = c_Offset; i < c_Size + c_Offset; i++)
            {
                result = result != XorByte(m_CurrentCypherText[i]);
            }

            return result;
        }

        private bool XorByte(byte input)
        {
            int count = 0;
            while (input != 0)
            {
                count++;
                input &= (byte)(input - 1);
            }

            return (count % 2) != 0; // count = liczba jedynek, jeśli nieparzysta to zawracamy (1) true
        }
    }
}
