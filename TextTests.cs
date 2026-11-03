using Lab4;
using NUnit.Framework;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Lab4.Tests
{
    [TestFixture]
    public class TextTests
    {
        
        [Test]
        public void Parse_ValidInput_CreatesCorrectNumberOfSentences()
        {
            string sampleText = "Привет мир! Как твои дела? Всё отлично.";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var reader = new StreamReader(stream);

            Text text = TextParcer.Parse(reader);

            Assert.That(text.Sentences.Count, Is.EqualTo(3));
        }

        
        [Test]
        public void SenchInQuestionSent_ReturnsUniqueWordsOfGivenLengthFromQuestionsOnly()
        {
            string sampleText = "Это обычное предложение. Что тут происходит? Как дела?";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var reader = new StreamReader(stream);
            Text text = TextParcer.Parse(reader);

            var result = text.SenchInQuestionSent(3);

            Assert.That(result, Has.Exactly(3).Items);
            Assert.That(result, Contains.Item("что"));
            Assert.That(result, Contains.Item("тут"));
            Assert.That(result, Contains.Item("как"));
        }

        
        [Test]
        public void RemoveForLength_RemovesWordsStartingWithConsonant()
        {
            string sampleText = "Кот и имя дом.";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var reader = new StreamReader(stream);

            Text text = TextParcer.Parse(reader);
            text.RemoveForLength(3);

            string resultText = text.ToString();

            Assert.That(resultText, Does.Not.Contain("Кот"));
            Assert.That(resultText, Does.Not.Contain("дом"));
            Assert.That(resultText, Does.Contain("имя"));
        }

        
        
        [Test]
        public void Change_ReplacesWordsOfTargetLengthInSpecificSentence()
        {
            string sampleText = "Мама мыла пол. Папа мыл окно.";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var reader = new StreamReader(stream);

            Text text = TextParcer.Parse(reader);

            text.Change("REPLACED", 0, 4);

            Assert.That(text.Sentences[0].ToString(), Is.EqualTo("REPLACED REPLACED пол."));
            Assert.That(text.Sentences[1].ToString(), Is.EqualTo("Папа мыл окно."));
        }

        
        [Test]
        public void OutForWordCount_SortsSentencesByWordCountDescending()
        {
            string sampleText = "Одно слово. Тут три слова здесь. Два слова.";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var reader = new StreamReader(stream);

            Text text = TextParcer.Parse(reader);
            text.OutForWordCount();

            Assert.That(text.Sentences[0].WordCount, Is.EqualTo(4));
            Assert.That(text.Sentences[1].WordCount, Is.EqualTo(2)); 
            Assert.That(text.Sentences[2].WordCount, Is.EqualTo(2));
        }

        
        [Test]
        public void RemoveStopWord_RemovesMatchingWordsFromText()
        {
            string sampleText = "Привет и добро пожаловать в мир.";
            string stopWordsList = "и\r\nв";

            using var textStream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var textReader = new StreamReader(textStream);
            Text text = TextParcer.Parse(textReader);

            using var stopStream = new MemoryStream(Encoding.UTF8.GetBytes(stopWordsList));
            using var stopReader = new StreamReader(stopStream);

            text.RemoveStopWord(stopReader);

            string result = text.ToString();
            Assert.That(result, Does.Not.Contain(" и "));
            Assert.That(result, Does.Not.Contain(" в "));
            Assert.That(result, Is.EqualTo("Привет добро пожаловать мир."));
        }

        [Test]
        public void GetConcordance_BuildsCorrectConcordance()
        {
            string sampleText = "Peter Piper picked a peck. A peck Peter Piper picked.";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleText));
            using var reader = new StreamReader(stream);

            Text text = TextParcer.Parse(reader);

            string result = text.GetConcordance();

            Assert.That(result, Does.Contain("peter"));
            Assert.That(result, Does.Contain("piper"));
            Assert.That(result, Does.Contain("peck"));

            
            Assert.That(result, Does.Match(@"peter\.+2: 1 2"));
            Assert.That(result, Does.Match(@"piper\.+2: 1 2"));
            Assert.That(result, Does.Match(@"picked\.+2: 1 2"));

            
            int indexOfPeck = result.IndexOf("peck");
            int indexOfPeter = result.IndexOf("peter");
            Assert.That(indexOfPeck, Is.LessThan(indexOfPeter));
        }
    }
}