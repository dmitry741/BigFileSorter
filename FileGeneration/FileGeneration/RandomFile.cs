using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordLib;

namespace FileGeneration
{
    class RandomFile
    {
        readonly Random _random;
        readonly long _fileSize;
        readonly long _cSameCount; // the count of the same string part
        long _sameIterator;

        public RandomFile(long fileSizeInBytes)
        {
            _random = new Random();
            _fileSize = fileSizeInBytes;
            _cSameCount = 8;
            _sameIterator = 0;
        }

        private string GetRandomWord(long curFileSize)
        {
            string randomWord = string.Empty;

            if (curFileSize > _sameIterator * _fileSize / _cSameCount)
            {
                randomWord = "Apple";
                _sameIterator++;
            }
            else
            {
                // no less than 3 letters per word
                int len = _random.Next(16) + 3;

                // A-Z (65-90) a-z(97-122)
                randomWord += Convert.ToChar(_random.Next(65, 90));

                for (int i = 0; i < len - 1; i++)
                {
                    randomWord += Convert.ToChar(_random.Next(97, 122));
                }
            }           

            return randomWord;
        }

        public void Reset()
        {
            _sameIterator = 0;
        }

        public Record GetNewRecord(long curFileSize)
        {
            int number = _random.Next(int.MaxValue - 1) + 1; // to avoid zero value
            string word = GetRandomWord(curFileSize);

            return new Record(number, word);
        }
    }
}
