using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment
{
    /// <summary>
    ///     Class representing a plaintext, cyphertext pair, along with key and information
    ///     of what test type is using that pair.
    /// </summary>
    internal sealed class EncriptionData
    {
        internal EncriptionData(byte[] plainText, byte[] cypherText, byte[] key, TestTypes testType, int? uniqueKeyId = null)
        {
            PlainText = (byte[])plainText.Clone();
            CypherText = (byte[])cypherText.Clone();
            Key = (byte[])key.Clone();
            TestType = testType;
            UniqueKeyId = uniqueKeyId;
        }

        internal byte[] PlainText { get; private set; }
        internal byte[] CypherText { get; private set; }
        internal byte[] Key { get; private set; }
        internal int? UniqueKeyId { get; private set; }
        internal TestTypes TestType { get; private set; }
    }
}
