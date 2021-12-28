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
            string outputFileName = "output.txt";
            int fileSizeImMb = 4;

            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                RandomFile randomFile = new RandomFile();
                long size = 0;

                while (size / (1024 * 1024) < fileSizeImMb)
                {
                    Record record = randomFile.GetNewRecord();
                    size += record.SizeInBytes;
                    sw.WriteLine(record.Line);
                }
            }



            
        }
    }
}
