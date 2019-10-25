namespace PuzzleBot.Common
{
    using System;
    using System.Collections.Generic;
    using BoardClasses;

    public static class Utils
    {
        public static bool GenericRule(Cell[,] cells, Func<Cell, bool> method, int? cellState)
        {
            bool hasChanged = false;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (!cells[i, j].IsReady)
                    {
                        if (cellState.HasValue && (int)cells[i, j].CellState != cellState)
                        {
                            continue;
                        }
                        Cell cell90 = Cell.Turn90(cells[i, j]),
                            cell180 = Cell.Turn90(cell90),
                            cell270 = Cell.Turn90(cell180);
                        if (method(cells[i, j])
                            || method(cell90)
                            || method(cell180)
                            || method(cell270))
                        {
                            hasChanged = true;
                        }
                    }
                }
            }

            return hasChanged;
        }

        public static int LowestNotContainedNumber(ICollection<int> set)
        {
            int i = 1;
            while (set.Contains(i))
            {
                i++;
            }

            return i;
        }

    }
}
