using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Songbook.Structure.Parsing;
using Xunit;

namespace Songbook.Theory
{
    public class ChordNamerTests
    {
        [Fact]
        public void Major()
        {
            var chord = ParseChord("C");

            chord.GetProperName().Should().Be("C");
        }

        [Fact]
        public void Major7()
        {
            var chord = ParseChord("Cmaj7");

            chord.GetProperName().Should().Be("Cmaj7");
        }

        [Fact]
        public void Minor7()
        {
            var chord = ParseChord("Cm7");

            chord.GetProperName().Should().Be("Cm7");
        }

        [Fact]
        public void Seventh()
        {
            var chord = ParseChord("C7");

            chord.GetProperName().Should().Be("C7");
        }

        [Fact]
        public void Thirteenth()
        {
            var chord = ParseChord("C13");

            chord.GetProperName().Should().Be("C13");
        }

        [Fact]
        public void Suspended()
        {
            var chord = ParseChord("Csus2");

            chord.GetProperName().Should().Be("Csus2");
        }

        [Fact]
        public void Augmented()
        {
            var chord = ParseChord("C+");

            chord.GetProperName().Should().Be("C+");
        }

        [Fact]
        public void Diminished()
        {
            var chord = ParseChord("Cm(b5)");

            chord.GetProperName().Should().Be("Cdim");
        }

        [Fact]
        public void HalfDimished_seventh()
        {
            var chord = ParseChord("Cm7b5");

            chord.GetProperName().Should().Be("Cm7(b5)");
        }

        [Fact]
        public void Add2Flat5()
        {
            var chord = ParseChord("C2b5");

            chord.GetProperName().Should().Be("C2(b5)");
        }

        [Fact]
        public void Eleventh_augmented_fifth_add_sixth_with_base_note()
        {
            var chord = ParseChord("C11+5add6/G");

            chord.GetProperName().Should().Be("C13(#5)/G");
        }

        private Chord ParseChord(string chord)
        {
            return new ParsingChordLookup().Lookup(chord).Item2;
        }
    }
}
