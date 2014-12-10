using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure.Parsing
{
    public class SongLexer : AbstractBufferedInputStream<Token>
    {
        private readonly IInputStream<char> _input;
        private int _character;
        private int _line;

        public SongLexer(IInputStream<char> input)
            : base(10)
        {
            _input = input;
            _character = 1;
            _line = 1;
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
                    break;
                token = Read();
            }

            return tokens.ToArray();
        }

        private Token Read()
        {
            var c = _input.LA(0);

            if (c == '\0')
                return ReadEndOfFile();

            if (c == '\r' || c == '\n')
                return ReadLine();

            if (char.IsWhiteSpace(c))
            {
                return ReadWhiteSpace();
            }

            return ReadWord();
        }

        private Token ReadEndOfFile()
        {
            return new Token(TokenKind.EOF, null, _line, _character);
        }

        private Token ReadLine()
        {
            var line = _line;
            var character = _character;

            _input.Mark();
            if (_input.LA(0) == '\r' && _input.LA(1) == '\n')
                _input.Consume(2);
            else
                _input.Consume();

            _line++;
            _character = 1;
            return new Token(TokenKind.EndOfLine, new string(_input.ClearMark()), line, character);
        }

        private Token ReadWhiteSpace()
        {
            var line = _line;
            var character = _character;
            _input.Mark();
            char c = _input.LA(0);
            while (char.IsWhiteSpace(c))
            {
                if (c == '\r' || c == '\n')
                    break;

                _input.Consume();
                _character++;
                c = _input.LA(0);
            }

            return new Token(TokenKind.WhiteSpace, new string(_input.ClearMark()), line, character);
        }

        private Token ReadWord()
        {
            var line = _line;
            var character = _character;
            _input.Mark();
            char c = _input.LA(0);
            while (c != '\0' && !char.IsWhiteSpace(c))
            {
                _input.Consume();
                _character++;
                c = _input.LA(0);
            }

            return new Token(TokenKind.Text, new string(_input.ClearMark()), line, character);
        }
    }
}
