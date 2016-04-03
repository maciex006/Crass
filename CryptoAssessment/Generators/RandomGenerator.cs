using System;
using System.Configuration;
using System.Collections.Generic;
using CyrptoAssessment.Helpers;

namespace CyrptoAssessment.Generators
{
    internal static class RandomGenerator
    {
        private static TestTypes m_fullRandomAssignedTests = TestTypes.None;
        private static TestTypes m_randomKeyAssignedTests = TestTypes.None;
        private static TestTypes m_randomInputAssignedTests = TestTypes.None;

        static RandomGenerator()
        {
            m_fullRandomAssignedTests = Configuration.RandomGeneratorMapping;
            m_randomKeyAssignedTests = Configuration.RandomKeyGeneratorMapping;
            m_randomInputAssignedTests = Configuration.RandomInputGeneratorMapping;
            //m_AssignedTests = ConfigurationSettings.AppSettings["randomGeneratorMapping"].ToEnum(TestTypes.None);
        }

        internal static List<EncriptionData> InvokeGeneratedKeys(int reqDataForEachKey, int reqKeysCount, IEncriptable Algorithm)
        {
            List<EncriptionData> output = new List<EncriptionData>();
            for (int i = 0; i < reqKeysCount; i++)
            {
                byte[] key = new byte[Algorithm.KeySize];
                SequenceGenerator.Invoke(key);
                output.AddRange(InvokeFixedKey(reqDataForEachKey, Algorithm, key, UniqueKeyIdGenerator.Get()));
            }

            return output;
        }

        internal static List<EncriptionData> InvokeFixedKey(int reqDataCount, IEncriptable Algorithm, byte[] key, int? keyId = null)
        {
            Byte[] input = new Byte[Algorithm.BlockSize];
            List<EncriptionData> output = new List<EncriptionData>();

            for (int i = 0; i < reqDataCount; i++)
            {
                SequenceGenerator.Invoke(input);
                Algorithm.Input = input;
                Algorithm.Key = key;
                Algorithm.Run();
                output.Add(new EncriptionData(input, Algorithm.Output, key, m_randomInputAssignedTests, keyId));
            }

            return output;
        }

        internal static List<EncriptionData> InvokeFixedInput(int reqDataCount, IEncriptable Algorithm, byte[] input)
        {
            Byte[] key = new Byte[Algorithm.KeySize];
            List<EncriptionData> output = new List<EncriptionData>();

            for (int i = 0; i < reqDataCount; i++)
            {
                SequenceGenerator.Invoke(key);
                Algorithm.Input = input;
                Algorithm.Key = key;
                Algorithm.Run();
                output.Add(new EncriptionData(input, Algorithm.Output, key, m_randomKeyAssignedTests));
            }

            return output;
        }

        internal static List<EncriptionData> Invoke(int reqDataCount, IEncriptable Algorithm)
        {
            Byte[] key = new Byte[Algorithm.KeySize];
            Byte[] input = new Byte[Algorithm.BlockSize];
            List<EncriptionData> output = new List<EncriptionData>();

            for (int i = 0; i < reqDataCount; i++)
            {
                try
                {
                    SequenceGenerator.Invoke(input);
                    SequenceGenerator.Invoke(key);
                    Algorithm.Input = input;
                    Algorithm.Key = key;
                    Algorithm.Run();
                    output.Add(new EncriptionData(input, Algorithm.Output, key, m_fullRandomAssignedTests));
                }
                catch (Exception)
                {
                    //Do nothing.
                }
            }

            return output;
        }
    }
}
