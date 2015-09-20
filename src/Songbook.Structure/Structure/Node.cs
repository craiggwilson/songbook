using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public abstract class Node
    {
        public abstract NodeKind Kind { get; }
    }
}
