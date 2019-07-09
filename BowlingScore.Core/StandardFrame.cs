namespace BowlingScore.Core
{
    /// <summary>
    /// A standard frame (1-9) in a game of bowling
    /// </summary>
    public class StandardFrame : Frame
    {

        /// <summary>
        /// The bowling score for this frame, taking into account any bonus points
        /// </summary>
        public override int Score
        {
            get
            {
                if (!this.HasStarted)
                    return 0;
                var firstRoll = (int)this.Rolls.Span[0];
                if (firstRoll == 10) // strike
                    return (int)(10 + this.Rolls.Span[1] + this.Rolls.Span[2]);
                var bothRolls = firstRoll + (int)this.Rolls.Span[1];
                return bothRolls < 10 ? bothRolls : 10 + (int)this.Rolls.Span[2]; // open or spare
            }
        }

        /// <summary>
        /// Create a new instance of a frame
        /// </summary>
        /// <param name="number">The frame number this frame 1-10</param>
        public StandardFrame(int number) : base(number) { }

        /// <summary>
        /// Handles actual frame logic
        /// </summary>
        /// <param name="pins">The number of pins knocked down in this roll</param>
        /// <returns>True if this was a valid roll</returns>
        protected override bool Roll(uint pins)
        {
            // how many pins have been knocked down so far
            var totalPins = pins + (this.RollCount == 1 ? this.Rolls.Span[0] : 0); 

            if (totalPins > 10)
                return false;

            this.IsComplete = ++this.RollCount == 2 || totalPins == 10; // handles ope frame or strike/spare

            return true;
        }
    }
}
