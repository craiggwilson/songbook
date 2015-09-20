using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Songbook.Structure
{
    public sealed class SongNode : Node
    {
        public SongNode(IEnumerable<PropertyNode> properties, IEnumerable<SectionNode> sections)
        {
            Properties = properties as ReadOnlyCollection<PropertyNode>;
            if (Properties == null)
            {
                Properties = (properties ?? new List<PropertyNode>()).ToList().AsReadOnly();
            }

            Sections = sections as ReadOnlyCollection<SectionNode>;
            if (Sections == null)
            {
                Sections = (sections ?? new List<SectionNode>()).ToList().AsReadOnly();
            }
        }

        public override NodeKind Kind => NodeKind.Song;

        public ReadOnlyCollection<PropertyNode> Properties { get; }

        public ReadOnlyCollection<SectionNode> Sections { get; }

        public SongNode Update(IEnumerable<PropertyNode> properties, IEnumerable<SectionNode> sections)
        {
            if (properties != Properties || sections != Sections)
            {
                return new SongNode(properties, sections);
            }

            return this;
        }
    }
}