using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Tests
{
    internal abstract class Test
    {
        private TestTypes _type;

        internal TestTypes Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (IsSingleTest(value))
                {
                    throw new Exception("Cannot assign multiple test types to the test instance.");
                }
                else
                {
                    _type = value;
                }
            }
        }

        internal TestResult Result { get; set; }
        public IEnumerable<EncriptionData> Pairs { protected get; set; }
        internal abstract void Perform();

        public static bool IsSingleTest(TestTypes type)
        {
            int val = (int)type;
            return (val != 1 && val % 2 != 0) ? true : false;
        }
    }
}
