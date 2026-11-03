using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lab4
{
    public class Sentence : Token
    {
        
        [XmlIgnore]
        public List<Token> Items { get; set; } = new List<Token>();


        [XmlIgnore]
        public new string Content { get; set; } = string.Empty;
        public Sentence() : base("") { }

        [XmlElement("word")]
        public List<string> WordsForXml
        {
            get => Items.OfType<Word>().Select(w => w.Content).ToList();
            set
            {
                if (value != null)
                {
                    Items = value.Select(w => (Token)new Word(w)).ToList();
                }
            }
        }
        
        [XmlElement("punctuation")]
        
        public List<string> PunctForXml
        {
            get => Items.OfType<Punctuation>().Select(p => p.Content).ToList();
            set
            {
                if (value != null)
                {
                    Items = value.Select(p => (Token)new Punctuation(p)).ToList();
                }
            }
        }
        
        public void Add(Token token)
        {
            Items.Add(token);
        }
        public override string ToString()
        {
            
            StringBuilder Result=new StringBuilder();
            Token[] Sent=Items.ToArray();
            for(int i=0; i<Sent.Length;i++)
                if (Sent[i] is Word && i>0) 
                {
                    Result.Append($" {Sent[i]}");
                }
                else
                {
                    Result.Append(Sent[i]);
                }
                return Result.ToString();
        }

        public int WordCount => Items.OfType<Word>().Count();

        public int Length => ToString().Length;

        public bool IsQuestion => Items.OfType<Punctuation>().Any(p => p.Content.Contains("?"));

    }
}
