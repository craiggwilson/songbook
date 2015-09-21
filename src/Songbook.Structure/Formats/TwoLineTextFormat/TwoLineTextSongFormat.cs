using System.IO;
using Songbook.Structure;
using Songbook.Text;

namespace Songbook.Formats.TwoLineTextFormat
{
    public class SimpleTwoLineTextFormat : ISongFormat
    {
        private readonly IChordLookup _chordLookup;

        public SimpleTwoLineTextFormat(IChordLookup chordLookup)
        {
            _chordLookup = chordLookup;
        }

        public string Name => "Two Line Text";

        public SongNode Read(TextReader reader)
        {
            var lexer = new SongLexer(new BufferedCharInputStream(reader, 10));
            var node = new TwoLineTextFormatReader(lexer).Parse();

            node = (SongNode)new ChordVisitor(_chordLookup).Visit(node);
            node = (SongNode)new SectionNamingVisitor().Visit(node);
            return node;
        }

        public void Write(SongNode node, TextWriter writer)
        {
            new TwoLineTextFormatWriter(writer).Visit(node);
        }
    }
}
