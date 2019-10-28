using NUnit.Framework;

namespace PuzzleBot.Test
{
    using BoardClasses;
    using Common;

    public class OverallBoardSolvingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Board board = BoardReader.ReadFromFile(@"..\PuzzleBot.App\Puzzles\25x30.txt");
            board.Solve();
            string solution = board.Print(false, false).ToString();
            Assert.AreEqual(solution, );
        }
    }
}