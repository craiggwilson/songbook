using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Structure;

namespace Songbook.Formats.TwoLineTextFormat
{
    internal class TwoLineTextFormatWriter : NodeVisitor
    {
        private readonly TextWriter _writer;

        public TwoLineTextFormatWriter(TextWriter writer)
        {
            _writer = writer;
        }

        protected override Node VisitChord(ChordNode node)
        {
            _writer.Write(node.Chord.ToString());
            return base.VisitChord(node);
        }

        protected override Node VisitLine(LineNode node)
        {
            var result = base.VisitLine(node);
            _writer.WriteLine();
            return result;
        }

        protected override Node VisitProperty(PropertyNode node)
        {
            _writer.WriteLine($"#{node.Name}={node.Values.FirstOrDefault()}");
            return base.VisitProperty(node);
        }

        protected override Node VisitSection(SectionNode node)
        {
            if (!string.IsNullOrEmpty(node.Name))
            {
                _writer.WriteLine($"[{node.Name}]");
            }

            return base.VisitSection(node);
        }

        protected override Node VisitText(TextNode node)
        {
            _writer.Write(node.Text);
            return base.VisitText(node);
        }

        protected override Node VisitWhiteSpace(WhiteSpaceNode node)
        {
            _writer.Write(node.Text);
            return base.VisitWhiteSpace(node);
        }
    }
}