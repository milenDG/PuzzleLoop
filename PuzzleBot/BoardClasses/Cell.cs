namespace PuzzleBot.BoardClasses
{
    using System;
    using Enums;

    public class Cell
    {
        public Line TopLine { get; }
        public Line BottomLine { get; }
        public Line LeftLine { get; }
        public Line RightLine { get; }
        public Dot TopLeft { get; private set; }
        public Dot TopRight { get; private set; }
        public Dot BottomLeft { get; private set; }
        public Dot BottomRight { get; private set; }
        public CellState CellState { get; }
        public bool IsReady { get; set; }

        public Cell(int cellNumber, Line topLine, Line bottomLine, Line leftLine, Line rightLine)
        {
            this.CellState = cellNumber switch
            {
                4 => CellState.Empty,
                0 => CellState.Zero,
                1 => CellState.One,
                2 => CellState.Two,
                3 => CellState.Three,
                _ => throw new Exception("Wrong cell number!!!")
            };

            this.TopLine = topLine;

            this.BottomLine = bottomLine;

            this.LeftLine = leftLine;

            this.RightLine = rightLine;

            this.IsReady = false;

        }

        public void InitialiseDots(Dot topLeft, Dot topRight, Dot bottomLeft, Dot bottomRight)
        {
            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
            this.BottomRight = bottomRight;
        }
        
        public static Cell Turn90(Cell cell)
        {
            Line top = cell.LeftLine,
                bottom = cell.RightLine, 
                left = cell.BottomLine, 
                right = cell.TopLine;

            Cell outCell = new Cell((int) cell.CellState, top, bottom, left, right)
            {
                TopLeft = Dot.Turn90(cell.BottomLeft),
                TopRight = Dot.Turn90(cell.TopLeft),
                BottomLeft = Dot.Turn90(cell.BottomRight),
                BottomRight = Dot.Turn90(cell.TopRight)
            };


            if(cell.IsReady) 
            {
                outCell.IsReady = true;
            }
		    return outCell;
	    }
    }

}