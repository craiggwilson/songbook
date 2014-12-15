using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Songbook.Structure.Parsing
{
    public class SongLexerTests
    {
        [Fact]
        public void It_should_recognize_words_correctly()
        {
            const string song = "[Verse]\r\nAm  C   G\rI'm the only one in the world\rAm  C  D\nHello, goodbye world";

            var stream = new SongLexer(song);

            AssertToken(TokenKind.Text, "[Verse]", 1, 1, stream.Consume());
            AssertToken(TokenKind.EndOfLine, "\r\n", 1, 8, stream.Consume());
            AssertToken(TokenKind.Text, "Am", 2, 1, stream.Consume());
            AssertToken(TokenKind.WhiteSpace, "  ", 2, 3, stream.Consume());
            AssertToken(TokenKind.Text, "C", 2, 5, stream.Consume());
            AssertToken(TokenKind.WhiteSpace, "   ", 2, 6, stream.Consume());
            AssertToken(TokenKind.Text, "G", 2, 9, stream.Consume());
            AssertToken(TokenKind.EndOfLine, "\r", 2, 10, stream.Consume());
        }

        private void AssertToken(TokenKind kind, string text, int line, int character, Token token)
        {
            token.Kind.Should().Be(kind);
            token.Text.Should().Be(text);
            token.LineInfo.Character.Should().Be(character);
            token.LineInfo.Line.Should().Be(line);
        }

    }
}
