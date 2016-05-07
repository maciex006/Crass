using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment
{
    /// <summary>
    ///     Enumeration representing various types of tests.
    /// </summary>
    public enum TestTypes
    {
        None = 0,
        KeyLength = 1,
        Randomness = 2,
        Nonlinearity = 4,
        SacInput = 8,
        SacKey = 16,
        FrequencyTest = 32,
        BlockFrequencyTest = 64,
        RunsTest = 128,
        All = 255
    }
}
