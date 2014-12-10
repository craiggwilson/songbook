using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Theory
{
    public class Note : IEquatable<Note>
    {
        private static readonly List<string> _chromaticSharpScale = new List<string> { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        private static readonly List<string> _chromaticFlatScale = new List<string> { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab" };

        private static readonly Dictionary<string, Note> _notes = new Dictionary<string, Note>
        {
             { "A", new Note("A") },
             { "A#", new Note("A#", "Bb") },
             { "Bb", new Note("Bb", "A#") },
             { "B", new Note("B") },
             { "C", new Note("C") },
             { "C#", new Note("C#", "Db") },
             { "Db", new Note("Db", "C#") },
             { "D", new Note("D") },
             { "D#", new Note("D#", "Eb") },
             { "Eb", new Note("Eb", "D#") },
             { "E", new Note("E") },
             { "F", new Note("F") },
             { "F#", new Note("F#", "Gb") },
             { "Gb", new Note("Gb", "F#") },
             { "G", new Note("G") },
             { "G#", new Note("G#", "Ab") },
             { "Ab", new Note("Ab", "G#") }
        };

        private Note(string name, string enharmonicName = null)
        {
            Name = name;
            EnharmonicName = enharmonicName ?? name;
        }

        public string Name { get; private set; }

        public string EnharmonicName { get; private set; }

        public Note Transpose(int semitoneOffset)
        {
            var scale = _chromaticSharpScale;
            var index = scale.IndexOf(Name);
            if (index == -1)
            {
                scale = _chromaticFlatScale;
                index = scale.IndexOf(Name);
            }

            index += semitoneOffset;
            if (index >= scale.Count)
                index -= scale.Count;
            else if (index < 0)
                index += scale.Count;

            return Note.FromName(scale[index]);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Note);
        }

        public bool Equals(Note other)
        {
            if (other == null)
                return false;

            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static Note FromName(string name)
        {
            Note note;
            if (_notes.TryGetValue(name, out note))
                return note;

            return null;
        }

        public static bool operator ==(Note a, Note b)
        {
            if (object.ReferenceEquals(a, b))
                return true;

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Note a, Note b)
        {
            return !(a == b);
        }

    }
}
