namespace BowlingScore.Core.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FinalFrameTests
    {
        [TestMethod]
        public void CannotRollMoreThan10()
        {
            var frame = new FinalFrame();
            Assert.IsFalse(frame.RollFirst(11, new ReadOnlyMemory<uint>(new uint[3])));
        }
        [TestMethod]
        public void CannotRollOver10InTwoRolls()
        {
            var frame = new FinalFrame();
            var rolls = new uint[3];
            Assert.IsTrue(frame.RollFirst(9, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 9;
            Assert.IsFalse(frame.RollAgain(2));
        }

        [TestMethod]
        public void TwoGutterBallsEndsFrame()
        {
            var frame = new FinalFrame();
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollFirst(0, new ReadOnlyMemory<uint>(new uint[3])));
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollAgain(0));
            Assert.IsTrue(frame.IsComplete);
        }

        [TestMethod]
        public void StrikeAllows2MoreRolls()
        {
            var frame = new FinalFrame();
            var rolls = new uint[3];
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollFirst(10, new ReadOnlyMemory<uint>(rolls)));
            Assert.IsFalse(frame.IsComplete);
            rolls[0] = 10;
            Assert.IsTrue(frame.RollAgain(0));
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollAgain(0));
            Assert.IsTrue(frame.IsComplete);
        }

        [TestMethod]
        public void SpareAllows1MoreRoll()
        {
            var frame = new FinalFrame();
            var rolls = new uint[3];
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollFirst(1, new ReadOnlyMemory<uint>(rolls)));
            Assert.IsFalse(frame.IsComplete);
            rolls[0] = 1;
            Assert.IsTrue(frame.RollAgain(9));
            Assert.IsFalse(frame.IsComplete);
            rolls[1] = 9;
            Assert.IsTrue(frame.RollAgain(0));
            Assert.IsTrue(frame.IsComplete);
        }

        [TestMethod]
        public void OpenFrameScoresCorrect()
        {
            var frame = new FinalFrame();
            var rolls = new uint[3];
            Assert.AreEqual(0, frame.Score);
            Assert.IsTrue(frame.RollFirst(4, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 4;
            Assert.AreEqual(4, frame.Score);
            Assert.IsTrue(frame.RollAgain(5));
            rolls[1] = 5;
            Assert.AreEqual(9, frame.Score);
        }

        [TestMethod]
        public void SpareScoresCorrect()
        {
            var frame = new FinalFrame();
            var rolls = new uint[3];
            Assert.AreEqual(0, frame.Score);
            Assert.IsTrue(frame.RollFirst(1, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 1;
            Assert.AreEqual(1, frame.Score);
            Assert.IsTrue(frame.RollAgain(9));
            rolls[1] = 9;
            Assert.AreEqual(10, frame.Score);
            Assert.IsTrue(frame.RollAgain(7));
            rolls[2] = 7;
            Assert.AreEqual(17, frame.Score);
        }

        [TestMethod]
        public void StrikeScoresCorrect()
        {
            var frame = new FinalFrame();
            var rolls = new uint[3];
            Assert.AreEqual(0, frame.Score);
            Assert.IsTrue(frame.RollFirst(10, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 10;
            Assert.AreEqual(10, frame.Score);
            Assert.IsTrue(frame.RollAgain(9));
            rolls[1] = 9;
            Assert.AreEqual(19, frame.Score);
            Assert.IsTrue(frame.RollAgain(8));
            rolls[2] = 8;
            Assert.AreEqual(27, frame.Score);
        }
    }
}
