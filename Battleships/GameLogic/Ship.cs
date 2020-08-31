using System.Collections.Generic;
using System.Linq;

namespace Battleships.GameLogic
{
    public class Ship
    {
        private readonly List<GridCell> _cells;
        public IReadOnlyList<GridCell> Cells => _cells.AsReadOnly();
        public virtual bool HasSink => Cells.All(t => t.Hit);
        public Ship(IEnumerable<GridCell> shipCells)
            => _cells = shipCells.ToList();
    }
}