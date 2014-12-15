using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure.Parsing
{
    public interface INodeTransformer
    {
        Node Transform(Node node);
    }
}
