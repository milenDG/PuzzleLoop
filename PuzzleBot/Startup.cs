namespace PuzzleBot
{
    using System;
    using Common;
    using PuzzleBot.BoardClasses;

    internal class Startup
    {
        private static void Main()
        {
            Board board = BoardReader.ReadFromFile(@"Puzzles\25x30.txt");
            //Board board = BoardReader.ReadFromConsole();
            board.Solve();
            Console.WriteLine(board.Print(true, true));
        }
    }
}
