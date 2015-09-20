using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public sealed class PropertyNode : Node
    {
        public PropertyNode(string name, IEnumerable<string> values)
        {
            Name = name;
            Values = values as ReadOnlyCollection<string>;
            if (Values == null)
            {
                Values = values.ToList().AsReadOnly();
            }
        }

        public override NodeKind Kind => NodeKind.Property;

        public string Name { get; }

        public ReadOnlyCollection<string> Values { get; }
    }
}
