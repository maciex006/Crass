using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoAssessment;
using CryptoAssessment.Tests;
using System.Security.Cryptography;

namespace OutSideNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            Analizer desAnalizer = new Analizer(new DesEncriptor(8, 8));
            desAnalizer.Run(TestTypes.FrequencyTest | TestTypes.BlockFrequencyTest | TestTypes.RunsTest);
            desAnalizer.GetResults();
            //Console.WriteLine(RandomnessTestUtil.FrequencyTest(
            //    new byte[] { 201, 15, 218, 162, 33, 104, 194, 52, 196, 198, 98, 139, 128 }, 
            //    100));


            Console.ReadKey();
        }
    }

    class DesEncriptor : IEncriptable
    {
        public int BlockSize { get; private set; }
        public int KeySize { get; private set; }
        public byte[] Input { get; set; }
        public byte[] Output { get; private set; }
        public byte[] Key { get; set; }

        public DesEncriptor(int blockSizeBytes, int keySizeBytes)
        {
            BlockSize = blockSizeBytes;
            KeySize = keySizeBytes;
        }

        public void Run()
        {
            DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();

            desCSP.Key = Key;
            //desCSP.GenerateKey();
            desCSP.Padding = PaddingMode.None;
            desCSP.Mode = CipherMode.ECB;
            desCSP.GenerateIV();

            ICryptoTransform xfrm = desCSP.CreateEncryptor();
            Output = xfrm.TransformFinalBlock(Input, 0, Input.Length);
        }

        public IEncriptable Duplicate()
        {
            return new DesEncriptor(BlockSize, KeySize);
        }
    }
}
