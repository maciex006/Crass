using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Tests
{
    internal sealed class BlockFrequencyTest : Test
    {
        private int m_CtLength = 0;
        private int m_CombIndex = 0;
        private int M = 4; // Liczba bajtow w bloku
        private int n = 0; // Liczba bajtow w comb
        private int N = 0; // Liczba blokow w comb
        private byte[] m_Sequence;
        private int m_CombCount = 0; // Liczba comb

        private double[] m_ComFreq;
        private double[,] m_ComBlockFreq;
        private double[] m_ChiSquareStat; //CHI
        private double[] m_GammaResult; // GAMMA
        private double m_PValueThreshold = 0.05;
        private string m_PassedComment = "Sequence seems to have an expected distribution of zero-ones frequency.";
        private string m_FailedComment = "Significant fluctuations of zero-ones frequency over the sequence length.";

        internal BlockFrequencyTest(IEnumerable<EncriptionData> pairs)
        {
            var pair = pairs.FirstOrDefault();
            if (pair != null)
            {
                m_CtLength = pair.CypherText.Length;
                double mLowerLim = 0.01 * m_CtLength;
                if (M <= 0.01 * mLowerLim)
                {
                    M = (int)mLowerLim;
                }

                while (n < 16 || n % M != 0)
                {
                    m_CombIndex++;
                    n = m_CombIndex * m_CtLength;
                }

                N = Math.Abs(n / M);
            }

            this.Pairs = pairs;
            this.Type = TestTypes.FrequencyTest;
            m_Sequence = this.Pairs.SelectMany(x => x.CypherText).ToArray();
            m_CombCount = this.Pairs.Count() / m_CombIndex;
            m_ComFreq = new double[m_CombCount];
            m_ComBlockFreq = new double[m_CombCount,N];
            m_ChiSquareStat = new double[m_CombCount];
            m_GammaResult = new double[m_CombCount];
        }

        internal override void Perform()
        {
            Stopwatch sw = Stopwatch.StartNew();

            int i = 0;
            int blockCount = 0;
            int combCount = 0;
            double onesBlockCount = 0;
            double onesCombCount = 0;
            double onesCount = 0;

            int bitsBlockCount = M * 8;
            int bitsCombCount = n * 8;
            int bitsCount = n * m_CombCount * 8;
            double[] chi = new double[N]; // CHI
            foreach(byte b in m_Sequence)
            {
                int ones = AdditionalFunctions.CountOnes(b);
                onesCount += ones;
                onesCombCount += ones;
                onesBlockCount += ones;
                i++;
                if (i == M)
                {
                    i = 0;
                    m_ComBlockFreq[combCount, blockCount] = onesBlockCount / bitsBlockCount;
                    chi[blockCount] = m_ComBlockFreq[combCount, blockCount]; // CHI
                    onesBlockCount = 0;
                    blockCount++;
                }

                if (blockCount == N)
                {
                    blockCount = 0;
                    m_ChiSquareStat[combCount] = 4 * M * AdditionalFunctions.CountChiStat(chi); // CHI
                    m_GammaResult[combCount] = GammaFunctions.GammaUpper((double)N / 2, m_ChiSquareStat[combCount]); // GAMMA
                    m_ComFreq[combCount] = onesCombCount / bitsCombCount;
                    onesCombCount = 0;
                    combCount++;
                }
            }

            double chiAvg;
            double chiStdDev = AdditionalFunctions.CalculateStdDev(m_GammaResult, out chiAvg);
            double blockFreqPValue = chiAvg - chiStdDev;
            bool passed = blockFreqPValue > m_PValueThreshold;
            sw.Stop();
            Result = new TestResult(this, Pairs.Count(), sw.Elapsed, passed, blockFreqPValue, (passed ? m_PassedComment : m_FailedComment));
        }
    }
}

