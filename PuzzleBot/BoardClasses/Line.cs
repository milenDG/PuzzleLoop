namespace PuzzleBot.BoardClasses
{
    using System;
    using System.Text;
    using Enums;

    public class Line
    {
        public LineState LineState { get; private set; }
        public bool IsVertical { get; }
        public int Weight { get; set; }

        public Line(bool isVertical)
        {
            this.LineState = LineState.Disconnected;
            this.IsVertical = isVertical;
            this.Weight = 0;
        }
        public void Connect()
        {
            this.LineState = LineState.Connected;
        }
        public void Forbid()
        {
            this.LineState = LineState.Forbidden;
        }

        public string Print(bool forbids, bool weights)
        {
            StringBuilder builder = new StringBuilder();

            switch (this.LineState)
            {
                case LineState.Connected:
                    if (weights)
                    {
                        builder.Append(this.IsVertical ? this.Weight.ToString() : $"—{this.Weight}—");
                    }
                    else
                    {
                        builder.Append(this.IsVertical ? "|" : "———");
                    }
                    break;
                case LineState.Forbidden:
                    if (forbids)
                    {
                        builder.Append(this.IsVertical ? "x" : " x ");
                    }
                    else
                    {
                        builder.Append(this.IsVertical ? " " : "   ");
                    }
                    break;
                case LineState.Disconnected:
                    builder.Append(this.IsVertical ? " " : "   ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder.ToString();
        }
    }

}