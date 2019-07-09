namespace BowlingScore.Core.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StandardFrameTests
    {
        [TestMethod]
        public void CannotRollMoreThan10()
        {
            var frame = new StandardFrame(1);
            Assert.IsFalse(frame.RollFirst(11, new ReadOnlyMemory<uint>(new uint[3])));
        }
        [TestMethod]
        public void CannotRollOver10InTwoRolls()
        {
            var frame = new StandardFrame(1);
            var rolls = new uint[3];
            Assert.IsTrue(frame.RollFirst(9, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 9;
            Assert.IsFalse(frame.RollAgain(2));
        }

        [TestMethod]
        public void TwoGutterBallsEndsFrame()
        {
            var frame = new StandardFrame(1);
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollFirst(0, new ReadOnlyMemory<uint>(new uint[3])));
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollAgain(0));
            Assert.IsTrue(frame.IsComplete);
        }

        [TestMethod]
        public void StrikeEndsFrame()
        {
            var frame = new StandardFrame(1);
            Assert.IsFalse(frame.IsComplete);
            Assert.IsTrue(frame.RollFirst(10, new ReadOnlyMemory<uint>(new uint[3])));
            Assert.IsTrue(frame.IsComplete);
        }

        [TestMethod]
        public void OpenFrameScoresCorrect()
        {
            var frame = new StandardFrame(1);
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
            var frame = new StandardFrame(1);
            var rolls = new uint[3];
            Assert.AreEqual(0, frame.Score);
            Assert.IsTrue(frame.RollFirst(1, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 1;
            Assert.AreEqual(1, frame.Score);
            Assert.IsTrue(frame.RollAgain(9));
            rolls[1] = 9;
            rolls[2] = 7;
            Assert.AreEqual(17, frame.Score);
        }

        [TestMethod]
        public void StrikeScoresCorrect()
        {
            var frame = new StandardFrame(1);
            var rolls = new uint[3];
            Assert.AreEqual(0, frame.Score);
            Assert.IsTrue(frame.RollFirst(10, new ReadOnlyMemory<uint>(rolls)));
            rolls[0] = 10;
            rolls[1] = 2;
            rolls[2] = 7;
            Assert.AreEqual(19, frame.Score);
        }
    }
}
