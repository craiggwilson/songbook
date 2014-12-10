using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Theory;

namespace Songbook.Structure.Parsing
{
    public class ChordParser
    {
        const char EOF = '\0';

        private readonly IInputStream<char> _input;
        private Note _rootNote;
        private bool _hasDeterminedQuality;
        private Dictionary<int, ChordInterval> _intervals;
        private Note _baseNote;

        public ChordParser(IInputStream<char> input)
        {
            _input = input;
            _intervals = new Dictionary<int, ChordInterval>();
        }

        public Chord Parse()
        {
            _input.Mark();

            _rootNote = ReadNote();
            if (_rootNote == null)
                throw ParseException("A", "B", "C", "D", "E", "F", "G");

            //add default triad intervals...
            AddInterval(ChordInterval.Perfect(1));
            AddInterval(ChordInterval.Major(3));
            AddInterval(ChordInterval.Perfect(5));

            ReadInitialIntervals();
            _hasDeterminedQuality = true;

            while (_input.LA(0) != EOF)
                ReadAdditionalIntervals();

            var name = new string(_input.ClearMark());
            return new Chord(name, _rootNote, _intervals.Values.OrderBy(x => x.Value), _baseNote);
        }

        private Note ReadNote()
        {
            string note;
            switch (_input.LA(0))
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                    note = _input.Consume().ToString();
                    break;
                default:
                    return null;
            }

            switch (_input.LA(0))
            {
                case '#':
                    if (note != "B" && note != "E")
                    {
                        note += _input.Consume();
                    }
                    break;
                case 'b':
                    if (note != "C" && note != "F")
                    {
                        note += _input.Consume();
                    }
                    break;
            }

            return Note.FromName(note);
        }

        private void ReadInitialIntervals()
        {
            switch (_input.LA(0))
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
            switch (_input.LA(0))
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
                    AddInterval(ChordInterval.Major(2));
                    return;
                case 4:
                    AddInterval(ChordInterval.Perfect(4));
                    return;
            }
        }

        private void ReadDiminishedOrDominant()
        {
            switch (_input.LA(1))
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
                AddInterval(ChordInterval.Minor(3));
                AddInterval(ChordInterval.Diminished(5));
            }

            if (_input.LA(0) == '7')
            {
                _input.Consume();
                AddInterval(ChordInterval.Diminished(7));
            }
        }

        private void ReadDominant()
        {
            Consume("dom");
        }

        private void ReadAugmentedOrAdd()
        {
            switch (_input.LA(1))
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

            AddInterval(ChordInterval.Major(3));
            AddInterval(ChordInterval.Augmented(5));

            if (_input.LA(0) == '7')
            {
                _input.Consume();
                AddInterval(ChordInterval.Minor(7));
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
                    AddInterval(ChordInterval.Major(2));
                    break;
                case 4:
                case 11:
                    AddInterval(ChordInterval.Perfect(4));
                    break;
                case 6:
                case 13:
                    AddInterval(ChordInterval.Major(6));
                    break;
            }
        }

        private void ReadMajorOrMinor()
        {
            switch (_input.LA(1))
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
                AddInterval(ChordInterval.Major(7));
            }
            else
            {
                if (_input.LA(0) == '7')
                {
                    _input.Consume();
                    AddInterval(ChordInterval.Major(7));
                }
                else if (_input.LA(0) == '9')
                {
                    _input.Consume();
                    AddInterval(ChordInterval.Major(7));
                    AddInterval(ChordInterval.Major(2));
                }
                else if (_input.LA(0) == '1' && _input.LA(1) == '1')
                {
                    _input.Consume(2);
                    AddInterval(ChordInterval.Major(7));
                    AddInterval(ChordInterval.Perfect(4));
                }
                else if (_input.LA(0) == '1' && _input.LA(1) == '3')
                {
                    _input.Consume(2);
                    AddInterval(ChordInterval.Major(7));
                    AddInterval(ChordInterval.Major(6));
                }
            }
        }

        private void ReadMinor()
        {
            if (_input.LA(0) == 'm' && _input.LA(1) == 'i')
                Consume("min");
            else
                Consume("-", "m");

            AddInterval(ChordInterval.Minor(3));

            if (_input.LA(0) == '9')
            {
                _input.Consume();
                AddInterval(ChordInterval.Minor(7));
                AddInterval(ChordInterval.Major(2));
            }
            else if (_input.LA(0) == '1' && _input.LA(1) == '1')
            {
                _input.Consume(2);
                AddInterval(ChordInterval.Minor(7));
                AddInterval(ChordInterval.Perfect(4));
            }
            else if (_input.LA(0) == '1' && _input.LA(1) == '3')
            {
                _input.Consume(2);
                AddInterval(ChordInterval.Minor(7));
                AddInterval(ChordInterval.Major(6));
            }
        }

        private void ReadNumber()
        {
            int num = Consume(2, 4, 5, 6, 7, 9, 11, 13);

            switch (num)
            {
                case 2:
                    AddInterval(ChordInterval.Major(2));
                    return;
                case 4:
                    AddInterval(ChordInterval.Perfect(4));
                    return;
                case 6:
                    AddInterval(ChordInterval.Major(6));
                    return;
                case 7:
                    AddInterval(ChordInterval.Minor(7));
                    return;
                case 9:
                    if (!_hasDeterminedQuality)
                        AddInterval(ChordInterval.Minor(7));
                    AddInterval(ChordInterval.Major(2));
                    return;
                case 11:
                    if (!_hasDeterminedQuality)
                        AddInterval(ChordInterval.Minor(7));
                    AddInterval(ChordInterval.Perfect(4));
                    return;
                case 13:
                    if (!_hasDeterminedQuality)
                        AddInterval(ChordInterval.Minor(7));
                    AddInterval(ChordInterval.Major(6));
                    return;
            }
        }

        private void ReadBaseOrNumber()
        {
            switch (_input.LA(1))
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
                    _input.Consume();//get rid of the '/'
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
                    AddInterval(ChordInterval.Augmented(2));
                    return;
                case 4:
                case 11:
                    AddInterval(ChordInterval.Augmented(4));
                    return;
                case 5:
                    AddInterval(ChordInterval.Augmented(5));
                    return;
                case 6:
                case 13:
                    AddInterval(ChordInterval.Augmented(6));
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
                    AddInterval(ChordInterval.Minor(2));
                    return;
                case 4:
                case 11:
                    AddInterval(ChordInterval.Augmented(4));
                    return;
                case 5:
                    AddInterval(ChordInterval.Diminished(5));
                    return;
                case 6:
                case 13:
                    AddInterval(ChordInterval.Minor(6));
                    return;
            }
        }

        private void AddInterval(ChordInterval interval)
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
                if (_input.LA(0) == EOF)
                    break;
                consumed += _input.Consume();
                if (expected.Any(x => x == consumed))
                    return consumed;
            }

            throw ParseException(expected);
        }

        private Exception ParseException(params string[] expected)
        {
            var current = new string(_input.ClearMark());
            if (expected.Length > 1)
            {
                return new ParseException(string.Format(
                    "After {0}, we expected one of [{1}] but found '{2}'.",
                    current,
                    string.Join(", ", expected),
                    _input.LA(0)));
            }
            else
            {
                return new ParseException(string.Format(
                    "After {0}, we expected '{1}' but found '{2}'.",
                    current,
                    expected[0],
                    _input.LA(0)));
            }
        }
    }
}
