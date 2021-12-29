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

        public Record()
        {
            _strPart = string.Empty;
            _numberPart = 0;
        }

        public Record(string line)
        {
            string[] ar = line.Split('.');
            _numberPart = int.Parse(ar[0]);
            _strPart = ar[1].TrimStart();
        }

        public Record(int numberPart, string strPart)
        {
            _numberPart = numberPart;
            _strPart = strPart;
        }

        public string Line
        {
            get { return $"{_numberPart}. {_strPart}"; }            
            set
            {
                string[] ar = value.Split('.');
                _numberPart = int.Parse(ar[0]);
                _strPart = ar[1].TrimStart();
            }
        }

        public int SizeInBytes => Line.Length + 2;

        public int CompareTo(Record other)
        {
            int strCompare = _strPart.CompareTo(other._strPart);
            return (strCompare != 0) ? strCompare : _numberPart.CompareTo(other._numberPart);
        }

        public override string ToString()
        {
            return Line;
        }
    }
}
