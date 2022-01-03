using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordLib;

namespace Verifier
{
    class Program
    {
        static void Main(string[] args)
        {
            // output file name
            string fileName = "..\\..\\..\\FileSorter\\bin\\Debug\\output.txt";

            Console.WriteLine(Utility.IsOrdered(fileName));
            Console.ReadLine();
        }
    }
}
