﻿using System;
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
            string inputFile = "inputGb.txt";
            string outputFile = "outputGb.txt";

            FileInfo file = new FileInfo(inputFile);
            long sizeFile = file.Length;
            long toleranceLevel = Convert.ToInt64(640 * 1024 * 1024); // 640 Mb

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (sizeFile < toleranceLevel)
            {
                SortInRam(inputFile, sizeFile, outputFile);
            }
            else
            {
                long parts = sizeFile / (512 * 1024 * 1024);
                SortBySplit(inputFile, sizeFile, outputFile, parts);
            }

            stopwatch.Stop();

            string elapsed = $"Elapsed {stopwatch.Elapsed.Minutes} min {stopwatch.Elapsed.Seconds} sec.";
            Console.WriteLine(elapsed);
            Console.ReadLine();
        }

        static void SortBySplit(string inputFile, long sizeOfInputFile, string outputFile, long parts)
        {
            int procs = Environment.ProcessorCount;
            List<Record>[] records = new List<Record>[procs];

            for (int i = 0; i < procs; i++)
            {
                records[i] = new List<Record>();
            }

            using (StreamReader sr = File.OpenText(inputFile))
            {
                int iterator = 0;
                string line;
                long size = 0;
                int partIterator = 1;
                long block = sizeOfInputFile / parts;

                while ((line = sr.ReadLine()) != null)
                {
                    var re = new Record(line);
                    size += re.SizeInBytes;
                    records[iterator % procs].Add(re);
                    iterator++;

                    if (partIterator < parts)
                    {
                        if (size > block)
                        {
                            WriteRecords($"_temp{partIterator}.txt", records);

                            for (int i = 0; i < procs; i++)
                            {
                                records[i].Clear();
                            }

                            partIterator++;
                            size = 0;
                        }
                    }
                }

                WriteRecords($"_temp{parts}.txt", records);                
            }

            // merge sorted files
            StreamReader[] streamReaders = new StreamReader[parts];
            StreamWriter sw = null;

            for(int i = 0; i < parts; i++)
            {
                streamReaders[i] = new StreamReader($"_temp{i + 1}.txt");
            }

            sw = new StreamWriter(outputFile);

            string[] lines = new string[parts];
            Record[] r = new Record[parts];

            // read the first lines
            for (int i = 0; i < parts; i++)
            {
                lines[i] = streamReaders[i].ReadLine();
                r[i] = new Record(lines[i]);
            }

            while (true)
            {
                Record minRec = null;
                int index = -1;

                for (int i = 0; i < parts; i++)
                {
                    if (lines[i] != null)
                    {
                        if (minRec != null)
                        {
                            if (r[i].CompareTo(minRec) < 0)
                            {
                                minRec = r[i];
                                index = i;
                            }
                        }
                        else
                        {
                            minRec = r[i];
                            index = i;
                        }
                    }
                }

                if (index < 0)
                    break;

                sw.WriteLine(minRec.Line);

                lines[index] = streamReaders[index].ReadLine();

                if (lines[index] != null)
                {
                    r[index].Line = lines[index];
                }
            }

            // finally block
            for (int i = 0; i < parts; i++)
            {
                streamReaders[i].Close();
            }

            sw.Close();

            for (int i = 0; i < parts; i++)
            {
                File.Delete($"_temp{i + 1}.txt");
            }
        }

        static void WriteRecords(string outputFile, List<Record>[] records)
        {
            int procs = records.Length;

            Parallel.For(0, procs, i => {
                int index = i;
                records[index].Sort();
            });

            int[] positions = new int[procs];
            Record[] recs = new Record[procs];

            for (int i = 0; i < procs; i++)
            {
                recs[i] = records[i][0];
            }

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                Record minRec;
                int index;

                while (true)
                {
                    minRec = null;
                    index = -1;

                    for (int i = 0; i < procs; i++)
                    {
                        if (positions[i] < records[i].Count)
                        {
                            if (minRec != null)
                            {
                                if (recs[i].CompareTo(minRec) < 0)
                                {
                                    minRec = recs[i];
                                    index = i;
                                }
                            }
                            else
                            {
                                minRec = recs[i];
                                index = i;
                            }
                        }
                    }

                    if (index < 0)
                        break;

                    sw.WriteLine(minRec.Line);

                    positions[index]++;

                    if (positions[index] < records[index].Count)
                    {
                        recs[index] = records[index][positions[index]];
                    }
                }
            }
        }

        static void SortInRam(string inputFile, long sizeOfInputFile, string outputFile)
        {
            const int cTolarance = 2; // Mb

            if (sizeOfInputFile < cTolarance * 1024 * 1024) // less than tolarance Mb (for short files)
            {
                List<Record> records = new List<Record>();

                using (StreamReader sr = File.OpenText(inputFile))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
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
                int procs = Environment.ProcessorCount;
                List<Record>[] records = new List<Record>[procs];

                for (int i = 0; i < procs; i++)
                {
                    records[i] = new List<Record>();
                }

                using (StreamReader sr = File.OpenText(inputFile))
                {
                    int iterator = 0;
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        records[iterator % procs].Add(new Record(line));
                        iterator++;
                    }
                }

                WriteRecords(outputFile, records);
            }
        }
    }
}
