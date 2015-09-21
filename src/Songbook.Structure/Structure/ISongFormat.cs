using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Structure
{
    public interface ISongFormat
    {
        string Name { get; }

        SongNode Read(TextReader reader);

        void Write(SongNode node, TextWriter writer);
    }
}
