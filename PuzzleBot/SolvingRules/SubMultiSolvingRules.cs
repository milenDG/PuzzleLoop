namespace PuzzleBot.SolvingRules
{
    using PuzzleBot.BoardClasses;
    using PuzzleBot.Enums;

    public class SubMultiSolvingRules
    {
        public static bool SubThreeCellWithConnectedNeighbourLine(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLeft.TopLine.LineState == LineState.Connected
               || cell.TopLeft.LeftLine.LineState == LineState.Connected)
            {
                if (cell.TopLeft.LeftLine.LineState == LineState.Disconnected)
                {
                    cell.TopLeft.LeftLine.Forbid();
                    hasChanged = true;
                }
                else if (cell.TopLeft.TopLine.LineState == LineState.Disconnected)
                {
                    cell.TopLeft.TopLine.Forbid();
                    hasChanged = true;
                }
                if (cell.RightLine.LineState == LineState.Disconnected
                   || cell.BottomLine.LineState == LineState.Disconnected)
                {
                    cell.RightLine.Connect();
                    cell.BottomLine.Connect();
                    hasChanged = true;
                }
            }
            return hasChanged;
        }
        public static bool SubOneCellWithConnectedNeighbourLine(Cell cell)
        {
            bool hasChanged = false;
            if (((cell.TopLeft.TopLine.LineState == LineState.Connected
                 && cell.TopLeft.LeftLine.LineState == LineState.Forbidden)
                || (cell.TopLeft.LeftLine.LineState == LineState.Connected
                    && cell.TopLeft.TopLine.LineState == LineState.Forbidden))
                   && (cell.RightLine.LineState == LineState.Disconnected
                       || cell.BottomLine.LineState == LineState.Disconnected))
            {
                cell.RightLine.Forbid();
                cell.BottomLine.Forbid();
                hasChanged = true;
            }
            return hasChanged;
        }

        public static bool SubSetForbidsOfConnectedDot(Dot dot)
        {
            bool hasChanged = false;
            if (dot.TopLine.LineState == LineState.Connected)
            {
                if (dot.LeftLine.LineState == LineState.Connected)
                {
                    if (dot.BottomLine.LineState == LineState.Disconnected ||
                       dot.RightLine.LineState == LineState.Disconnected)
                    {
                        dot.BottomLine.Forbid();
                        dot.RightLine.Forbid();
                        dot.IsReady = true;
                        hasChanged = true;
                    }
                }
                else if (dot.BottomLine.LineState == LineState.Connected)
                {
                    if (dot.LeftLine.LineState == LineState.Disconnected ||
                       dot.RightLine.LineState == LineState.Disconnected)
                    {
                        dot.LeftLine.Forbid();
                        dot.RightLine.Forbid();
                        dot.IsReady = true;
                        hasChanged = true;
                    }
                }
                else if (dot.RightLine.LineState == LineState.Connected)
                {
                    if (dot.LeftLine.LineState == LineState.Disconnected ||
                       dot.BottomLine.LineState == LineState.Disconnected)
                    {
                        dot.LeftLine.Forbid();
                        dot.BottomLine.Forbid();
                        dot.IsReady = true;
                        hasChanged = true;
                    }
                }
            }

            return hasChanged;
        }
        public static bool SubSetLineOfForbiddenDot(Dot dot)
        {
            bool hasChanged = false;
            if (dot.TopLine.LineState == LineState.Connected)
            {
                if (dot.LeftLine.LineState == LineState.Forbidden)
                {
                    if (dot.BottomLine.LineState == LineState.Forbidden)
                    {
                        dot.RightLine.Connect();
                        dot.IsReady = true;
                        hasChanged = true;
                    }
                    else if (dot.RightLine.LineState == LineState.Forbidden)
                    {
                        dot.BottomLine.Connect();
                        dot.IsReady = true;
                        hasChanged = true;
                    }
                }
                else if (dot.BottomLine.LineState == LineState.Forbidden
                        && dot.RightLine.LineState == LineState.Forbidden)
                {
                    dot.LeftLine.Connect();
                    dot.IsReady = true;
                    hasChanged = true;
                }
            }

            return hasChanged;
        }

        /// <summary>
        /// Sub Rule:
        ///   |        |
        /// x o x => x o x
        ///            |
        /// </summary>
        /// <param name="dot">Dots</param>
        /// <returns></returns>
        public static bool SubDotWithThreeForbidden(Dot dot)
        {
            bool hasChanged = false;
            if (dot.TopLine.LineState == LineState.Forbidden
               && dot.BottomLine.LineState == LineState.Forbidden
               && dot.LeftLine.LineState == LineState.Forbidden)
            {
                dot.RightLine.Forbid();
                dot.IsReady = true;
                hasChanged = true;
            }
            return hasChanged;
        }


        /// <summary>
        /// Sub Rule:
        ///           _
        /// x 3  => x 3|
        ///           ▔
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static bool SubReadyThreeCell(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLine.LineState == LineState.Forbidden
               && (cell.BottomLine.LineState == LineState.Disconnected
                   || cell.LeftLine.LineState == LineState.Disconnected
                   || cell.RightLine.LineState == LineState.Disconnected))
            {
                cell.BottomLine.Connect();
                cell.LeftLine.Connect();
                cell.RightLine.Connect();
                cell.IsReady = true;
                hasChanged = true;
            }
            else
            {
                if (cell.TopLine.LineState == LineState.Disconnected
                   && cell.BottomLine.LineState == LineState.Connected
                   && cell.LeftLine.LineState == LineState.Connected
                   && cell.RightLine.LineState == LineState.Connected)
                {
                    cell.TopLine.Forbid();
                    cell.IsReady = true;
                    hasChanged = true;
                }
            }
            return hasChanged;
        }
        public static bool SubReadyOneCell(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLine.LineState == LineState.Connected
               && (cell.BottomLine.LineState == LineState.Disconnected
                   || cell.LeftLine.LineState == LineState.Disconnected
                   || cell.RightLine.LineState == LineState.Disconnected))
            {
                cell.BottomLine.Forbid();
                cell.LeftLine.Forbid();
                cell.RightLine.Forbid();
                cell.IsReady = true;
                hasChanged = true;
            }
            else
            {
                if (cell.BottomLine.LineState == LineState.Forbidden
                   && cell.LeftLine.LineState == LineState.Forbidden
                   && cell.RightLine.LineState == LineState.Forbidden
                   && cell.TopLine.LineState == LineState.Disconnected)
                {
                    cell.TopLine.Connect();
                    cell.IsReady = true;
                    hasChanged = true;
                }
            }
            return hasChanged;
        }
        public static bool SubReadyTwoCell(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLine.LineState == LineState.Connected
               && cell.BottomLine.LineState == LineState.Connected
               && (cell.LeftLine.LineState == LineState.Disconnected
                   || cell.RightLine.LineState == LineState.Disconnected))
            {
                cell.LeftLine.Forbid();
                cell.RightLine.Forbid();
                cell.IsReady = true;
                hasChanged = true;
            }
            else if (cell.TopLine.LineState == LineState.Forbidden
                    && cell.BottomLine.LineState == LineState.Forbidden
                    && (cell.LeftLine.LineState == LineState.Disconnected
                        || cell.RightLine.LineState == LineState.Disconnected))
            {
                cell.LeftLine.Connect();
                cell.RightLine.Connect();
                cell.IsReady = true;
                hasChanged = true;
            }
            else if (cell.TopLine.LineState == LineState.Connected
                    && cell.LeftLine.LineState == LineState.Connected
                    && (cell.BottomLine.LineState == LineState.Disconnected
                        || cell.RightLine.LineState == LineState.Disconnected))
            {
                cell.BottomLine.Forbid();
                cell.RightLine.Forbid();
                cell.IsReady = true;
                hasChanged = true;
            }
            else if (cell.TopLine.LineState == LineState.Forbidden
                    && cell.LeftLine.LineState == LineState.Forbidden
                    && (cell.BottomLine.LineState == LineState.Disconnected
                        || cell.RightLine.LineState == LineState.Disconnected))
            {
                cell.BottomLine.Connect();
                cell.RightLine.Connect();
                cell.IsReady = true;
                hasChanged = true;
            }
            return hasChanged;
        }

        /*
	 * Others
	 */
        public static bool SubThreeCellWithForbiddenDot(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLeft.TopLine.LineState == LineState.Forbidden
               && cell.TopLeft.LeftLine.LineState == LineState.Forbidden)
            {
                if (cell.TopLine.LineState == LineState.Disconnected
                   || cell.LeftLine.LineState == LineState.Disconnected)
                {
                    cell.TopLine.Connect();
                    cell.LeftLine.Connect();
                    cell.TopLeft.IsReady = true;
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        public static bool SubOneCellWithForbiddenDot(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLeft.TopLine.LineState == LineState.Forbidden
               && cell.TopLeft.LeftLine.LineState == LineState.Forbidden)
            {
                if (cell.TopLine.LineState == LineState.Disconnected
                   || cell.LeftLine.LineState == LineState.Disconnected)
                {
                    cell.TopLine.Forbid();
                    cell.LeftLine.Forbid();
                    cell.TopLeft.IsReady = true;
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        public static bool SubForbidWholeCells(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLine.LineState == LineState.Connected
               && cell.BottomLine.LineState == LineState.Connected
               && cell.LeftLine.LineState == LineState.Connected
               && cell.RightLine.LineState == LineState.Disconnected)
            {
                cell.RightLine.Forbid();
                cell.IsReady = true;
                hasChanged = true;
            }

            return hasChanged;
        }

        public static bool SubTwoCellWithForbiddenSide(Cell cell)
        {
            bool hasChanged = false;
            if ((cell.TopLeft.TopLine.LineState == LineState.Connected
                && cell.BottomLine.LineState == LineState.Forbidden)
               && (cell.RightLine.LineState == LineState.Disconnected
                   || cell.TopLeft.LeftLine.LineState == LineState.Disconnected))
            {
                cell.RightLine.Connect();
                cell.TopLeft.LeftLine.Forbid();
                hasChanged = true;
            }
            else if ((cell.TopLeft.TopLine.LineState == LineState.Connected
                     && cell.RightLine.LineState == LineState.Forbidden)
                    && (cell.BottomLine.LineState == LineState.Disconnected
                        || cell.TopLeft.LeftLine.LineState == LineState.Disconnected))
            {
                cell.BottomLine.Connect();
                cell.TopLeft.LeftLine.Forbid();
                hasChanged = true;
            }
            else if ((cell.TopLeft.LeftLine.LineState == LineState.Connected
                     && cell.BottomLine.LineState == LineState.Forbidden)
                    && (cell.RightLine.LineState == LineState.Disconnected
                        || cell.TopLeft.TopLine.LineState == LineState.Disconnected))
            {
                cell.RightLine.Connect();
                cell.TopLeft.TopLine.Forbid();
                hasChanged = true;
            }
            else if ((cell.TopLeft.LeftLine.LineState == LineState.Connected
                     && cell.RightLine.LineState == LineState.Forbidden)
                    && (cell.BottomLine.LineState == LineState.Disconnected
                        || cell.TopLeft.TopLine.LineState == LineState.Disconnected))
            {
                cell.BottomLine.Connect();
                cell.TopLeft.TopLine.Forbid();
                hasChanged = true;
            }
            return hasChanged;
        }

        public static bool SubTwoCellWithConnectedDot(Cell cell)
        {
            bool hasChanged = false;
            if ((cell.TopLeft.TopLine.LineState == LineState.Connected
                && cell.TopLeft.LeftLine.LineState == LineState.Forbidden)
               || (cell.TopLeft.TopLine.LineState == LineState.Forbidden
                   && cell.TopLeft.LeftLine.LineState == LineState.Connected))
            {
                if (cell.BottomRight.BottomLine.LineState == LineState.Forbidden
                   && cell.BottomRight.RightLine.LineState == LineState.Disconnected)
                {
                    cell.BottomRight.RightLine.Connect();
                    hasChanged = true;
                }
                else if (cell.BottomRight.BottomLine.LineState == LineState.Disconnected
                        && cell.BottomRight.RightLine.LineState == LineState.Forbidden)
                {
                    cell.BottomRight.BottomLine.Connect();
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        public static bool SubTwoCellWithForbiddenDot(Cell cell)
        {
            bool hasChanged = false;
            if (cell.TopLeft.TopLine.LineState == LineState.Forbidden
               && cell.TopLeft.LeftLine.LineState == LineState.Forbidden)
            {
                if ((cell.BottomLine.LineState == LineState.Forbidden ||
                    cell.RightLine.LineState == LineState.Forbidden)
                   && (cell.TopLine.LineState == LineState.Disconnected
                       || cell.LeftLine.LineState == LineState.Disconnected))
                {
                    cell.TopLine.Connect();
                    cell.LeftLine.Connect();
                    cell.BottomLine.Forbid();
                    cell.RightLine.Forbid();
                    cell.IsReady = true;
                    hasChanged = true;
                }
                else if ((cell.BottomLine.LineState == LineState.Connected
                         && cell.RightLine.LineState == LineState.Disconnected)
                        || (cell.BottomLine.LineState == LineState.Disconnected
                            && cell.RightLine.LineState == LineState.Connected))
                {
                    cell.BottomLine.Connect();
                    cell.RightLine.Connect();
                    cell.TopLine.Forbid();
                    cell.LeftLine.Forbid();
                    cell.IsReady = true;
                    hasChanged = true;
                }

                if (cell.TopRight.TopLine.LineState == LineState.Connected
                   && cell.TopRight.RightLine.LineState == LineState.Disconnected)
                {
                    cell.TopRight.RightLine.Forbid();
                    hasChanged = true;
                }
                else if (cell.TopRight.TopLine.LineState == LineState.Disconnected
                        && cell.TopRight.RightLine.LineState == LineState.Connected)
                {
                    cell.TopRight.TopLine.Forbid();
                    hasChanged = true;
                }

                if (cell.BottomLeft.BottomLine.LineState == LineState.Connected
                   && cell.BottomLeft.LeftLine.LineState == LineState.Disconnected)
                {
                    cell.BottomLeft.LeftLine.Forbid();
                    hasChanged = true;
                }
                else if (cell.BottomLeft.BottomLine.LineState == LineState.Disconnected
                        && cell.BottomLeft.LeftLine.LineState == LineState.Connected)
                {
                    cell.BottomLeft.BottomLine.Forbid();
                    hasChanged = true;
                }

                if (cell.TopRight.TopLine.LineState == LineState.Forbidden
                   && cell.TopRight.RightLine.LineState == LineState.Disconnected)
                {
                    cell.TopRight.RightLine.Connect();
                    hasChanged = true;
                }
                else if (cell.TopRight.TopLine.LineState == LineState.Disconnected
                        && cell.TopRight.RightLine.LineState == LineState.Forbidden)
                {
                    cell.TopRight.TopLine.Connect();
                    hasChanged = true;
                }

                if (cell.BottomLeft.BottomLine.LineState == LineState.Forbidden
                   && cell.BottomLeft.LeftLine.LineState == LineState.Disconnected)
                {
                    cell.BottomLeft.LeftLine.Connect();
                    hasChanged = true;
                }
                else if (cell.BottomLeft.BottomLine.LineState == LineState.Disconnected
                        && cell.BottomLeft.LeftLine.LineState == LineState.Forbidden)
                {
                    cell.BottomLeft.BottomLine.Connect();
                    hasChanged = true;
                }

                if (cell.BottomRight.BottomLine.LineState == LineState.Connected &&
                   cell.BottomRight.RightLine.LineState == LineState.Disconnected)
                {
                    cell.BottomRight.RightLine.Connect();
                    hasChanged = true;
                }
                else if (cell.BottomRight.BottomLine.LineState == LineState.Disconnected &&
                        cell.BottomRight.RightLine.LineState == LineState.Connected)
                {
                    cell.BottomRight.BottomLine.Connect();
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        public static bool SubTwoCellWithTwoConnectedDots(Cell cell)
        {
            bool hasChanged = false;
            if ((cell.TopLeft.TopLine.LineState == LineState.Connected
                && cell.TopLeft.LeftLine.LineState == LineState.Forbidden)
               || (cell.TopLeft.TopLine.LineState == LineState.Forbidden
                   && cell.TopLeft.LeftLine.LineState == LineState.Connected))
            {
                if (cell.BottomRight.BottomLine.LineState == LineState.Forbidden &&
                   cell.BottomRight.RightLine.LineState == LineState.Disconnected)
                {
                    cell.BottomRight.RightLine.Connect();
                    hasChanged = true;
                }
                else if (cell.BottomRight.BottomLine.LineState == LineState.Disconnected &&
                        cell.BottomRight.RightLine.LineState == LineState.Forbidden)
                {
                    cell.BottomRight.BottomLine.Connect();
                    hasChanged = true;
                }
                else if (cell.BottomRight.BottomLine.LineState == LineState.Connected &&
                        cell.BottomRight.RightLine.LineState == LineState.Disconnected)
                {
                    cell.BottomRight.RightLine.Forbid();
                    hasChanged = true;
                }
                else if (cell.BottomRight.BottomLine.LineState == LineState.Disconnected &&
                        cell.BottomRight.RightLine.LineState == LineState.Connected)
                {
                    cell.BottomRight.BottomLine.Forbid();
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        public static void SetDotWeight(Dot dot, int newWeight)
        {
            dot.Weight = newWeight;
            if (dot.TopLine.LineState == LineState.Connected)
            {
                dot.TopLine.Weight = newWeight;
            }
            if (dot.BottomLine.LineState == LineState.Connected)
            {
                dot.BottomLine.Weight = newWeight;
            }
            if (dot.LeftLine.LineState == LineState.Connected)
            {
                dot.LeftLine.Weight = newWeight;
            }
            if (dot.RightLine.LineState == LineState.Connected)
            {
                dot.RightLine.Weight = newWeight;
            }
        }
    }
}