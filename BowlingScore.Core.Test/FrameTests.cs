namespace BowlingScore.Core.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FrameTests
    {
        [TestMethod]
        [ExpectedException(typeof(BowlingScoreException))]
        public void RollingACompletedFrameThrows()
        {
            var frame = new StandardFrame(1);
            Assert.IsTrue(frame.RollFirst(0, new ReadOnlyMemory<uint>(new uint[3])));
            Assert.IsTrue(frame.RollAgain(0));
            frame.RollAgain(0);
        }

        [TestMethod]
        [ExpectedException(typeof(BowlingScoreException))]
        public void RollFirstCanOnkyBeCalledOnce()
        {
            var frame = new StandardFrame(1);
            Assert.IsTrue(frame.RollFirst(0, new ReadOnlyMemory<uint>(new uint[3])));
            frame.RollFirst(0, new ReadOnlyMemory<uint>(new uint[3]));
        }

        [TestMethod]
        [ExpectedException(typeof(BowlingScoreException))]
        public void RollAgainCanNotBeCalledFirst()
        {
            var frame = new StandardFrame(1);
            frame.RollAgain(0);
        }
    }
}
