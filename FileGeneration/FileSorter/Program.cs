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
            // path to input file
            string inputFile = "inputGb.txt";

            // path to output file
            string outputFile = "output.txt";

            FileInfo file = new FileInfo(inputFile);
            long sizeFile = file.Length;
            long toleranceLevel = Convert.ToInt64(513 * Utility.ToMb());

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (sizeFile < toleranceLevel)
            {
                SortInRam(inputFile, outputFile);
            }
            else
            {
                long parts = Math.Max(sizeFile / (256 * Utility.ToMb()), 2);
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
                    records[iterator % procs].Add(re);
                    size += re.SizeInBytes;
                    iterator++;

                    if (partIterator < parts)
                    {
                        if (size > block)
                        {
                            WriteRecords($"_t{partIterator}.txt", records);

                            for (int i = 0; i < procs; i++)
                            {
                                records[i].Clear();
                            }

                            partIterator++;
                            iterator = 0;
                            size = 0;
                        }
                    }
                }

                WriteRecords($"_t{parts}.txt", records);
            }

            StreamReader[] streamReaders = new StreamReader[parts];
            StreamWriter sw = null;

            //try
            //{
            // merge sorted files

            for (int i = 0; i < parts; i++)
            {
                streamReaders[i] = new StreamReader($"_t{i + 1}.txt");
            }

            sw = new StreamWriter(outputFile);

            Record[] r = new Record[parts];

            // read the first lines
            for (int i = 0; i < parts; i++)
            {
                r[i] = new Record(streamReaders[i].ReadLine());
            }

            Record minRec;
            int index;

            while (true)
            {
                minRec = null;
                index = -1;

                for (int i = 0; i < parts; i++)
                {
                    if (r[i] != null)
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
                string line = streamReaders[index].ReadLine();

                if (line != null)
                    r[index].Line = line;
                else
                    r[index] = null;
            }
            //}
            //catch(Exception e)
            //{
            //Console.WriteLine(e.Message); 
            //}
            //finally
            //{
            for (int i = 0; i < parts; i++)
            {
                streamReaders[i].Close();
            }

            sw.Close();

            for (int i = 0; i < parts; i++)
            {
                File.Delete($"_t{i + 1}.txt");
            }
            //}
        }

        static void WriteRecords(string outputFile, List<Record>[] records)
        {
            int procs = records.Length;
            Record[][] arRecs = new Record[procs][];

            Parallel.For(0, procs, i => {
                int index = i;
                arRecs[index] = records[index].ToArray();
                Array.Sort(arRecs[index]);
            });

            int[] positions = new int[procs];

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
                        if (positions[i] < arRecs[i].Length)
                        {
                            if (minRec != null)
                            {
                                if (arRecs[i][positions[i]].CompareTo(minRec) < 0)
                                {
                                    minRec = arRecs[i][positions[i]];
                                    index = i;
                                }
                            }
                            else
                            {
                                minRec = arRecs[i][positions[i]];
                                index = i;
                            }
                        }
                    }

                    if (index < 0)
                        break;

                    sw.WriteLine(minRec.Line);
                    positions[index]++;
                }
            }
        }

        static void WriteRecord(string outputFile, List<Record> record)
        {
            var ordered = record.AsParallel().OrderBy(x => x);

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (Record r in ordered)
                {
                    sw.WriteLine(r.Line);
                }
            }
        }       

        static void SortInRam(string inputFile, string outputFile)
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

            WriteRecord(outputFile, records);
        }
    }
}
