namespace BowlingScore.Core
{
    using System;

    /// <summary>
    /// An individual frame in a game of bowling
    /// </summary>
    public abstract class Frame
    {
        /// <summary>
        /// This frame's view into the rolls data
        /// </summary>
        protected ReadOnlyMemory<uint> Rolls { get; set; }

        /// <summary>
        /// The frame number 1-10
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// How many rolls have been played in this frame
        /// </summary>
        public int RollCount { get; protected set; }

        /// <summary>
        /// True if this frame has at least one roll
        /// </summary>
        public bool HasStarted => this.RollCount > 0;

        /// <summary>
        /// True if there are no more rolls in this frame
        /// </summary>
        public bool IsComplete { get; protected set; }

        /// <summary>
        /// The bowling score for this frame, taking into account any bonus points
        /// </summary>
        public abstract int Score { get; }

        /// <summary>
        /// Create a new instance of a frame
        /// </summary>
        /// <param name="number">The frame number this frame 1-10</param>
        protected Frame(int number) => this.Number = number;

        /// <summary>
        /// This function is called to record an initial roll in this frame.  It should only be called once per frame.
        /// </summary>
        /// <param name="pins">The number of pins knocked down in this roll</param>
        /// <param name="rolls">This frame's view into the rolls data</param>
        /// <returns>True if this was a valid roll</returns>
        public bool RollFirst(uint pins, ReadOnlyMemory<uint> rolls)
        {
            this.TestValidRoll(true);
            this.Rolls = rolls;
            return this.Roll(pins);
        }

        /// <summary>
        /// This function is called to record an aditional roll in this frame.  It should only be called after FirstRoll.
        /// </summary>
        /// <param name="pins">The number of pins knocked down in this roll</param>
        /// <returns>True if this was a valid roll</returns>
        public bool RollAgain(uint pins)
        {
            this.TestValidRoll(false);
            return this.Roll(pins);
        }

        private void TestValidRoll(bool isFirst)
        {
            if (this.IsComplete)
                throw new BowlingScoreException("Cannot roll in a completed frame");

            if (isFirst == this.HasStarted)
                throw new BowlingScoreException(isFirst ? $"Cannot call {nameof(RollFirst)} twice on the same frame" : $"Cannot call {nameof(RollAgain)} before calling {nameof(RollFirst)}");
        }

        /// <summary>
        /// Handles actual frame logic
        /// </summary>
        /// <param name="pins">The number of pins knocked down in this roll</param>
        /// <returns>True if this was a valid roll</returns>
        protected abstract bool Roll(uint pins);

        public override string ToString() => $"Frame: {this.Number}  Score: {this.Score}";
    }
}
