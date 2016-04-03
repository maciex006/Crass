using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyrptoAssessment
{
    /// <summary>
    ///     Interface representing functionality that must be provided by 
    ///     cryptographic algorithm to be handled by assessment classes.
    /// </summary>
    public interface IEncriptable
    {
        int BlockSize { get; }
        int KeySize { get; }
        byte[] Input { set; }
        byte[] Output { get; }
        byte[] Key { get; set; }
        void Run();
        IEncriptable Duplicate();
    }
}
