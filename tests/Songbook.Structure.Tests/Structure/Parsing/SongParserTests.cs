using System.IO;
using FluentAssertions;
using Songbook.Formats;
using Songbook.Formats.TwoLineTextFormat;
using Xunit;

namespace Songbook.Structure.Parsing
{
    public class SongParserTests
    {
        [Fact]
        public void Should_parse_song_correctly()
        {
            const string text = "[Verse]\r\nAm  C   G\rI'm the only one in the world\rAm  C  D\nHello, goodbye world\r\n\r\n[Chorus]\r\nD  Em\r\nAwesome!!!";

            var subject = new SimpleTwoLineTextFormat(ParsingChordLookup.Instance);

            var song = subject.Read(new StringReader(text));

            song.Sections.Count.Should().Be(2);
            var section = song.Sections[0];
            section.Name.Should().Be("Verse");
            section.Lines.Count.Should().Be(6);

            section.Lines[0].Parts.Count.Should().Be(1);
            section.Lines[1].Parts.Count.Should().Be(5);
            section.Lines[1].Parts[0].Kind.Should().Be(NodeKind.Chord);

            section.Lines[2].Parts[0].Kind.Should().Be(NodeKind.Text);

            section.Lines[3].Parts[0].Kind.Should().Be(NodeKind.Chord);

            section.Lines[4].Parts[0].Kind.Should().Be(NodeKind.Text);

            section = song.Sections[1];
            section.Lines.Count.Should().Be(3);
            section.Name.Should().Be("Chorus");
            section.Lines[0].Parts.Count.Should().Be(1);
            section.Lines[1].Parts.Count.Should().Be(3);
            section.Lines[2].Parts.Count.Should().Be(1);
        }
    }
}
