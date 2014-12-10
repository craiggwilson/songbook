using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Structure;

namespace Songbook.Structure.Parsing
{
    public class SongParser
    {
        private readonly IInputStream<Token> _input;

        public SongParser(IInputStream<Token> input)
        {
            _input = input;
        }

        public SongNode Parse()
        {
            var lines = ReadLines();

            return new SongNode(new LineInfo(0, 0), lines);
        }

        private IEnumerable<LineNode> ReadLines()
        {
            var lines = new List<LineNode>();
            while(_input.LA(0).Kind != TokenKind.EOF)
                lines.Add(ReadLine());
            return lines;
        }

        private LineNode ReadLine()
        {
            var parts = new List<LinePartNode>();

            var lineInfo = _input.LA(0).LineInfo;
            ReadLineStart:
            switch(_input.LA(0).Kind)
            {
                case TokenKind.Text:
                case TokenKind.WhiteSpace:
                    var token = _input.Consume();
                    parts.Add(new TextNode(token.LineInfo, token.Text));
                    goto ReadLineStart;
                case TokenKind.EndOfLine:
                    _input.Consume();
                    break;
            }

            return new LineNode(lineInfo, parts);
        }
    }
}
