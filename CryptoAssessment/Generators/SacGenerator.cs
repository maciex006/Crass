using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyrptoAssessment.Helpers;

namespace CyrptoAssessment.Generators
{
    internal static class SacGenerator
    {
        private static TestTypes m_sacInputAssignedTests = TestTypes.None;
        private static TestTypes m_sacKeyAssignedTests = TestTypes.None;

        static SacGenerator()
        {
            m_sacInputAssignedTests = Configuration.SacInputGeneratorMapping;
            m_sacKeyAssignedTests = Configuration.SacKeyGeneratorMapping;
            //m_AssignedTests = ConfigurationSettings.AppSettings["sacGeneratorMapping"].ToEnum(TestTypes.None);
        }

        // Dla sacInput
        internal static List<EncriptionData> InvokeGeneratedKeys(int usedKeysCount, IEncriptable Algorithm)
        {
            List<EncriptionData> output = new List<EncriptionData>();
            for (int i = 0; i < usedKeysCount; i++)
            {
                byte[] key = new byte[Algorithm.KeySize];
                SequenceGenerator.Invoke(key);
                output.AddRange(InvokeFixedKey(Algorithm, key));
            }

            return output;
        }

        // Dla sacKey
        internal static List<EncriptionData> InvokeGeneratedInputs(int usedInputsCount, IEncriptable Algorithm)
        {
            List<EncriptionData> output = new List<EncriptionData>();
            for (int i = 0; i < usedInputsCount; i++)
            {
                byte[] input = new byte[Algorithm.BlockSize];
                SequenceGenerator.Invoke(input);
                output.AddRange(InvokeFixedInput(Algorithm, input));
            }

            return output;
        }

        // Dla sacInput
        internal static List<EncriptionData> InvokeFixedKey(IEncriptable Algorithm, byte[] key)
        {
            // Dodać unique key id generator.
            byte[] input = new byte[Algorithm.BlockSize];
            byte[] shifts = new byte[9] { 0, 1, 2, 4, 8, 16, 32, 64, 128 };
            List<EncriptionData> output = new List<EncriptionData>();

            for (int i = 0; i < Algorithm.BlockSize; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    input[i] += shifts[j];
                    Algorithm.Input = input;
                    Algorithm.Key = key;
                    Algorithm.Run();
                    output.Add(new EncriptionData(input, Algorithm.Output, key, m_sacInputAssignedTests));
                }
            }

            return output;
        }

        // Dla sacKey
        internal static List<EncriptionData> InvokeFixedInput(IEncriptable Algorithm, byte[] input)
        {
            byte[] key = new byte[Algorithm.KeySize];
            byte[] shifts = new byte[9] { 0, 1, 2, 4, 8, 16, 32, 64, 128 };
            List<EncriptionData> output = new List<EncriptionData>();

            for (int i = 0; i < Algorithm.BlockSize; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    key[i] += shifts[j];
                    try
                    {
                        Algorithm.Input = input;
                        Algorithm.Key = key;
                        Algorithm.Run();
                        output.Add(new EncriptionData(input, Algorithm.Output, key, m_sacKeyAssignedTests));
                    }
                    catch (Exception)
                    {
                        // Do nothing.
                    }

                }
            }

            return output;
        }
    }
}
