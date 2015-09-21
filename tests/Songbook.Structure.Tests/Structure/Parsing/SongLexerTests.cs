using FluentAssertions;
using Songbook.Formats.TwoLineTextFormat;
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

            AssertToken(TokenKind.Text, "[Verse]", stream.Consume());
            AssertToken(TokenKind.EndOfLine, "\r\n", stream.Consume());
            AssertToken(TokenKind.Text, "Am", stream.Consume());
            AssertToken(TokenKind.WhiteSpace, "  ", stream.Consume());
            AssertToken(TokenKind.Text, "C", stream.Consume());
            AssertToken(TokenKind.WhiteSpace, "   ", stream.Consume());
            AssertToken(TokenKind.Text, "G", stream.Consume());
            AssertToken(TokenKind.EndOfLine, "\r", stream.Consume());
        }

        private void AssertToken(TokenKind kind, string text, Token token)
        {
            token.Kind.Should().Be(kind);
            token.Text.Should().Be(text);
        }

    }
}
