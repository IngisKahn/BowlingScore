namespace BowlingScore.Core.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void GameHas10Frames()
        {
            var game = new Game();
            var frames = game.Frames;
            Assert.AreEqual(10, frames.Count);
        }

        [TestMethod]
        public void InitialFrameIs1()
        {
            var game = new Game();
            var frame = game.CurrentFrame;
            Assert.AreEqual(1, frame.Number);
        }

        [TestMethod]
        public void KnockingAPinChnagesScore()
        {
            var game = new Game();
            var initialScore = game.Score;
            Assert.IsTrue(game.Roll(1));
            Assert.AreNotEqual(initialScore, game.Score);
        }

        [TestMethod]
        public void TwoGutterBallsAdvancesFrame()
        {
            var game = new Game();
            var initialFrame = game.CurrentFrame.Number;
            Assert.IsTrue(game.Roll(0));
            Assert.IsTrue(game.Roll(0));
            Assert.AreEqual(initialFrame + 1, game.CurrentFrame.Number);
        }

        [TestMethod]
        public void TwentyGutterBallsEndsGame()
        {
            var game = new Game();
            for (var x = 0; x < 20; x++)
            {
                Assert.IsFalse(game.IsComplete);
                Assert.IsTrue(game.Roll(0));
            }
            Assert.IsTrue(game.IsComplete);
        }
    }
}
