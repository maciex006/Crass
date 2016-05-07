using CryptoAssessment.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment
{
    internal sealed class TestUtil
    {
        internal List<Test> Init(TestTypes tests, List<EncriptionData> data)
        {
            List<Test> initializedTests = new List<Test>();

            if ((tests & TestTypes.KeyLength) != 0)
            {
                //Test bitBalanceTest = new BitBalanceTest(data.Where(x => (x.TestType & TestTypes.KeyLength) != 0));
            }

            if ((tests & TestTypes.Nonlinearity) != 0)
            {
                initializedTests.Add(new NonlinearityTest(data.Where(x => (x.TestType & TestTypes.Nonlinearity) != 0)));
            }

            if ((tests & TestTypes.RunsTest) != 0)
            {
                initializedTests.Add(new RunsTest(data.Where(x => (x.TestType & TestTypes.FrequencyTest) != 0)));
            }

            if ((tests & TestTypes.FrequencyTest) != 0)
            {
                initializedTests.Add(new FrequencyTest(data.Where(x => (x.TestType & TestTypes.FrequencyTest) != 0)));
            }

            if ((tests & TestTypes.BlockFrequencyTest) != 0)
            {
                initializedTests.Add(new BlockFrequencyTest(data.Where(x => (x.TestType & TestTypes.FrequencyTest) != 0)));
            }

            if ((tests & TestTypes.SacInput) != 0)
            {
                //Test bitBalanceTest = new BitBalanceTest(data.Where(x => (x.TestType & TestTypes.Randomness) != 0));
            }

            if ((tests & TestTypes.SacKey) != 0)
            {
                //Test bitBalanceTest = new BitBalanceTest(data.Where(x => (x.TestType & TestTypes.Randomness) != 0));
            }
            // Tutaj uzupełnić też pola list EncriptionData w każdym z testów (jakoś sensownie żeby nie powtarzać instrukcji)
            //TODO: implement.
            return initializedTests;
        }

        internal void Run(List<Test> tests)
        {
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            List<Task> tasks = new List<Task>();


            foreach (var test in tests)
            {
                Task task = new Task(() => test.Perform());
                tasks.Add(task);
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());

            //foreach (var test in tests)
            //{
            //    test.Perform();
            //}
#if DEBUG
            sw.Stop();
            Console.WriteLine("Testing time: " + sw.Elapsed);
#endif
        }
    }
}
