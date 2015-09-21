using System.Collections.Generic;
using Songbook.Text;

namespace Songbook.Formats.TwoLineTextFormat
{
    public class SongLexer : AbstractBufferedInputStream<Token>
    {
        private readonly IInputStream<char> _input;

        public SongLexer(string input)
            : this(new BufferedCharInputStream(input, 10))
        { }

        public SongLexer(IInputStream<char> input)
            : base(10)
        {
            _input = input;
        }

        protected override Token[] ReadInput(int count)
        {
            int i = 0;
            var tokens = new List<Token>();
            Token token = Read();
            while (i < count)
            {
                tokens.Add(token);
                if (token.Kind == TokenKind.EOF)
                {
                    break;
                }
                token = Read();
            }

            return tokens.ToArray();
        }

        private Token Read()
        {
            var c = _input.LA(0);

            if (c == '\0')
            {
                return ReadEndOfFile();
            }

            if (c == '\r' || c == '\n')
            {
                return ReadLine();
            }

            if (char.IsWhiteSpace(c))
            {
                return ReadWhiteSpace();
            }

            return ReadWord();
        }

        private Token ReadEndOfFile()
        {
            return new Token(TokenKind.EOF, null);
        }

        private Token ReadLine()
        {
            _input.Mark();
            if (_input.LA(0) == '\r' && _input.LA(1) == '\n')
            {
                _input.Consume(2);
            }
            else
            {
                _input.Consume();
            }

            return new Token(TokenKind.EndOfLine, new string(_input.ClearMark()));
        }

        private Token ReadWhiteSpace()
        {
            _input.Mark();
            char c = _input.LA(0);
            while (char.IsWhiteSpace(c))
            {
                if (c == '\r' || c == '\n')
                {
                    break;
                }

                _input.Consume();
                c = _input.LA(0);
            }

            return new Token(TokenKind.WhiteSpace, new string(_input.ClearMark()));
        }

        private Token ReadWord()
        {
            _input.Mark();
            char c = _input.LA(0);
            while (c != '\0' && !char.IsWhiteSpace(c))
            {
                _input.Consume();
                c = _input.LA(0);
            }

            return new Token(TokenKind.Text, new string(_input.ClearMark()));
        }
    }
}
