using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public interface ISongParser
    {
        SongNode Parse(string song);
    }
}
