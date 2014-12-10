using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public class LineInfo
    {
        public int Character { get; private set; }

        public int Line { get; private set; }

        public LineInfo(int line, int character)
        {
            Line = line;
            Character = character;
        }
    }
}
