using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Theory;

namespace Songbook.Structure.Parsing
{
    public interface IChordLookup
    {
        Tuple<bool, Chord> Lookup(string chord);
    }
}
