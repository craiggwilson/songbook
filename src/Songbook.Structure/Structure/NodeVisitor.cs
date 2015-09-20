using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public abstract class NodeVisitor
    {
        public virtual Node Visit(Node node)
        {
            switch (node.Kind)
            {
                case NodeKind.Chord:
                    return VisitChord((ChordNode)node);
                case NodeKind.Line:
                    return VisitLine((LineNode)node);
                case NodeKind.Property:
                    return VisitProperty((PropertyNode)node);
                case NodeKind.Section:
                    return VisitSection((SectionNode)node);
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
            return node.Update(
                Visit(node.Parts));
        }

        private Node VisitProperty(PropertyNode node)
        {
            return node;
        }

        protected virtual Node VisitSection(SectionNode node)
        {
            return node.Update(
                Visit(node.Lines));
        }

        protected virtual Node VisitSong(SongNode node)
        {
            return node.Update(
                Visit(node.Properties),
                Visit(node.Sections));
        }

        protected virtual Node VisitText(TextNode node)
        {
            return node;
        }

        protected virtual Node VisitWhiteSpace(WhiteSpaceNode node)
        {
            return node;
        }

        protected ReadOnlyCollection<T> Visit<T>(ReadOnlyCollection<T> nodes) where T : Node
        {
            List<T> newNodes = null;
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var newNode = (T)Visit(node);
                if (newNodes == null && newNode != node)
                    newNodes = nodes.Take(i).ToList();
                if (newNodes != null)
                    newNodes.Add(newNode);
            }

            if (newNodes == null)
            {
                return nodes;
            }

            return newNodes.AsReadOnly();
        }
    }
}
