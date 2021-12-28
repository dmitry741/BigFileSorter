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
            string inputFile = "input1.txt";
            string outputFile = "output1.txt";

            FileInfo file = new FileInfo(inputFile);
            long sizeFile = file.Length;
            long toleranceLevel = 2 * Convert.ToInt64(1024 * 1024 * 1024); // 2Gb

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (sizeFile < toleranceLevel) // less than 2 Gb
            {
                SortInRam(inputFile, sizeFile, outputFile);
            }
            else
            {

            }

            stopwatch.Stop();

            string elapsed = $"Elapsed {stopwatch.Elapsed.Minutes} min {stopwatch.Elapsed.Seconds} sec.";
            Console.WriteLine(elapsed);
            Console.ReadLine();
        }

        static void SortInRam(string inputFile, long sizeOfInputFile, string outputFile)
        {
            const int cTolarance = 2; // Mb

            /*if (sizeOfInputFile < cTolarance * 1024 * 1024) // less than tolarance Mb
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
            else*/
            {
                int procs = Environment.ProcessorCount;
                List<Record>[] records = new List<Record>[procs];

                for (int i = 0; i < procs; i++)
                {
                    records[i] = new List<Record>();
                }

                using (StreamReader sr = new StreamReader(inputFile))
                {
                    int iterator = 0;

                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        records[iterator % procs].Add(new Record(line));
                        iterator++;
                    }
                }

                /*
                Action[] actions = new Action[procs];

                for (int i = 0; i < procs; i++)
                {
                    actions[i] = new Action(delegate () { records[i].Sort(); });
                }

                Parallel.Invoke(actions);*/

                for (int i = 0; i < procs; i++)
                {
                    records[i].Sort();
                }

                int[] positions = new int[procs];

                using (StreamWriter sw = new StreamWriter(outputFile))
                {
                    while (true)
                    {
                        Record minRec = null;
                        int index = -1;

                        for (int i = 0; i < procs; i++)
                        {
                            if (positions[i] < records[i].Count)
                            {
                                if (minRec != null)
                                {
                                    int compare = records[i][positions[i]].CompareTo(minRec);

                                    if (compare < 0)
                                    {
                                        minRec = records[i][positions[i]];
                                        index = i;
                                    }
                                }
                                else
                                {
                                    minRec = records[i][positions[i]];
                                    index = i;
                                }
                            }
                        }

                        if (index < 0)
                            break;

                        sw.WriteLine(minRec);

                        positions[index]++;
                    }
                }





            }
        }
    }
}
