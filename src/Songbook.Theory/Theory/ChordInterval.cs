using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Theory
{
    public class ChordInterval : IEquatable<ChordInterval>
    {
        public ChordInterval(ChordIntervalKind kind, int value)
        {
            Kind = kind;
            Value = value;
        }

        public ChordIntervalKind Kind { get; private set; }

        public int Value { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChordInterval);
        }

        public bool Equals(ChordInterval other)
        {
            if (other == null)
                return false;

            return Kind == other.Kind && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Kind.GetHashCode() ^ Value.GetHashCode();
        }

        public override string ToString()
        {
            return Kind.ToString() + " " + Value;
        }

        public static bool operator ==(ChordInterval a, ChordInterval b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ChordInterval a, ChordInterval b)
        {
            return !(a == b);
        }

        public static ChordInterval Augmented(int interval)
        {
            return new ChordInterval(ChordIntervalKind.Augmented, interval);
        }

        public static ChordInterval Diminished(int interval)
        {
            return new ChordInterval(ChordIntervalKind.Diminished, interval);
        }

        public static ChordInterval Major(int interval)
        {
            return new ChordInterval(ChordIntervalKind.Major, interval);
        }

        public static ChordInterval Minor(int interval)
        {
            return new ChordInterval(ChordIntervalKind.Minor, interval);
        }

        public static ChordInterval Perfect(int interval)
        {
            return new ChordInterval(ChordIntervalKind.Perfect, interval);
        }
    }
}
