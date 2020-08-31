using System;

namespace Battleships.GameLogic
{
    public class GridCell
    {
        public int Column { get; }
        public int Row { get; }
        public bool Hit { get; internal set; }

        public GridCell(int column, int row)
        {
            Column = column;
            Row = row;
        }

        protected bool Equals(GridCell other) => Column == other.Column && Row == other.Row;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GridCell) obj);
        }

        public override int GetHashCode() => HashCode.Combine(Column, Row);

        public override string ToString() => $"Col: {Column} Row: {Row}";
    }
}