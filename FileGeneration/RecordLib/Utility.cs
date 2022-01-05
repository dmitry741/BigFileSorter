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

        public static int ToMb()
        {
            return 1 << 20;
        }
    }
}
