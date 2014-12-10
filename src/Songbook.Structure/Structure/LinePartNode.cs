using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public abstract class LinePartNode : Node
    {
        protected LinePartNode(LineInfo lineInfo)
            : base(lineInfo)
        { }
    }
}
