using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Structure;

namespace Songbook.Structure.Parsing
{
    public class SongParser : ISongParser
    {
        private readonly List<INodeTransformer> _transformers;

        public SongParser(IEnumerable<INodeTransformer> transformers)
        {
            _transformers = transformers.ToList();
        }

        public SongNode Parse(string song)
        {
            var lexer = new SongLexer(song);
            var songNode = new StatefulSongParser(lexer).Parse();

            return (SongNode)_transformers.Aggregate(
                (Node)songNode, 
                (node, transformer) => transformer.Transform(node));
        }

        private class StatefulSongParser
        {
            private readonly IInputStream<Token> _input;

            public StatefulSongParser(IInputStream<Token> input)
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
                while (_input.LA(0).Kind != TokenKind.EOF)
                    lines.Add(ReadLine());
                return lines;
            }

            private LineNode ReadLine()
            {
                var parts = new List<LinePartNode>();

                var lineInfo = _input.LA(0).LineInfo;
                ReadLineStart:
                switch (_input.LA(0).Kind)
                {
                    case TokenKind.Text:
                        var wordToken = _input.Consume();
                        parts.Add(new TextNode(wordToken.LineInfo, wordToken.Text));
                        goto ReadLineStart;
                    case TokenKind.WhiteSpace:
                        var whiteSpaceToken = _input.Consume();
                        parts.Add(new WhiteSpaceNode(whiteSpaceToken.LineInfo, whiteSpaceToken.Text));
                        goto ReadLineStart;
                    case TokenKind.EndOfLine:
                        _input.Consume();
                        break;
                }

                return new LineNode(lineInfo, parts);
            }
        }
    }
}
