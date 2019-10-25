namespace PuzzleBot.BoardClasses
{
    using System.Text;
    using SolvingRules;

    public class Board
    {
        private Cell[,] Cells { get; }
        private Dot[,] Dots { get; }
        private bool HasInitialChecksHappened { get; set; }

        /*
         * Initializing board.
         */
        public Board(int height, int width, int[, ] cellNumbers)
        {
            this.HasInitialChecksHappened = false;
            this.Cells = new Cell[height, width];
            this.Dots = new Dot[height + 1, width + 1];

            this.InitialiseCells(cellNumbers);
            this.InitialiseDots();
            this.InitialiseDotsWithinCells();
        }

        private void InitialiseCells(int[, ] cellNumbers)
        {
            for (int i = 0; i < this.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.Cells.GetLength(1); j++)
                {
                    Line topLine = (i == 0) ? new Line(false) : this.Cells[i - 1, j].BottomLine,
                        bottomLine = new Line(false),
                        leftLine = (j == 0) ? new Line(true) : this.Cells[i, j - 1].RightLine,
                        rightLine = new Line(true);

                    this.Cells[i, j] = new Cell(cellNumbers[i, j], topLine, bottomLine, leftLine, rightLine);
                }
            }
        }

        private void InitialiseDots()
        {
            for (int i = 0; i < this.Dots.GetLength(0); i++)
            {
                for (int j = 0; j < this.Dots.GetLength(1); j++)
                {
                    Line topLine = null, bottomLine = null, leftLine = null, rightLine = null;
                    if (i == 0)
                    {
                        topLine = new Line(true);
                        topLine.Forbid();
                        if (j == 0)
                        {
                            bottomLine = this.Cells[i, j].LeftLine;
                            rightLine = this.Cells[i, j].TopLine;
                        }
                        else
                        {
                            bottomLine = this.Cells[i, j - 1].RightLine;
                            leftLine = this.Cells[i, j - 1].TopLine;
                            if (j != this.Dots.GetLength(1) - 1)
                            {
                                rightLine = this.Cells[i, j].TopLine;
                            }
                        }
                    }
                    else if (i == this.Dots.GetLength(0) - 1)
                    {
                        bottomLine = new Line(true);
                        bottomLine.Forbid();
                        if (j != 0)
                        {
                            leftLine = this.Cells[i - 1, j - 1].BottomLine;
                            topLine = this.Cells[i - 1, j - 1].RightLine;
                        }

                        if (j != this.Dots.GetLength(1) - 1)
                        {
                            rightLine = this.Cells[i - 1, j].BottomLine;
                        }

                    }

                    if (j == 0)
                    {
                        leftLine = new Line(false);
                        leftLine.Forbid();
                        if (i != this.Dots.GetLength(0) - 1)
                        {
                            bottomLine = this.Cells[i, j].LeftLine;
                        }

                        if (i != 0)
                        {
                            topLine = this.Cells[i - 1, j].LeftLine;
                            rightLine = this.Cells[i - 1, j].BottomLine;
                        }
                    }
                    else if (j == this.Dots.GetLength(1) - 1)
                    {
                        rightLine = new Line(false);
                        rightLine.Forbid();
                        if (i != 0 && i != this.Dots.GetLength(0) - 1)
                        {
                            leftLine = this.Cells[i - 1, j - 1].BottomLine;
                            topLine = this.Cells[i - 1, j - 1].RightLine;
                            bottomLine = this.Cells[i, j - 1].RightLine;
                        }
                    }

                    if (i != 0 && i != this.Dots.GetLength(0) - 1 && j != 0 && j != this.Dots.GetLength(1) - 1)
                    {
                        topLine = this.Cells[i - 1, j - 1].RightLine;
                        bottomLine = this.Cells[i, j - 1].RightLine;
                        leftLine = this.Cells[i - 1, j - 1].BottomLine;
                        rightLine = this.Cells[i - 1, j].BottomLine;
                    }

                    this.Dots[i, j] = new Dot(topLine, bottomLine, leftLine, rightLine);
                }
            }
        }

        private void InitialiseDotsWithinCells()
        {
            for (int i = 0; i < this.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.Cells.GetLength(1); j++)
                {
                    this.Cells[i, j].InitialiseDots(this.Dots[i, j], this.Dots[i, j + 1], this.Dots[i + 1, j],
                        this.Dots[i + 1, j + 1]);
                }
            }
        }

        public StringBuilder Print(bool forbids, bool weights)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.Cells.GetLength(0); i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < this.Cells.GetLength(1); j++)
                    {
                        if (j == 0)
                        {
                            builder.Append("o");
                        }

                        builder.Append(this.Cells[i, j].TopLine.Print(forbids, weights));
                        builder.Append("o");
                    }

                    builder.AppendLine();
                }

                for (int j = 0; j < this.Cells.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        builder.Append(this.Cells[i, j].LeftLine.Print(forbids, weights));
                    }

                    builder.Append((int) this.Cells[i, j].CellState < 4 ? $" {(int)this.Cells[i, j].CellState} " : "   ");

                    builder.Append(this.Cells[i, j].RightLine.Print(forbids, weights));
                }

                builder.AppendLine();
                for (int j = 0; j < this.Cells.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        builder.Append("o");
                    }

                    builder.Append(this.Cells[i, j].BottomLine.Print(forbids, weights));
                    builder.Append("o");
                }

                builder.AppendLine();
            }

            return builder;
        }

        public void Solve()
        {
            if (!this.HasInitialChecksHappened)
            {
                this.InitialRules();
            }

            this.MultiRules();
        }

        private void InitialRules()
        {
            InitialSolvingRules.ZeroCellInitial(this.Cells);
            InitialSolvingRules.ThreeNeighbourCells(this.Cells);
            InitialSolvingRules.ThreeWithTwosBetween(this.Cells);
            this.HasInitialChecksHappened = true;
        }

        private void MultiRules()
        {
            do
            {
                bool hasChanged;
                do
                {
                    hasChanged = false;
                    if (MultiSolvingRules.ReadyCells(this.Cells)
                        | MultiSolvingRules.ReadyDots(this.Dots)
                        | MultiSolvingRules.OneCellWithConnectedNeighbourLine(this.Cells)
                        | MultiSolvingRules.ThreeCellWithConnectedNeighbourLine(this.Cells)
                        | MultiSolvingRules.ThreeCellWithForbiddenDot(this.Cells)
                        | MultiSolvingRules.OneCellWithForbiddenDot(this.Cells)
                        | MultiSolvingRules.ForbidWholeCells(this.Cells)
                        | MultiSolvingRules.TwoCellWithForbiddenSide(this.Cells)
                        | MultiSolvingRules.TwoCellWithConnectedDot(this.Cells)
                        | MultiSolvingRules.TwoCellWithForbiddenDot(this.Cells)
                        | MultiSolvingRules.TwoCellWithTwoConnectedDots(this.Cells))
                    {
                        hasChanged = true;
                    }
                } while (hasChanged);

                MultiSolvingRules.SetDotWeightAsTheLowestLine(this.Dots);
            } while (MultiSolvingRules.ForbidDotsConnectionBetweenEqualWeightDots(this.Dots));
        }
    }
}