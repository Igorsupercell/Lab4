using System.Text;


namespace Lab4
{
    public class TextParcer
    {
        static public Text Parse(StreamReader sr)
        {

            StringBuilder WordBuilder = new StringBuilder();
            StringBuilder PunctBuilder = new StringBuilder();

            string? line;
            Text text = new Text();
            int j = 0;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                for (int i = 0; i < line.Length; i++)
                {
                    char CurrentChar = line[i];
                    if (char.IsWhiteSpace(CurrentChar))
                    {
                        if (WordBuilder.Length > 0)
                        {
                            text.Sentences[j].Add(new Word(WordBuilder.ToString()));
                            WordBuilder.Clear();
                        }
                        continue;
                    }


                    if (Punctuation.Allowed.Contains(CurrentChar.ToString()))
                    {
                        if (WordBuilder.Length > 0)
                        {
                            text.Sentences[j].Add(new Word(WordBuilder.ToString()));
                            WordBuilder.Clear();
                        }
                        PunctBuilder.Append(CurrentChar.ToString());

                        if (Punctuation.AllowedToendSentence.Contains(CurrentChar.ToString()))
                        {
                            while (i + 1 < line.Length && Punctuation.AllowedToendSentence.Contains(line[i + 1].ToString()))
                            {
                                PunctBuilder.Append(line[i + 1]);
                                i++;
                            }
                            text.Sentences[j].Add(new Punctuation(PunctBuilder.ToString()));
                            PunctBuilder.Clear();
                            text.Add(new Sentence());
                            j++;
                        }
                        else
                        {
                            text.Sentences[j].Add(new Punctuation(PunctBuilder.ToString()));
                            PunctBuilder.Clear();
                        }
                    }
                    else
                    {
                        WordBuilder.Append(line[i]);
                    }
                }
                if (WordBuilder.Length > 0)
                {
                    text.Sentences[j].Add(new Word(WordBuilder.ToString()));
                    WordBuilder.Clear();
                }
            }
            text.Sentences.RemoveAll(s => s.Items.Count == 0);
            return text;
        }



    }
}
     
