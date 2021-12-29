﻿using System;
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
            string outputFileName = "..\\..\\..\\FileSorter\\bin\\Debug\\input.txt";
            int fileSizeImMb = 512;

            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                RandomFile randomFile = new RandomFile(fileSizeImMb * 1024 * 1024);
                long size = 0;
                long divider = 1 << 20; // to Mb

                while (size / divider < fileSizeImMb)
                {
                    Record record = randomFile.GetNewRecord(size);
                    size += record.SizeInBytes;
                    sw.WriteLine(record.Line);
                }
            }           
        }
    }
}
