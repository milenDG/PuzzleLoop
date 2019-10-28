namespace PuzzleBot.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using BoardClasses;

    public class BoardReader
    {
        public static Board ReadFromFile(string path)
        {
            string[] content = File.ReadAllText(path).Split(new []{Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);

            string[] inputDimensions = content[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int height = int.Parse(inputDimensions?[0] ?? throw new InvalidOperationException());
            int width = int.Parse(inputDimensions[1]);

            int[,] cellNumbers = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                var column = content[i + 1]
                    ?.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                for (int j = 0; j < width; j++)
                {
                    if (column != null) cellNumbers[i, j] = column[j];
                }
            }

            return new Board(height, width, cellNumbers);
        }

        public static Board ReadFromConsole()
        {
            string[] inputDimensions = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int height = int.Parse(inputDimensions[0]);
            int width = int.Parse(inputDimensions[1]);

            int[,] cellNumbers = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                var column = Console.ReadLine()
                    ?.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                for (int j = 0; j < width; j++)
                {
                    if (column != null) cellNumbers[i, j] = column[j];
                }
            }

            return new Board(height, width, cellNumbers);
        }
    }
}
