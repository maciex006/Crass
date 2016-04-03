using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyrptoAssessment
{
    /// <summary>
    ///     Enumeration representing various types of tests.
    /// </summary>
    public enum TestTypes
    {
        None = 0,
        KeyLength = 1,
        Randomness = 2,
        BitBalance = 4,
        Nonlinearity = 8,
        SacInput = 16,
        SacKey = 32,
        All = 63
    }
}
