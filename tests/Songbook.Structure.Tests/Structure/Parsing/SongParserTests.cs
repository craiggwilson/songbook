using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Songbook.Structure.Visitors;
using Xunit;

namespace Songbook.Structure.Parsing
{
    public class SongParserTests
    {
        [Fact]
        public void Should_parse_song_correctly()
        {
            const string text = "[Verse]\r\nAm  C   G\rI'm the only one in the world\rAm  C  D\nHello, goodbye world";

            var subject = new SongParser(new [] 
            {
                new ChordLineTransformer(new ParsingChordLookup())
            });
            
            var song = subject.Parse(text);

            song.Lines.Count.Should().Be(5);

            song.Lines[0].Parts.Count.Should().Be(1);
            song.Lines[1].Parts.Count.Should().Be(5);
            song.Lines[1].Parts[0].Kind.Should().Be(NodeKind.Chord);

            song.Lines[2].Parts[0].Kind.Should().Be(NodeKind.Text);

            song.Lines[3].Parts[0].Kind.Should().Be(NodeKind.Chord);

            song.Lines[4].Parts[0].Kind.Should().Be(NodeKind.Text);
        }
    }
}
