using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor.Models
{
    public class TextProcessorModel
    {
        public async Task ProcessText(string inputFile, string outputFile, int minWordLength, bool removePunctuation)
        {
            string text = File.ReadAllText(inputFile);
            text = RemoveShortWords(text, minWordLength);
            if (removePunctuation)
            {
                text = RemovePunctuation(text);
            }
            File.WriteAllText(outputFile, text);
            await Task.Delay(1);
        }

        private string RemoveShortWords(string text, int minWordLength)
        {
            return string.Join(" ", text.Split()
                .Where(word => word.Length >= minWordLength));
        }

        private string RemovePunctuation(string text)
        {
            return new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
        }
    }
}
