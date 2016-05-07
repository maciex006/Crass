using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoAssessment.Tests;

namespace CryptoAssessment
{
    public sealed class TestResult
    {
        private Test m_CurrentTest;

        internal int NumOfSamples { get; private set; }
        internal TimeSpan ExecutionTime { get; private set; }
        internal double Result { get; private set; }
        internal bool Passed { get; private set; }
        internal string Comment { get; private set; }

        internal TestResult(Test test, int NumOfSamples, TimeSpan ExecutionTime, bool passed, double result, string comment = " - ")
        {
            m_CurrentTest = test;
            this.NumOfSamples = NumOfSamples;
            this.ExecutionTime = ExecutionTime;
            this.Passed = passed;
            this.Result = result;
            this.Comment = comment;
        }

        public override string ToString()
        {
            return "\n" + m_CurrentTest.ToString() + "\n" +
                "Analized " + NumOfSamples + " samples. \n" +
                "Execution time " + ExecutionTime.ToString() + ". \n" +
                "Completed with result = " + (Passed ? "Passed" : "Failed") + "("+ Result + "). \n" +
                Comment + "\n";
        }
    }
}
