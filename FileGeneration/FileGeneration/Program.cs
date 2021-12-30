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
            string outputFileName = "..\\..\\..\\FileSorter\\bin\\Debug\\inputGb.txt";

            // size of file in Mb
            int fileSizeInMb = 1024; // size of file measured in Mb, f.e. 1024 is equal to 1 Gb

            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                RandomFile randomFile = new RandomFile(fileSizeInMb * 1024 * 1024);
                long size = 0;
                long divider = 1 << 20; // to Mb

                while (size / divider < fileSizeInMb)
                {
                    Record record = randomFile.GetNewRecord(size);
                    size += record.SizeInBytes;
                    sw.WriteLine(record.Line);
                }
            }           
        }
    }
}
