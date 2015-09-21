using System;
using System.Collections.Generic;
using System.Linq;

namespace Songbook.Theory
{
    internal class ChordParser
    {
        const char EOF = '\0';

        private readonly string _input;
        private int _pos;
        private Note _rootNote;
        private bool _hasDeterminedQuality;
        private Dictionary<int, Interval> _intervals;
        private Note _baseNote;

        public ChordParser(string input)
        {
            _input = input;
            _pos = 0;
            _intervals = new Dictionary<int, Interval>();
        }

        public Chord Parse()
        {
            _rootNote = ReadNote();
            if (_rootNote == null)
            {
                throw ParseException("A", "B", "C", "D", "E", "F", "G");
            }

            //add default triad intervals...
            AddInterval(Interval.Perfect(1));
            AddInterval(Interval.Major(3));
            AddInterval(Interval.Perfect(5));

            ReadInitialIntervals();
            _hasDeterminedQuality = true;

            while (LA(0) != EOF)
            {
                ReadAdditionalIntervals();
            }

            return new Chord(_input, _rootNote, _intervals.Values.OrderBy(x => x.Value), _baseNote);
        }

        private Note ReadNote()
        {
            string note;
            switch (LA(0))
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                    note = Consume().ToString();
                    break;
                default:
                    return null;
            }

            switch (LA(0))
            {
                case '#':
                    if (note != "B" && note != "E")
                    {
                        note += Consume();
                    }
                    break;
                case 'b':
                    if (note != "C" && note != "F")
                    {
                        note += Consume();
                    }
                    break;
            }

            return Note.FromName(note);
        }

        private void ReadInitialIntervals()
        {
            switch (LA(0))
            {
                case EOF:
                    return;
                case '(':
                    ReadParenthesis(ReadInitialIntervals);
                    return;
                case 's':
                    ReadSuspended();
                    return;
                case 'd':
                    ReadDiminishedOrDominant();
                    return;
                case 'o':
                    ReadDiminished();
                    return;
                case 'a':
                    ReadAugmentedOrAdd();
                    return;
                case '+':
                    ReadAugmented();
                    return;
                case 'm':
                    ReadMajorOrMinor();
                    return;
                case 'M':
                    ReadMajor();
                    return;
                case '-':
                    ReadMinor();
                    return;
                case '/':
                    ReadBase();
                    return;
                case '1':
                case '2':
                case '4':
                case '5':
                case '6':
                case '7':
                case '9':
                    ReadNumber();
                    return;
            }

            throw ParseException("s", "d", "o", "a", "+", "m", "M", "-", "/", "1", "2", "4", "5", "6", "7", "9");
        }

        private void ReadAdditionalIntervals()
        {
            switch (LA(0))
            {
                case EOF:
                    return;
                case '(':
                    ReadParenthesis(ReadAdditionalIntervals);
                    return;
                case 'a':
                    ReadAdd();
                    return;
                case 'b':
                case '-':
                    ReadFlat();
                    return;
                case '+':
                case '#':
                    ReadSharp();
                    return;
                case 'm':
                case 'M':
                    ReadMajor();
                    return;
                case '/':
                    ReadBaseOrNumber();
                    return;
                case '1':
                case '2':
                case '4':
                case '5':
                case '6':
                case '7':
                case '9':
                    ReadNumber();
                    return;
            }

            throw ParseException("a", "b", "+", "#", "m", "M", "/", "1", "2", "4", "5", "6", "7", "9");
        }

        private void ReadParenthesis(Action read)
        {
            Consume("(");
            read();
            Consume(")");
        }

        private void ReadSuspended()
        {
            Consume("sus");
            var number = Consume(2, 4);
            _intervals.Remove(3);
            switch (number)
            {
                case 2:
                    AddInterval(Interval.Major(2));
                    return;
                case 4:
                    AddInterval(Interval.Perfect(4));
                    return;
            }
        }

        private void ReadDiminishedOrDominant()
        {
            switch (LA(1))
            {
                case 'i':
                    ReadDiminished();
                    break;
                case 'o':
                    ReadDominant();
                    break;
            }

            throw ParseException("di", "do");
        }

        private void ReadDiminished()
        {
            Consume("o", "dim");

            if (!_hasDeterminedQuality)
            {
                AddInterval(Interval.Minor(3));
                AddInterval(Interval.Diminished(5));
            }

            if (LA(0) == '7')
            {
                Consume();
                AddInterval(Interval.Diminished(7));
            }
        }

        private void ReadDominant()
        {
            Consume("dom");
        }

        private void ReadAugmentedOrAdd()
        {
            switch (LA(1))
            {
                case 'u':
                    ReadAugmented();
                    break;
                case 'd':
                    ReadAdd();
                    break;
            }
            throw ParseException("au", "ad");
        }

        private void ReadAugmented()
        {
            Consume("+", "aug");

            AddInterval(Interval.Major(3));
            AddInterval(Interval.Augmented(5));

            if (LA(0) == '7')
            {
                Consume();
                AddInterval(Interval.Minor(7));
            }
        }

        private void ReadAdd()
        {
            Consume("add");
            var number = Consume(2, 4, 6, 9, 11, 13);

            switch (number)
            {
                case 2:
                case 9:
                    AddInterval(Interval.Major(2));
                    break;
                case 4:
                case 11:
                    AddInterval(Interval.Perfect(4));
                    break;
                case 6:
                case 13:
                    AddInterval(Interval.Major(6));
                    break;
            }
        }

        private void ReadMajorOrMinor()
        {
            switch (LA(1))
            {
                case 'a':
                    ReadMajor();
                    return;
                default:
                    ReadMinor();
                    return;
            }
        }

