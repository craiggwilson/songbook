using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure.Visitors
{
    public abstract class Visitor
    {
        protected virtual Node Visit(Node node)
        {
            switch(node.Kind)
            {
                case NodeKind.Chord:
                    return VisitChord((ChordNode)node);
                case NodeKind.Line:
                    return VisitLine((LineNode)node);
                case NodeKind.Song:
                    return VisitSong((SongNode)node);
                case NodeKind.Text:
                    return VisitText((TextNode)node);
                case NodeKind.WhiteSpace:
                    return VisitWhiteSpace((WhiteSpaceNode)node);
            }

            throw new NotSupportedException(string.Format("Unknown node of kind '{0}'", node.Kind));
        }

        protected virtual Node VisitChord(ChordNode node)
        {
            return node;
        }

        protected virtual Node VisitLine(LineNode node)
        {
            List<LinePartNode> newParts = null;
            for (int i = 0; i < node.Parts.Count; i++)
            {
                var part = node.Parts[i];
                var newPart = (LinePartNode)Visit(part);
                if (newParts == null && newPart != part)
                    newParts = node.Parts.Take(i).ToList();
                if (newParts != null)
                    newParts.Add(newPart);
            }

            if (newParts != null)
                return new LineNode(node.LineInfo, newParts);

            return node;
        }

        protected virtual Node VisitSong(SongNode node)
        {
            List<LineNode> newLines = null;
            for(int i = 0; i < node.Lines.Count; i++)
            {
                var line = node.Lines[i];
                var newLine = (LineNode)Visit(line);
                if (newLines == null && newLine != line)
                    newLines = node.Lines.Take(i).ToList();
                if (newLines != null)
                    newLines.Add(newLine);
            }

            if (newLines != null)
                return new SongNode(node.LineInfo, newLines);

            return node;
        }

        protected virtual Node VisitText(TextNode node)
        {
            return node;
        }

        protected virtual Node VisitWhiteSpace(WhiteSpaceNode node)
        {
            return node;
        }
    }
}
