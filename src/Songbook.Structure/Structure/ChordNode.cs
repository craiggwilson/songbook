using Songbook.Theory;

namespace Songbook.Structure
{
    public sealed class ChordNode : LinePartNode
    {
        public ChordNode(Chord chord)
        {
            Chord = chord;
        }

        public Chord Chord { get; }

        public override NodeKind Kind => NodeKind.Chord;
    }
}
