using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Songbook.Theory;
using Xunit;

namespace Songbook.Structure.Parsing
{
    public class ParsingChordLookupTests
    {
        [Fact]
        public void Not_a_chord()
        {
            ParseChord("About").Item1.Should().BeFalse();
        }

        [Fact]
        public void Major()
        {
            var result = ParseChord("C").Item2;
            AssertChord(result, "C", null, Interval.Major(3), Interval.Perfect(5));
        }

        [Fact]
        public void Minor()
        {
            var result = ParseChord("Cm").Item2;
            AssertChord(result, "C", null, Interval.Minor(3), Interval.Perfect(5));
        }

        [Fact]
        public void Major_seventh()
        {
            var result = ParseChord("Cmaj7").Item2;
            AssertChord(result, "C", null, Interval.Major(3), Interval.Perfect(5), Interval.Major(7));
        }

        [Fact]
        public void Minor_seventh()
        {
            var result = ParseChord("Cm7").Item2;
            AssertChord(result, "C", null, Interval.Minor(3), Interval.Perfect(5), Interval.Minor(7));
        }

        [Fact]
        public void Dominant_ninth()
        {
            var result = ParseChord("C9").Item2;
            AssertChord(result, "C", null, Interval.Major(3), Interval.Perfect(5), Interval.Minor(7), Interval.Major(2));
        }

        [Fact]
        public void Base_note()
        {
            var result = ParseChord("C/Bb").Item2;
            AssertChord(result, "C", "Bb", Interval.Major(3), Interval.Perfect(5));
        }

        [Fact]
        public void Eleventh_augmented_fifth_add_sixth_with_base_note()
        {
            //note: this chord is really a C13+5/G, but I need something to test
            //here...
            var result = ParseChord("C11+5add6/G").Item2;
            AssertChord(result, "C", "G", Interval.Major(3), Interval.Augmented(5), Interval.Minor(7), Interval.Perfect(4), Interval.Major(6));
        }

        [Fact]
        public void Suspened_with_an_add()
        {
            var result = ParseChord("Csus4add2").Item2;
            AssertChord(result, "C", null, Interval.Perfect(5), Interval.Perfect(4), Interval.Major(2));
        }

        private Tuple<bool, Chord> ParseChord(string chord)
        {
            return ParsingChordLookup.Instance.Lookup(chord);
        }

        private void AssertChord(Chord chord, string rootNote, string baseNote, params Interval[] intervals)
        {
            chord.RootNote.Name.Should().Be(rootNote);
            if (chord.HasBaseNote)
                chord.BaseNote.Name.Should().Be(baseNote);
            if (intervals != null)
            {
                foreach (var n in intervals)
                    chord.Intervals.Should().Contain(n);
            }
        }
    }
}
