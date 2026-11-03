using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Serialization;

namespace Lab4
{
    public abstract class Token
    {
        
        public string Content { get; set; }

        
        public abstract override string ToString();
        protected Token(string content)
        {
            Content = content;
        }



    }
}
