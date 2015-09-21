using System;
using System.Runtime.Serialization;

namespace Songbook.Theory
{
    [Serializable]
    public class ChordParseException : Exception
    {
        public ChordParseException()
        {
        }

        public ChordParseException(string message) : base(message)
        {
        }

        public ChordParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChordParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}