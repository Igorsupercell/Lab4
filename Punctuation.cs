using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace Lab4
{
    public  class Punctuation : Token
    {
        public Punctuation() : base(""){}
        public override string ToString() => Content;
        public Punctuation(string punct) : base(punct)
        {
            if (!Allowed.Contains(punct)) throw new InvalidOperationException($"знак {punct},не входит в список допустимых");
        }
        public static  readonly HashSet<string> Allowed = new() { ".", ",", "!", "?", "\'", "\"", "?!", ";", ":", "[", "]", "{", "}", ")", "_", "(", "!!", "-", "..."};
        public static readonly HashSet<string> AllowedToendSentence = new() { ".","!", "?", "?!","!!", "..."};
    }
}
