using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TextProcessor.Models;

namespace TextProcessor.Tests
{
    [TestClass]
    public class TextProcessorTests
    {
        [TestMethod]
        public void ProcessText_RemovesShortWords()
        {
            var model = new TextProcessorModel();
            string inputFile = "input.txt";
            string outputFile = "output.txt";
            int minWordLength = 3;
            bool removePunctuation = false;
            string inputText = "This is a test file with short words.";
            string expectedOutputText = "This test file with short words.";

            File.WriteAllText(inputFile, inputText);

            _ = model.ProcessText(inputFile, outputFile, minWordLength, removePunctuation);

            string actualOutputText = File.ReadAllText(outputFile);
            Assert.AreEqual(expectedOutputText, actualOutputText);

            File.Delete(inputFile);
            File.Delete(outputFile);
        }

        [TestMethod]
        public void ProcessText_RemovesPunctuation()
        {
            var model = new TextProcessorModel();
            string inputFile = "input.txt";
            string outputFile = "output.txt";
            int minWordLength = 1;
            bool removePunctuation = true;
            string inputText = "This, is! a test file with punctuation.";
            string expectedOutputText = "This is a test file with punctuation";

            File.WriteAllText(inputFile, inputText);

            _ = model.ProcessText(inputFile, outputFile, minWordLength, removePunctuation);

            string actualOutputText = File.ReadAllText(outputFile);
            Assert.AreEqual(expectedOutputText, actualOutputText);

            // Clean up
            File.Delete(inputFile);
            File.Delete(outputFile);
        }
    }
}

