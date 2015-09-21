using System.Collections.Generic;
using Songbook.Structure;

namespace Songbook.Formats.TwoLineTextFormat
{
    /// <summary>
    /// If all the "words" on a line are chords, then transform them all to chords. Otherwise, leave the line alone.
    /// </summary>
    public class ChordVisitor : NodeVisitor
    {
        private readonly IChordLookup _chordLookup;

        public ChordVisitor(IChordLookup chordLookup)
        {
            _chordLookup = chordLookup;
        }

        protected override Node VisitLine(LineNode node)
        {
            var newParts = new List<LinePartNode>();
            foreach (var part in node.Parts)
            {
                var newPart = Visit(part);
                if (newPart.Kind != NodeKind.WhiteSpace && newPart.Kind != NodeKind.Chord)
                    return node;

                newParts.Add((LinePartNode)newPart);
            }

            return new LineNode(newParts);
        }

        protected override Node VisitText(TextNode node)
        {
            if (string.IsNullOrWhiteSpace(node.Text))
                return node;

            var result = _chordLookup.Lookup(node.Text);
            if (result.Item1)
                return new ChordNode(result.Item2);

            return node;
        }
    }
}
