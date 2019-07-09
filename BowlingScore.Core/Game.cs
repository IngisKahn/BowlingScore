namespace BowlingScore.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a game of bowling
    /// </summary>
    public class Game
    {
        private readonly Frame[] frames = new Frame[10];

        private readonly uint[] rolls = new uint[21]; // maximum rolls in a game 

        /// <summary>
        /// The individual rolls in this game
        /// </summary>
        public IReadOnlyList<uint> Rolls => Array.AsReadOnly(this.rolls);

        private int currentRoll;

        /// <summary>
        /// The ten frames in this game
        /// </summary>
        public IReadOnlyList<Frame> Frames => Array.AsReadOnly(this.frames);

        /// <summary>
        /// The current frame in this game where the next roll will take place
        /// </summary>
        public Frame CurrentFrame { get; private set; }

        /// <summary>
        /// The value of the last roll entered
        /// </summary>
        public uint LastRoll => this.currentRoll > 0 ? this.rolls[this.currentRoll - 1] : 0;

        /// <summary>
        /// True when the tenth frame is complete and there are no more rols left in the game
        /// </summary>
        public bool IsComplete => this.frames[9].IsComplete;

        /// <summary>
        /// The total score of this game
        /// </summary>
        public int Score => this.frames.Sum(f => f.Score);

        /// <summary>
        /// Create a new instance of Game
        /// </summary>
        public Game()
        {
            for (var x = 0; x < 9;)
                this.frames[x] = new StandardFrame(++x);

            this.frames[9] = new FinalFrame();

            this.CurrentFrame = this.frames[0];
        }

        /// <summary>
        /// Perform a roll, it will be scored to the appropriate frame
        /// </summary>
        /// <param name="pins">The number of pins knocked down in this roll</param>
        /// <returns>True if this was a valid roll</returns>
        public bool Roll(uint pins)
        {
            if (this.IsComplete)
                throw new BowlingScoreException("Cannot roll in a completed game");

            var frame = this.CurrentFrame;

            // roll in the frame, if it is the first time, we need to pass a view into the roll amounts for scoring
            if (frame.HasStarted)
            {
                if (!frame.RollAgain(pins))
                    return false;
            }
            else if (!frame.RollFirst(pins, new ReadOnlyMemory<uint>(this.rolls, this.currentRoll, 3)))
                return false;

            
            // if this roll completes the current frame and there are more left, advance to the next frame
            if (frame.IsComplete && frame.Number < 10)
                this.CurrentFrame = this.frames[frame.Number];

            this.rolls[this.currentRoll++] = pins;

            return true;
        }

        public override string ToString() => $"Current Frame: {this.CurrentFrame.Number}  Score: {this.Score}";
    }
}
