using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Generators
{
    static class UniqueKeyIdGenerator
    {
        static volatile private int idCounter;

        static internal int Get()
        {
            return idCounter++;
        }
    }
}
