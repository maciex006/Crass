using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTests")]
namespace CryptoAssessment.Tests
{
    internal static partial class RandomnessTestUtil
    {
        // Recommended n = 100;
        static private readonly double rootTwo = 1.414213562373095;

        internal static double FrequencyTest(byte[] byteArray, int? inputLength = null)
        {
            int sum = 0;
            int nBits = byteArray.Length * 8;
            int n = inputLength ?? nBits;

            for (int i = 0; i < byteArray.Length; ++i)
            {
                sum += (2 * CountOnes(byteArray[i]) - 8);
            }

            sum += (nBits - n); // ignorujemy nadmiarowe zera dodane na końcu.

            double testStat = Math.Abs(sum) / Math.Sqrt(n);
            return AdditionalFunctions.Erfc(testStat / rootTwo);
        }

        private static int CountOnes(byte input)
        {
            int count = 0;
            while (input != 0)
            {
                count++;
                input &= (byte)(input - 1);
            }
            return count;
        }
    }
}
