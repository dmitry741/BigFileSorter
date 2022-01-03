using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLib
{
    public class Utility
    {
        public static bool IsOrdered(IEnumerable<Record> records)
        {
            IEnumerator<Record> enumerator = records.GetEnumerator();

            enumerator.MoveNext();
            var prev = enumerator.Current;
            bool result = true;

            while (enumerator.MoveNext())
            {
                var next = enumerator.Current;

                if (prev.CompareTo(next) > 0)
                {
                    result = false;
                    break;
                }

                prev = next;
            }

            return result;
        }

        public static bool IsOrdered(string path)
        {
            bool result = true;

            using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
            {
                Record prev = new Record(sr.ReadLine());
                Record next = new Record();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    next.Line = line;

                    if (prev.CompareTo(next) > 0)
                    {
                        result = false;
                        break;
                    }

                    prev.Line = next.Line;
                }
            }

            return result;
        }

        public static void Merge(List<Record> r1, List<Record> r2, out List<Record> result)
        {
            result = new List<Record>();

            IEnumerator<Record> en1 = r1.GetEnumerator();
            IEnumerator<Record> en2 = r2.GetEnumerator();

            en1.MoveNext();
            en2.MoveNext();

            Record record1 = en1.Current;
            Record record2 = en2.Current;

            while (true)
            {
                if (record1.CompareTo(record2) < 0)
                {
                    result.Add(record1);

                    if (en1.MoveNext())
                    {
                        record1 = en1.Current;
                    }
                    else
                    {
                        result.Add(record2);

                        while (en2.MoveNext())
                        {
                            result.Add(en2.Current);
                        }

                        break;
                    }
                }
                else
                {
                    result.Add(record2);

                    if (en2.MoveNext())
                    {
                        record2 = en2.Current;
                    }
                    else
                    {
                        result.Add(record1);

                        while (en1.MoveNext())
                        {
                            result.Add(en1.Current);
                        }

                        break;
                    }
                }                
            }
        }
    }
}
