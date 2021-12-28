using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RecordLib;

namespace FileSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "input.txt";
            string outputFile = "output.txt";

            FileInfo file = new FileInfo(inputFile);
            long sizeFile = file.Length;
            long toleranceLevel = 2 * Convert.ToInt64(1024 * 1024 * 1024); // 2Gb

            if (sizeFile < toleranceLevel) // less than 2 Gb
            {
                SortInRam(inputFile, sizeFile, outputFile);
            }
            else
            {

            }

        }

        static void SortInRam(string inputFile, long sizeOfInputFile, string outputFile)
        {
            const int cTolarance = 2; // Mb

            if (sizeOfInputFile < cTolarance * 1024 * 1024) // less than tolarance Mb
            {
                List<Record> records = new List<Record>();

                using (StreamReader sr = new StreamReader(inputFile))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        records.Add(new Record(line));
                    }
                }

                records.Sort();

                using (StreamWriter sw = new StreamWriter(outputFile))
                {
                    foreach(Record record in records)
                    {
                        sw.WriteLine(record.Line);
                    }
                }
            }
            else
            {
                // TODO:
            }
        }
    }
}
