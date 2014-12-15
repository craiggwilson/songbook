using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songbook.Theory
{
    public class ChordNamer
    {
        public static string GetProperName(Chord chord)
        {
            StringBuilder sb = new StringBuilder();

            var ri = chord.Intervals.Where(x => x.Value != 1).ToDictionary(x => x.Value);

            bool contains3 = ri.ContainsKey(3);

            if (ri.ContainsKey(3) && ri[3].Kind == IntervalKind.Minor)
            {
                if (ri.ContainsKey(5) && ri[5].Kind == IntervalKind.Diminished)
                {
                    ri.Remove(5);
                    if (ri.ContainsKey(7))
                    {
                        if (ri[7].Kind == IntervalKind.Minor)
                        {
                            sb.Append("m7(b5)");
                            ri.Remove(7);
                        }
                        else if (ri[7].Kind == IntervalKind.Diminished)
                        {
                            sb.Append("dim7");
                            ri.Remove(7);
                        }
                    }
                    else
                        sb.Append("dim");
                }
                else
                {
                    sb.Append("m");
                    if (ri.ContainsKey(7) && ri[7].Kind == IntervalKind.Major)
                    {
                        sb.Append("M7");
                        ri.Remove(7);
                    }
                }

                ri.Remove(3);
            }
            else if (ri.ContainsKey(3))
            {
                ri.Remove(3);
            }

            if (ri.ContainsKey(7))
            {
                if (ri[7].Kind == IntervalKind.Major)
                {
                    sb.Append("maj");
                }

                if (ri.ContainsKey(6) && ri[6].Kind == IntervalKind.Major)
                {
                    sb.Append("13");
                    ri.Remove(6);
                    if (ri.ContainsKey(4) && ri[4].Kind == IntervalKind.Perfect)
                        ri.Remove(4);
                    if (ri.ContainsKey(2) && ri[2].Kind == IntervalKind.Major)
                        ri.Remove(2);
                }
                else if (ri.ContainsKey(4) && ri[4].Kind == IntervalKind.Perfect)
                {
                    sb.Append("11");
                    ri.Remove(4);
                    if (ri.ContainsKey(2) && ri[2].Kind == IntervalKind.Major)
                        ri.Remove(2);
                }
                else if (ri.ContainsKey(2) && ri[2].Kind == IntervalKind.Major)
                {
                    sb.Append("9");
                    ri.Remove(2);
                }
                else
                    sb.Append("7");

                ri.Remove(7);
            }

            if (ri.ContainsKey(5) && ri[5].Kind == IntervalKind.Augmented && sb.Length == 0)
            {
                sb.Append("+");
                ri.Remove(5);
            }

            if (ri.ContainsKey(2))
            {
                if (ri[2].Kind == IntervalKind.Major)
                {
                    if (!contains3)
                        sb.Append("sus2");
                    else if (sb.Length == 0)
                        sb.Append("2");
                    else
                        sb.Append("add2");
                }
                else if (ri[2].Kind == IntervalKind.Minor)
                {
                    sb.Append("(b9)");
                }
                else if (ri[2].Kind == IntervalKind.Augmented)
                {
                    sb.Append("(#9)");
                }

                ri.Remove(2);
            }

            if (ri.ContainsKey(4))
            {
                if (ri[4].Kind == IntervalKind.Perfect)
                {
                    if (!contains3)
                        sb.Append("sus4");
                    else if (sb.Length == 0)
                        sb.Append("4");
                    else
                        sb.Append("add4");
                }
                else if (ri[4].Kind == IntervalKind.Diminished)
                {
                    sb.Append("(b11)");
                }
                else if (ri[4].Kind == IntervalKind.Augmented)
                {
                    sb.Append("(#11)");
                }

                ri.Remove(4);
            }

            if (ri.ContainsKey(6))
            {
                if (ri[6].Kind == IntervalKind.Major)
                {
                    if (sb.Length == 0)
                        sb.Append("6");
                    else
                        sb.Append("add6");
                }
                else if (ri[4].Kind == IntervalKind.Diminished)
                {
                    sb.Append("(b13)");
                }
                else if (ri[4].Kind == IntervalKind.Augmented)
                {
                    sb.Append("(#13)");
                }

                ri.Remove(6);
            }

            if (ri.ContainsKey(5))
            {
                if (ri[5].Kind == IntervalKind.Augmented)
                    sb.Append("(#5)");
                else if (ri[5].Kind == IntervalKind.Diminished)
                    sb.Append("(b5)");

                ri.Remove(5);
            }

            sb.Insert(0, chord.RootNote.Name);
            if (chord.BaseNote != null)
                sb.Append("/").Append(chord.BaseNote.Name);

            if (ri.Count > 0)
                throw new Exception("Unable to write all the intervals in the chord.");

            return sb.ToString();
        }
    }
}
