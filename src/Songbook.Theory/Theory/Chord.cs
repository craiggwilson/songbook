using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Songbook.Theory
{
    public class Chord : IEquatable<Chord>
    {
        public Chord(string name, Note root, IEnumerable<Interval> intervals, Note baseNote)
        {
            Name = name;
            RootNote = root;
            Intervals = intervals as ReadOnlyCollection<Interval>;
            if (Intervals == null)
            {
                Intervals = (intervals ?? new List<Interval>()).ToList().AsReadOnly();
            }
            BaseNote = baseNote;
        }

        public string Name { get; }

        public Note RootNote { get; }

        public Note BaseNote { get; }

        public bool HasBaseNote => BaseNote != null;

        public ReadOnlyCollection<Interval> Intervals { get; }

        public override bool Equals(object obj) => Equals(obj as Chord);

        public bool Equals(Chord other)
        {
            if (other == null)
                return false;

            return Name == other.Name;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public string GetProperName() => ChordNamer.GetProperName(this);

        public override string ToString() => Name;

        public Chord Transpose(int semitoneOffset)
        {
            var newRoot = RootNote.Transpose(semitoneOffset);

            Note newBase = null;
            if (BaseNote != null)
                newBase = BaseNote.Transpose(semitoneOffset);

            string newName = newRoot.Name;
            newName += GetSuffixWithoutBase();
            if (newBase != null)
                newName += "/" + newBase.Name;

            return new Chord(newName, newRoot, Intervals, newBase);
        }

        private string GetSuffixWithoutBase()
        {
            var result = Name.Remove(0, RootNote.Name.Length);

            if (BaseNote != null)
                result = result.Remove(result.Length - (1 + BaseNote.Name.Length));

            return result;
        }

        public static Chord Parse(string chord)
        {
            return new ChordParser(chord).Parse();
        }

        public static bool operator ==(Chord a, Chord b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Chord a, Chord b)
        {
            return !(a == b);
        }
    }
}
