using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Theory;

namespace Songbook.Structure
{
    public class ChordNode : LinePartNode
    {
        public ChordNode(LineInfo lineInfo, Chord chord)
            : base(lineInfo)
        {
            Chord = chord;
        }

        public Chord Chord { get; private set; }

        public override NodeKind Kind
        {
            get { return NodeKind.Chord; }
        }
    }
}
