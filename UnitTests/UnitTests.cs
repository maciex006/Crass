using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyrptoAssessment;
using CyrptoAssessment.Tests;

namespace UnitTests
{
    class UnitTests
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RandomnessTestUtil.FrequencyTest(
                new byte[] { 201, 15, 218, 162, 33, 104, 194, 52, 196, 198, 98, 139, 128 }, 
                100));


            Console.ReadKey();
        }
    }
}
