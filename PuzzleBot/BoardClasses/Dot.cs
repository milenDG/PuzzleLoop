namespace PuzzleBot.BoardClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;

    public class Dot
    {
        public Line TopLine { get; }
        public Line BottomLine { get; }
        public Line LeftLine { get; }
        public Line RightLine { get; }
        public bool IsReady { get; set; }
        public int Weight { get; set; }

        public Dot(Line topLine, Line bottomLine, Line leftLine, Line rightLine)
        {
            this.TopLine = topLine;
            this.BottomLine = bottomLine;
            this.LeftLine = leftLine;
            this.RightLine = rightLine;
            this.IsReady = false;
        }

        public static Dot Turn90(Dot inDot)
        {
            Line top = inDot.LeftLine, bottom = inDot.RightLine, left = inDot.BottomLine, right = inDot.TopLine;
            Dot dot = new Dot(top, bottom, left, right);
            if (inDot.IsReady)
            {
                dot.IsReady = true;
            }
            return dot;
        }

        public bool FixWeight()
        {
            if (!this.IsConnected())
            {
                return false;
            }

            int leastWeight = this.LeastWeightInDot();
            if (leastWeight > 0 && 
                (this.Weight != leastWeight ||
                 (this.TopLine.LineState == LineState.Connected && this.TopLine.Weight != leastWeight) ||
                 (this.LeftLine.LineState == LineState.Connected && this.LeftLine.Weight != leastWeight) ||
                 (this.BottomLine.LineState == LineState.Connected && this.BottomLine.Weight != leastWeight) ||
                 (this.RightLine.LineState == LineState.Connected && this.RightLine.Weight != leastWeight)))
            {
                this.Weight = leastWeight;
                if (this.TopLine.LineState == LineState.Connected)
                {
                    this.TopLine.Weight = leastWeight;
                }
                if (this.LeftLine.LineState == LineState.Connected)
                {
                    this.LeftLine.Weight = leastWeight;
                }
                if (this.BottomLine.LineState == LineState.Connected)
                {
                    this.BottomLine.Weight = leastWeight;
                }
                if (this.RightLine.LineState == LineState.Connected)
                {
                    this.RightLine.Weight = leastWeight;
                }

                return true;
            }

            return false;
        }

        private int LeastWeightInDot()
        {
            try
            {
                return new List<int>
                {
                    this.Weight,
                    this.TopLine.Weight,
                    this.BottomLine.Weight,
                    this.LeftLine.Weight,
                    this.RightLine.Weight
                }.Where(n => n > 0).Min();
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
            
        }

        public bool IsConnected()
        {
            return this.TopLine.LineState == LineState.Connected
                   || this.BottomLine.LineState == LineState.Connected
                   || this.LeftLine.LineState == LineState.Connected
                   || this.RightLine.LineState == LineState.Connected;
        }

        public void ResetWeight()
        {
            this.Weight = 0;
            this.TopLine.Weight = 0;
            this.BottomLine.Weight = 0;
            this.LeftLine.Weight = 0;
            this.RightLine.Weight = 0;
        }
    }
}