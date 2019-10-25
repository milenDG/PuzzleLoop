using PuzzleBot.BoardClasses;
using PuzzleBot.Enums;

namespace PuzzleBot.SolvingRules
{
    public class InitialSolvingRules {

        /// <summary>
        /// Rule
        ///        x 
        /// 0 => x 0 x
        ///        x
        /// </summary>
        /// <param name="cells">Cells</param>
        public static void ZeroCellInitial(Cell[, ] cells)
        {
            foreach (var cell in cells)
            {
                if(cell.CellState == 0
                   && ((cell.TopLine.LineState == LineState.Disconnected ||
                        cell.BottomLine.LineState == LineState.Disconnected ||
                        cell.LeftLine.LineState == LineState.Disconnected ||
                        cell.RightLine.LineState == LineState.Disconnected))) {
                    cell.TopLine.Forbid();
                    cell.BottomLine.Forbid();
                    cell.LeftLine.Forbid();
                    cell.RightLine.Forbid();
                    cell.IsReady = true;
                }
            }
        }
	
        /// <summary>
        /// Rule:
        ///  3 3 3 => |3|3|3|
        /// </summary>
        /// <param name="cells">Cells</param>
        public static void ThreeNeighbourCells(Cell[, ] cells) {
            for (int i = 0; i < cells.GetLength(0); i++) {
                for (int j = 0; j < cells.GetLength(1); j++) {
                    if((int) cells[i, j].CellState == 3) {
                        if(i < cells.GetLength(0) - 1) {
                            if((int) cells[i + 1, j].CellState == 3) {
                                cells[i, j].TopLine.Connect();;
                                cells[i, j].BottomLine.Connect();
                                cells[i + 1, j].BottomLine.Connect();
                                if(j != 0) {
                                    cells[i, j - 1].BottomLine.Forbid();
                                }
                                if(j != cells.GetLength(1) - 1) {
                                    cells[i, j + 1].BottomLine.Forbid();
                                }
                            }
                        }
                        if(j < cells.GetLength(1) - 1) {
                            if((int) cells[i, j + 1].CellState == 3) {
                                cells[i, j].LeftLine.Connect();
                                cells[i, j].RightLine.Connect();
                                cells[i, j + 1].RightLine.Connect();
                                if(i != 0) {
                                    cells[i - 1, j].RightLine.Forbid();
                                }
                                if(i != cells.GetLength(0) - 1) {
                                    cells[i + 1, j].RightLine.Forbid();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Rule:      _
        /// 3         |3
        ///   2    =>    2
        ///     3          3|
        /// </summary>     ▔
        /// <param name="cells">Cells</param>
        public static void ThreeWithTwosBetween(Cell[, ] cells) {
            for (int i = 0; i < cells.GetLength(0); i++) {
                for (int j = 0; j < cells.GetLength(1); j++) {
                    if((int) cells[i, j].CellState == 3) {
                        for (int k = 1; i + k < cells.GetLength(0) && j + k < cells.GetLength(1); k++)
                        {
                            Cell current = cells[i + k, j + k];
                            if ((int) current.CellState != 3 && (int) current.CellState != 2){
                                break;
                            }
                            else if((int) current.CellState == 3) {
                                if(cells[i, j].LeftLine.LineState == LineState.Disconnected
                                   || cells[i, j].TopLine.LineState == LineState.Disconnected
                                   || current.BottomLine.LineState == LineState.Disconnected
                                   || current.RightLine.LineState == LineState.Disconnected) {
                                    cells[i, j].TopLine.Connect();
                                    cells[i, j].LeftLine.Connect();
                                    current.BottomLine.Connect();
                                    current.RightLine.Connect();
                                }
                            }
                        }

                        for (int k = 1; i + k < cells.GetLength(0) && j - k >= 0; k++)
                        {
                            Cell current = cells[i + k, j - k];
                            if ((int) current.CellState != 3 && (int) current.CellState != 2){
                                break;
                            }
                            else if((int) current.CellState == 3) {
                                if(cells[i, j].RightLine.LineState == LineState.Disconnected
                                   || cells[i, j].TopLine.LineState == LineState.Disconnected
                                   || current.BottomLine.LineState == LineState.Disconnected
                                   || current.LeftLine.LineState == LineState.Disconnected) {
                                    cells[i, j].TopLine.Connect();
                                    cells[i, j].RightLine.Connect();
                                    current.BottomLine.Connect();
                                    current.LeftLine.Connect();
                                }
                            }
                        }
                    }
                }
            }
        }
	
	
    }
}
