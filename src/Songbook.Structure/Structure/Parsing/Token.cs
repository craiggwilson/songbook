using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure.Parsing
{
    public class Token
    {
        public TokenKind Kind { get; private set; }

        public LineInfo LineInfo { get; private set; }

        public string Text { get; private set; }

        public Token(TokenKind kind, string text, int line, int character)
        {
            Kind = kind;
            Text = text;
            LineInfo = new LineInfo(line, character);
        }
    }
}
