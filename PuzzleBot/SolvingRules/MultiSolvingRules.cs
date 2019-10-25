namespace PuzzleBot.SolvingRules
{
    using System.Collections.Generic;
    using System.Linq;
    using PuzzleBot.BoardClasses;
    using PuzzleBot.Enums;
    using static Common.Utils;

    public class MultiSolvingRules
    {
        public static bool ReadyDots(Dot[,] dots)
        {
            bool hasChanged = false;
            foreach (var dot in dots)
            {
                if (dot.IsReady) continue;
                Dot dot90 = Dot.Turn90(dot), dot180 = Dot.Turn90(dot90), dot270 = Dot.Turn90(dot180);
                if (SubMultiSolvingRules.SubSetForbidsOfConnectedDot(dot)
                    | SubMultiSolvingRules.SubSetForbidsOfConnectedDot(dot90)
                    | SubMultiSolvingRules.SubSetForbidsOfConnectedDot(dot180)
                    | SubMultiSolvingRules.SubSetForbidsOfConnectedDot(dot270)
                    | SubMultiSolvingRules.SubSetLineOfForbiddenDot(dot)
                    | SubMultiSolvingRules.SubSetLineOfForbiddenDot(dot90)
                    | SubMultiSolvingRules.SubSetLineOfForbiddenDot(dot180)
                    | SubMultiSolvingRules.SubSetLineOfForbiddenDot(dot270)
                    | SubMultiSolvingRules.SubDotWithThreeForbidden(dot)
                    | SubMultiSolvingRules.SubDotWithThreeForbidden(dot90)
                    | SubMultiSolvingRules.SubDotWithThreeForbidden(dot180)
                    | SubMultiSolvingRules.SubDotWithThreeForbidden(dot270))
                {
                    dot.IsReady = true;
                    hasChanged = true;
                }
            }

            return hasChanged;
        }

        public static bool ReadyCells(Cell[,] cells)
        {
            bool hasChanged = false;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (!cells[i, j].IsReady && (int)cells[i, j].CellState != 4)
                    {
                        Cell cell90 = Cell.Turn90(cells[i, j]),
                            cell180 = Cell.Turn90(cell90),
                            cell270 = Cell.Turn90(cell180);
                        hasChanged = (int) cells[i, j].CellState switch
                        {
                            3 when SubMultiSolvingRules.SubReadyThreeCell(cells[i, j]) ||
                                   SubMultiSolvingRules.SubReadyThreeCell(cell90) ||
                                   SubMultiSolvingRules.SubReadyThreeCell(cell180) ||
                                   SubMultiSolvingRules.SubReadyThreeCell(cell270) => true,
                            2 when SubMultiSolvingRules.SubReadyTwoCell(cells[i, j]) ||
                                   SubMultiSolvingRules.SubReadyTwoCell(cell90) ||
                                   SubMultiSolvingRules.SubReadyTwoCell(cell180) ||
                                   SubMultiSolvingRules.SubReadyTwoCell(cell270) => true,
                            1 when SubMultiSolvingRules.SubReadyOneCell(cells[i, j]) ||
                                   SubMultiSolvingRules.SubReadyOneCell(cell90) ||
                                   SubMultiSolvingRules.SubReadyOneCell(cell180) ||
                                   SubMultiSolvingRules.SubReadyOneCell(cell270) => true,
                            _ => hasChanged
                        };
                    }
                }
            }

            return hasChanged;
        }

        public static bool ThreeCellWithConnectedNeighbourLine(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubThreeCellWithConnectedNeighbourLine, 3);

        public static bool OneCellWithConnectedNeighbourLine(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubOneCellWithConnectedNeighbourLine, 1);

        public static bool ThreeCellWithForbiddenDot(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubThreeCellWithForbiddenDot, 3);

        public static bool OneCellWithForbiddenDot(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubOneCellWithForbiddenDot, 1);

        public static bool ForbidWholeCells(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubForbidWholeCells, null);

        public static bool TwoCellWithForbiddenSide(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubTwoCellWithForbiddenSide, 2);

        public static bool TwoCellWithConnectedDot(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubTwoCellWithConnectedDot, 2);

        public static bool TwoCellWithForbiddenDot(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubTwoCellWithForbiddenDot, 2);

        public static bool TwoCellWithTwoConnectedDots(Cell[,] cells)
            => GenericRule(cells, SubMultiSolvingRules.SubTwoCellWithTwoConnectedDots, 2);

        public static void SetDotWeightAsTheLowestLine(Dot[,] dots)
        {
            foreach (Dot dot in dots)
            {
                dot.ResetWeight();
            }

            HashSet<int> currentWeights;
            int nextWeight = 1;
            bool hasChanged;
            do
            {
                hasChanged = false;
                currentWeights = dots.OfType<Dot>().Select(d => d.Weight).ToHashSet();
                foreach (Dot dot in dots)
                {
                    if (!dot.IsConnected()) continue;
                    if (dot.FixWeight())
                    {
                        hasChanged = true;
                    }

                    if (dot.Weight != 0) continue;
                    dot.Weight = nextWeight;
                    dot.FixWeight();
                    currentWeights.Add(nextWeight++);

                    hasChanged = true;
                }
            } while (hasChanged);

            nextWeight = LowestNotContainedNumber(currentWeights);

            foreach (Dot dot in dots)
            {
                if(dot.Weight == 0 || nextWeight >= dot.Weight) continue;

                int oldWeight = dot.Weight;
                dot.Weight = nextWeight;
                dot.FixWeight();

                currentWeights.Remove(oldWeight);
                currentWeights.Add(nextWeight);

                dots.OfType<Dot>()
                    .Where(d => d.Weight == oldWeight)
                    .ToList()
                    .ForEach(d =>
                    {
                        d.Weight = nextWeight;
                        d.FixWeight();
                    });

                nextWeight = LowestNotContainedNumber(currentWeights);
            }
        }

        public static bool ForbidDotsConnectionBetweenEqualWeightDots(Dot[,] dots)
        {
            bool hasChanged = false;
            for (int i = 0; i < dots.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < dots.GetLength(1) - 1; j++)
                {
                    if (!dots[i, j].IsReady)
                    {
                        if (dots[i, j].Weight == dots[i, j + 1].Weight
                            && dots[i, j].Weight > 0
                            && dots[i, j].RightLine.LineState == LineState.Disconnected)
                        {
                            dots[i, j].RightLine.Forbid();
                            hasChanged = true;
                        }

                        if (dots[i, j].Weight == dots[i + 1, j].Weight
                            && dots[i, j].Weight > 0
                            && dots[i, j].BottomLine.LineState == LineState.Disconnected)
                        {
                            dots[i, j].BottomLine.Forbid();
                            hasChanged = true;
                        }
                    }
                }
            }

            return hasChanged;
        }
    }
}