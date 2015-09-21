using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Text;
using Songbook.Theory;

namespace Songbook.Formats
{
    public class ParsingChordLookup : IChordLookup
    {
        public static ParsingChordLookup Instance = new ParsingChordLookup();

        private ParsingChordLookup()
        { }

        public Tuple<bool, Chord> Lookup(string chord)
        {
            try
            {
                return Tuple.Create(true, Chord.Parse(chord));
            }
            catch (ChordParseException)
            {
                return Tuple.Create(false, (Chord)null);
            }
        }
    }
}
