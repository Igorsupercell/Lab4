using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Lab4
{
    [XmlRoot("text")]
    public class Text 
    {
        [XmlElement("sentence")]
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
        public Text()
        {
            Sentences.Add(new Sentence());
        } 
        public void Add(Sentence sent) { Sentences.Add(sent); }

        public override string ToString()
        {
            return string.Join(" ", Sentences.ToArray());
        }

        public void OutForWordCount()
        {
            int n =Sentences.Count;
            for (int i = 0; i < n-1; i++)
            {
                for (int j = 0; j < n-1-i; j++)
                {

                    int currentWords = Sentences[j].WordCount;
                    int nextWords = Sentences[j + 1].WordCount; 
                    if (currentWords < nextWords)
                    {
                        (Sentences[j],Sentences[j + 1]) = (Sentences[j + 1], Sentences[j]);
                    }
                }
            }
        }

        public void OutForSentenceLength()
        {
            int n = Sentences.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    int currentWords = Sentences[j].Length;
                    int nextWords = Sentences[j + 1].Length;
                    if (currentWords < nextWords)
                    {
                        (Sentences[j], Sentences[j + 1]) = (Sentences[j + 1], Sentences[j]);
                    }
                }
            }
            
        }

        public List<string> SenchInQuestionSent( int LengthOfWord) 
        {
            HashSet<string> uniqueWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var sentence in Sentences) 
            {
                if (sentence.IsQuestion)
                {
                    foreach (var word in sentence.Items.OfType<Word>())
                    {
                        if (word.Content.Length == LengthOfWord) uniqueWords.Add(word.Content.ToLower());
                    }
                }
            }
            return uniqueWords.ToList();
        }

        public void RemoveForLength(int LengthOfWord)
        {

            foreach (var sentence in Sentences)
            {
                var wordsToRemove = new List<Token>();
                
                foreach (var word in sentence.Items.OfType<Word>())
                {
                    if (word.Content.Length == LengthOfWord && (Word.ENGsogl.Contains(char.ToLower(word.Content[0])) 
                        || Word.RUSsogl.Contains(char.ToLower(word.Content[0])))) 
                     {
                        wordsToRemove.Add(word);
                     }
                }

                foreach(var word in wordsToRemove)
                {
                    sentence.Items.Remove(word);
                }
                
            }
        }
        
        public void Change(string STR, int sentenceIndex, int targetLength)
        {
            if(sentenceIndex < 0 || sentenceIndex>Sentences.Count-1)
            {
                Console.WriteLine("Строки с таким номером не существует");
                return;
            }
            Sentence sentence = Sentences[sentenceIndex];
            foreach (Token item in sentence.Items)
            {
                if (item is Word word && word.Content.Length == targetLength) word.Content = STR;
            }
        }

        public void RemoveStopWord(StreamReader sr) 
        {
            try
            {
                HashSet<string> uniqueStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                string? line;
                using (sr)
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (string.IsNullOrEmpty(line)) continue;

                        uniqueStopWords.Add(line.ToLower());

                    }   
                foreach (Sentence sentence in Sentences)
                        {
                            sentence.Items.RemoveAll(token => token is Word word && uniqueStopWords.Contains(word.Content));
                        }    
                }
            }                                         
            catch (Exception e) { Console.WriteLine($"Ошибка выполнения: {e}"); }
        }

        public void ExportToXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Text));

            
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8
            };
            XmlSerializerNamespaces emptyNamespaces = new XmlSerializerNamespaces();
            emptyNamespaces.Add("", "");

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(writer, this, emptyNamespaces);
            }
        }

        public string GetConcordance()
        {
           
            var concordance = new SortedDictionary<string, (int Count, HashSet<int> LineNumbers)>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < Sentences.Count; i++)
            {
                foreach (var token in Sentences[i].Items)
                {
                    if (token is Word word)
                    {
                        string lowerWord = word.Content.ToLower();

                        if (!concordance.ContainsKey(lowerWord))
                        {
                            concordance[lowerWord] = (0, new HashSet<int>());
                        }

                        var entry = concordance[lowerWord];
                        entry.Count++; 
                        entry.LineNumbers.Add(i + 1); 
                        concordance[lowerWord] = entry;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var kvp in concordance)
            {
                string word = kvp.Key.ToLower();
                int count = kvp.Value.Count;

                string lines = string.Join(" ", kvp.Value.LineNumbers.OrderBy(x => x));

                
                string dots = new string('.', Math.Max(2, 30 - word.Length));
                sb.AppendLine($"{word}{dots}{count}: {lines}");
            }

            return sb.ToString().TrimEnd();
        }




    }
}
