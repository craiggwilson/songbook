using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Theory
{
    public class Chord : IEquatable<Chord>
    {
        public Chord(string name, Note root, IEnumerable<Interval> intervals, Note baseNote)
        {
            Name = name;
            RootNote = root;
            Intervals = (intervals ?? new List<Interval>()).ToList().AsReadOnly();
            BaseNote = baseNote;
        }

        public string Name { get; private set; }

        public Note RootNote { get; private set; }

        public Note BaseNote { get; private set; }

        public bool HasBaseNote
        {
            get { return BaseNote != null; }
        }

        public ReadOnlyCollection<Interval> Intervals { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Chord);
        }

        public bool Equals(Chord other)
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

        public string GetProperName()
        {
            return ChordNamer.GetProperName(this);
        }

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
