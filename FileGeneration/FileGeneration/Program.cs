using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordLib;
using System.IO;

namespace FileGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            // output file name
            string outputFileName = "..\\..\\..\\FileSorter\\bin\\Debug\\input2Mb.txt";

            // size of file in Mb
            int fileSizeInMb = 2; // size of file measured in Mb, f.e. 1024 is equal to 1 Gb

            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                long fileSize = fileSizeInMb * Utility.ToMb();
                RandomFile randomFile = new RandomFile(fileSize);
                long size = 0;

                while (size < fileSize)
                {
                    Record record = randomFile.GetNewRecord(size);
                    size += record.SizeInBytes;
                    sw.WriteLine(record.Line);
                }
            }           
        }
    }
}
