using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CyrptoAssessment.Generators
{
    internal sealed class EncriptionDataGenerator
    {
        private IEncriptable m_Algorithm;

        internal EncriptionDataGenerator(IEncriptable alg)
        {
            m_Algorithm = alg;
        }

        internal IEnumerable<EncriptionData> Invoke(TestTypes tests)
        {
            // Przemyslec czemu byte[] idzie po referencji a nie value.
            // Może dodać możliwość wyboru między wywołaniem multithread a zwykłym.

#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            List<Task<List<EncriptionData>>> tasks = new List<Task<List<EncriptionData>>>();

            if ((tests & TestTypes.BitBalance) != 0)
            {
                Task<List<EncriptionData>> task = new Task<List<EncriptionData>>(InvokeBitBalance);
                tasks.Add(task);
                task.Start();
            }

            if ((tests & TestTypes.Nonlinearity) != 0)
            {
                Task<List<EncriptionData>> task = new Task<List<EncriptionData>>(InvokeNonlinearity);
                tasks.Add(task);
                task.Start();
            }

            if ((tests & TestTypes.SacInput) != 0)
            {
                Task<List<EncriptionData>> task = new Task<List<EncriptionData>>(InvokeSacInput);
                tasks.Add(task);
                task.Start();
            }

            if ((tests & TestTypes.SacKey) != 0)
            {
                Task<List<EncriptionData>> task = new Task<List<EncriptionData>>(InvokeSacKey);
                tasks.Add(task);
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());
#if DEBUG
            sw.Stop();
            Console.WriteLine("Generation time: " + sw.Elapsed);
#endif
            return tasks.SelectMany(task => task.Result);
        }

        private List<EncriptionData> InvokeBitBalance()
        {
            return RandomGenerator.Invoke(Configuration.BalanceGenPairs, m_Algorithm.Duplicate());
        }

        private List<EncriptionData> InvokeNonlinearity()
        {
            return RandomGenerator.InvokeGeneratedKeys(
                    Configuration.NonlinGenPars,
                    Configuration.NonlinGenKeys,
                    m_Algorithm.Duplicate());
        }

        private List<EncriptionData> InvokeSacInput()
        {
            return SacGenerator.InvokeGeneratedKeys(Configuration.SacInputGenKeys, m_Algorithm.Duplicate());
        }

        private List<EncriptionData> InvokeSacKey()
        {
            return SacGenerator.InvokeGeneratedInputs(Configuration.SacKeyGenInputs, m_Algorithm.Duplicate());
        }
    }
}
