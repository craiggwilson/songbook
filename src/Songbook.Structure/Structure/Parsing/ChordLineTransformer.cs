using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Theory;

namespace Songbook.Structure.Parsing
{
    /// <summary>
    /// If all the "words" on a line are chords, then transform them all to chords. Otherwise, leave the line alone.
    /// </summary>
    public class ChordLineTransformer : NodeVisitor
    {
        private readonly IChordLookup _chordLookup;

        public ChordLineTransformer(IChordLookup chordLookup)
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
