using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4
{
    public class Word : Token 
    {
        public Word(string content) : base(content){}
       
        public override string ToString() =>Content;

        public static readonly HashSet<char> ENGsogl = new() { 'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm'};
        public static readonly HashSet<char> RUSsogl = new() { 'ц', 'к', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ф', 'в', 'п', 'р', 'л', 'д', 'ж', 'ч', 'с', 'м', 'т', 'б'};
    }
}