        private void ReadMajor()
        {
            Consume("M", "maj");

            if (_hasDeterminedQuality)
            {
                Consume(7);
                AddInterval(Interval.Major(7));
            }
            else
            {
                if (LA(0) == '7')
                {
                    Consume();
                    AddInterval(Interval.Major(7));
                }
                else if (LA(0) == '9')
                {
                    Consume();
                    AddInterval(Interval.Major(7));
                    AddInterval(Interval.Major(2));
                }
                else if (LA(0) == '1' && LA(1) == '1')
                {
                    Consume(2);
                    AddInterval(Interval.Major(7));
                    AddInterval(Interval.Perfect(4));
                }
                else if (LA(0) == '1' && LA(1) == '3')
                {
                    Consume(2);
                    AddInterval(Interval.Major(7));
                    AddInterval(Interval.Major(6));
                }
            }
        }

        private void ReadMinor()
        {
            if (LA(0) == 'm' && LA(1) == 'i')
                Consume("min");
            else
                Consume("-", "m");

            AddInterval(Interval.Minor(3));

            if (LA(0) == '9')
            {
                Consume();
                AddInterval(Interval.Minor(7));
                AddInterval(Interval.Major(2));
            }
            else if (LA(0) == '1' && LA(1) == '1')
            {
                Consume(2);
                AddInterval(Interval.Minor(7));
                AddInterval(Interval.Perfect(4));
            }
            else if (LA(0) == '1' && LA(1) == '3')
            {
                Consume(2);
                AddInterval(Interval.Minor(7));
                AddInterval(Interval.Major(6));
            }
        }

        private void ReadNumber()
        {
            int num = Consume(2, 4, 5, 6, 7, 9, 11, 13);

            switch (num)
            {
                case 2:
                    AddInterval(Interval.Major(2));
                    return;
                case 4:
                    AddInterval(Interval.Perfect(4));
                    return;
                case 6:
                    AddInterval(Interval.Major(6));
                    return;
                case 7:
                    AddInterval(Interval.Minor(7));
                    return;
                case 9:
                    if (!_hasDeterminedQuality)
                        AddInterval(Interval.Minor(7));
                    AddInterval(Interval.Major(2));
                    return;
                case 11:
                    if (!_hasDeterminedQuality)
                        AddInterval(Interval.Minor(7));
                    AddInterval(Interval.Perfect(4));
                    return;
                case 13:
                    if (!_hasDeterminedQuality)
                        AddInterval(Interval.Minor(7));
                    AddInterval(Interval.Major(6));
                    return;
            }
        }

        private void ReadBaseOrNumber()
        {
            switch (LA(1))
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                    ReadBase();
                    return;
                case '1':
                case '2':
                case '4':
                case '5':
                case '6':
                case '7':
                case '9':
                    Consume();//get rid of the '/'
                    ReadNumber();
                    return;
                case 'b':
                case '-':
                    ReadFlat();
                    return;
                case '#':
                case '+':
                    ReadSharp();
                    return;
            }

            throw ParseException("A", "B", "C", "D", "E", "F", "G", "1", "2", "4", "5", "6", "7", "9", "b", "#");
        }

        private void ReadBase()
        {
            Consume("/");

            var note = ReadNote();
            if (note == null)
                throw ParseException("A", "B", "C", "D", "E", "F", "G");

            _baseNote = note;
        }

        private void ReadSharp()
        {
            Consume("#", "+");

            var number = Consume(2, 4, 5, 6, 9, 11, 13);
            switch (number)
            {
                case 2:
                case 9:
                    AddInterval(Interval.Augmented(2));
                    return;
                case 4:
                case 11:
                    AddInterval(Interval.Augmented(4));
                    return;
                case 5:
                    AddInterval(Interval.Augmented(5));
                    return;
                case 6:
                case 13:
                    AddInterval(Interval.Augmented(6));
                    return;
            }
        }

        private void ReadFlat()
        {
            Consume("b", "-");

            var number = Consume(2, 4, 5, 6, 9, 11, 13);
            switch (number)
            {
                case 2:
                case 9:
                    AddInterval(Interval.Minor(2));
                    return;
                case 4:
                case 11:
                    AddInterval(Interval.Augmented(4));
                    return;
                case 5:
                    AddInterval(Interval.Diminished(5));
                    return;
                case 6:
                case 13:
                    AddInterval(Interval.Minor(6));
                    return;
            }
        }

        private void AddInterval(Interval interval)
        {
            _intervals[interval.Value] = interval;
        }

        private int Consume(params int[] expected)
        {
            var num = Consume(expected.Select(x => x.ToString()).ToArray());
            return int.Parse(num);
        }

        private string Consume(params string[] expected)
        {
            string consumed = "";
            var maxLength = expected.Max(x => x.Length);
            for (int i = 0; i < maxLength; i++)
            {
                if (LA(0) == EOF)
                    break;
                consumed += Consume();
                if (expected.Any(x => x == consumed))
                    return consumed;
            }

            throw ParseException(expected);
        }

        private Exception ParseException(params string[] expected)
        {
            if (expected.Length > 1)
            {
                return new ChordParseException(string.Format(
                    "After {0}, we expected one of [{1}] but found '{2}'.",
                    _input,
                    string.Join(", ", expected),
                    LA(0)));
            }
            else
            {
                return new ChordParseException(string.Format(
                    "After {0}, we expected '{1}' but found '{2}'.",
                    _input,
                    expected[0],
                    LA(0)));
            }
        }

        private char LA(int count)
        {
            if ((_pos + count) >= _input.Length)
            {
                return EOF;
            }

            return _input[_pos + count];
        }

        private char Consume()
        {
            var ret = LA(0);
            _pos++;
            return ret;
        }

        private char Consume(int count)
        {
            var ret = LA(0);
            while (count-- > 0)
                ret = Consume();

            return ret;
        }
    }
}
