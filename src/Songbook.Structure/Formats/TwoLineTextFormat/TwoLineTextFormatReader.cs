using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Songbook.Structure;
using Songbook.Text;

namespace Songbook.Formats.TwoLineTextFormat
{
    internal class TwoLineTextFormatReader
    {
        private readonly IInputStream<Token> _input;

        public TwoLineTextFormatReader(IInputStream<Token> input)
        {
            _input = input;
        }

        public SongNode Parse()
        {
            var properties = ReadProperties();
            var sections = ReadSections();

            return new SongNode(properties, sections);
        }

        private IEnumerable<PropertyNode> ReadProperties()
        {
            var properties = new List<PropertyNode>();
            while (_input.LA(0).Kind == TokenKind.Text)
            {
                if (_input.LA(0).Text.StartsWith("#"))
                {
                    var sb = new StringBuilder();
                    while (_input.LA(0).Kind != TokenKind.EndOfLine && _input.LA(0).Kind != TokenKind.EOF)
                    {
                        sb.Append(_input.LA(0).Text);
                        _input.Consume();
                    }
                    _input.Consume(); // end of Line

                    var parts = sb.ToString().Split('=');
                    var propName = parts[0].Substring(1);
                    var value = parts[1];
                    properties.Add(new PropertyNode(propName, new List<string> { value }.AsReadOnly()));
                    continue;
                }

                break;
            }

            return properties;
        }

        private IEnumerable<SectionNode> ReadSections()
        {
            var allLines = ReadLines();

            var sections = new List<SectionNode>();
            var sectionLines = new List<LineNode>();
            bool lastIsBlankLine = false;
            foreach (var line in allLines)
            {
                if (lastIsBlankLine)
                {
                    sections.Add(CreateSectionNode(sectionLines));
                    sectionLines.Clear();
                }

                sectionLines.Add(line);
                lastIsBlankLine = line.IsBlank;
            }

            sections.Add(CreateSectionNode(sectionLines));
            return sections;
        }

        private SectionNode CreateSectionNode(List<LineNode> lines)
        {
            return new SectionNode(null, lines);
        }

        private IEnumerable<LineNode> ReadLines()
        {
            var lines = new List<LineNode>();
            while (_input.LA(0).Kind != TokenKind.EOF)
            {
                lines.Add(ReadLine());
            }
            return lines;
        }

        private LineNode ReadLine()
        {
            var parts = new List<LinePartNode>();

            ReadLineStart:
            switch (_input.LA(0).Kind)
            {
                case TokenKind.Text:
                    var wordToken = _input.Consume();
                    parts.Add(new TextNode(wordToken.Text));
                    goto ReadLineStart;
                case TokenKind.WhiteSpace:
                    var whiteSpaceToken = _input.Consume();
                    parts.Add(new WhiteSpaceNode(whiteSpaceToken.Text));
                    goto ReadLineStart;
                case TokenKind.EndOfLine:
                    _input.Consume();
                    break;
            }

            return new LineNode(parts);
        }
    }
}
