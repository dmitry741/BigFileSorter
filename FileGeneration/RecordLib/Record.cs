using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLib
{
    public class Record : IComparable<Record>
    {
        string _line;
        string _strPart;

        public Record()
        {
            _line = string.Empty;
        }

        public Record(string line)
        {
            _line = line;
            int pos = _line.IndexOf('.');
            _strPart = _line.Substring(pos + 2);
        }

        public Record(int numberPart, string strPart)
        {
            _line = $"{numberPart}. {strPart}";
            _strPart = strPart;
        }

        public string Line
        {
            get { return _line; }
            set
            {
                _line = value;
                int pos = _line.IndexOf('.');
                _strPart = _line.Substring(pos + 2);
            }
        }

        public string StrPart => _strPart;

        public int SizeInBytes => _line.Length + 2;

        public int CompareTo(Record other)
        {
            int strCompare = _strPart.CompareTo(other._strPart);

            if (strCompare != 0) 
            {
                return strCompare;
            }
            else
            {
                int pos1 = _line.IndexOf('.');
                int pos2 = other._line.IndexOf('.');

                if (pos1 < pos2)
                    return -1;

                if (pos1 > pos2)
                    return 1;

                int n1 = int.Parse(_line.Substring(0, pos1));                
                int n2 = int.Parse(other._line.Substring(0, pos2));

                return n1.CompareTo(n2);
            }
        }

        public override string ToString()
        {
            return Line;
        }
    }
}
