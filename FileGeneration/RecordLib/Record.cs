using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLib
{
    public class Record : IComparable<Record>
    {
        string _strPart;
        int _numberPart;
        string _line;

        public Record(string line)
        {
            _line = line;
            string[] ar = line.Split('.');
            _numberPart = int.Parse(ar[0]);
            _strPart = ar[1].TrimStart();
        }

        public string Line => _line;

        public int CompareTo(Record other)
        {
            int strCompare = _strPart.CompareTo(other._strPart);
            return (strCompare != 0) ? strCompare : _numberPart.CompareTo(other._numberPart);
        }
    }
}
