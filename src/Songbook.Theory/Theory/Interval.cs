using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Theory
{
    public class Interval : IEquatable<Interval>
    {
        public Interval(IntervalKind kind, int value)
        {
            Kind = kind;
            Value = value;
        }

        public IntervalKind Kind { get; private set; }

        public int Value { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Interval);
        }

        public bool Equals(Interval other)
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

        public static bool operator ==(Interval a, Interval b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Interval a, Interval b)
        {
            return !(a == b);
        }

        public static Interval Augmented(int interval)
        {
            return new Interval(IntervalKind.Augmented, interval);
        }

        public static Interval Diminished(int interval)
        {
            return new Interval(IntervalKind.Diminished, interval);
        }

        public static Interval Major(int interval)
        {
            return new Interval(IntervalKind.Major, interval);
        }

        public static Interval Minor(int interval)
        {
            return new Interval(IntervalKind.Minor, interval);
        }

        public static Interval Perfect(int interval)
        {
            return new Interval(IntervalKind.Perfect, interval);
        }
    }
}
