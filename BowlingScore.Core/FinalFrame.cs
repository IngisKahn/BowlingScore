namespace BowlingScore.Core
{
    /// <summary>
    /// The final frame in a game of bowling
    /// </summary>
    public class FinalFrame : Frame
    {
        /// <summary>
        /// The bowling score for this frame
        /// </summary>
        public override int Score => this.HasStarted ? (int)(this.Rolls.Span[0] + this.Rolls.Span[1] + this.Rolls.Span[2]) : 0;
        
        /// <summary>
        /// Create a new instance of a tenth frame
        /// </summary>
        public FinalFrame() : base(10) { }

        /// <summary>
        /// Handles actual frame logic
        /// </summary>
        /// <param name="pins">The number of pins knocked down in this roll</param>
        /// <returns>True if this was a valid roll</returns>
        protected override bool Roll(uint pins)
        {
            if (pins > 10)
                return false;

            switch (++this.RollCount)
            {
                case 1: // we always have a second roll
                    break;
                case 2:
                    var firstRoll = this.Rolls.Span[0];

                    if (firstRoll < 10) // first roll was not a strike, detrmine if we have a thrid roll
                    {
                        var totalPins = firstRoll + pins;

                        if (totalPins > 10)
                            return false;

                        this.IsComplete = totalPins != 10;
                    }                    
                    break;
                default: // third roll ends it
                    this.IsComplete = true;
                    break;
            }

            return true;
        }
    }
}
