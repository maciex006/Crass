using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoAssessment.Generators;
using System.Runtime.CompilerServices;
using CryptoAssessment.Tests;

[assembly: InternalsVisibleTo("UnitTests")]
namespace CryptoAssessment
{
    /// <summary>
    /// Class used for analizing cryptographic algorithm.
    /// </summary>
    public class Analizer
    {
        private IEncriptable m_Algorithm;
        private EncriptionDataGenerator m_DataGenerator;
        private TestUtil m_TestUtility;
        private TestTypes m_InitializedTypes;
        private List<Test> m_Tests;
        private List<EncriptionData> m_EncData;

        /// <summary>
        /// Creates and initializates Analizer class.
        /// </summary>
        /// <param name="alg">Algorithm to be analized.</param>
        public Analizer(IEncriptable alg)
        {
            m_Algorithm = alg;
            m_DataGenerator = new EncriptionDataGenerator(m_Algorithm);
            m_TestUtility = new TestUtil();
            m_InitializedTypes = TestTypes.None;
            m_Tests = new List<Test>();
            m_EncData = new List<EncriptionData>();
        }

        /// <summary>
        /// Runs testing procedure.
        /// </summary>
        /// <param name="types">Types of tests to be performed.</param>
        public void Run(TestTypes types)
        {
            int newTypes = types - m_InitializedTypes;
            TestTypes typesToInit = (newTypes > 0) ? (TestTypes)newTypes : TestTypes.None;
            m_EncData.AddRange(m_DataGenerator.Invoke(typesToInit));
            m_Tests.AddRange(m_TestUtility.Init(typesToInit, m_EncData));
            m_InitializedTypes = (TestTypes)(m_InitializedTypes + newTypes);
            m_TestUtility.Run(m_Tests);
        }

        public void GetResults()
        {
            // Tymczasowa implementacja:
            // Tu nie może być wyświetlania - na razie w celu debugu
            foreach (var test in m_Tests)
            {
                Console.WriteLine(test.Result);
            }
        }

        public void GetResults(TestTypes types)
        {
            // TODO
        }
    }
}
