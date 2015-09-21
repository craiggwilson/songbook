namespace Songbook.Formats.TwoLineTextFormat
{
    public class Token
    {
        public TokenKind Kind { get; private set; }

        public string Text { get; private set; }

        public Token(TokenKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }
    }
}
