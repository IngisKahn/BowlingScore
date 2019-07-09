namespace BowlingScore.Core
{
    using System;

    [Serializable]
    public class BowlingScoreException : Exception
    {
        public BowlingScoreException() { }
        public BowlingScoreException(string message) : base(message) { }
        public BowlingScoreException(string message, Exception inner) : base(message, inner) { }
        protected BowlingScoreException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
