using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLib
{
    public class RandomFile
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
                int numberOfWords = _random.Next(4) + 1;

                for (int j = 0; j < numberOfWords; j++)
                {
                    // no less than 3 letters per word
                    int len = _random.Next(12) + 3;

                    for (int i = 0; i < len; i++)
                    {
                        if (j == 0)
                        {
                            // A-Z (65-90), a-z(97-122)
                            randomWord += Convert.ToChar(_random.Next(65, 90));

                        }

                        randomWord += Convert.ToChar(_random.Next(97, 122));
                    }

                    randomWord += " ";
                }
            }

            return randomWord.TrimEnd();
        }

        public long SameCount => _cSameCount;

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
