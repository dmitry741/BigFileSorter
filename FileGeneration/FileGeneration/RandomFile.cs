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
        Random _random;

        public RandomFile()
        {
            _random = new Random();
        }

        private string GetRandomWord()
        {
            int len = _random.Next(20) + 3; // no less 3 letter per word
            string randomWord = string.Empty;

            // A-Z (65-90) a-z(97-122)
            randomWord += Convert.ToChar(_random.Next(65, 90));

            for (int i = 0; i < len - 1; i++)
            {
                randomWord += Convert.ToChar(_random.Next(97, 122));
            }

            return randomWord;
        }

        public Record GetNewRecord()
        {
            int number = _random.Next(int.MaxValue - 1) + 1; // to avoid zero value
            string word = GetRandomWord();

            return new Record(number, word);
        }
    }
}
