using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyrptoAssessment.Tests;

namespace CyrptoAssessment
{
    public sealed class TestResult
    {
        private Test m_CurrentTest;

        internal int NumOfSamples { get; private set; }
        internal TimeSpan ExecutionTime { get; private set; }
        internal double Result { get; private set; }

        internal TestResult(Test test, int NumOfSamples, TimeSpan ExecutionTime, double Result)
        {
            m_CurrentTest = test;
            this.NumOfSamples = NumOfSamples;
            this.ExecutionTime = ExecutionTime;
            this.Result = Result;
        }

        public override string ToString()
        {
            return "\n" + m_CurrentTest.ToString() + "\n" +
                "Analized " + NumOfSamples + " samples. \n" +
                "Execution time " + ExecutionTime.ToString() + ". \n" +
                "Completed with result = " + Result + ". \n";
        }
    }
}
