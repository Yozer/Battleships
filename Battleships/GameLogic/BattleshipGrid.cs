using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.GameLogic
{
    public class BattleshipGrid : IBattleshipGrid
    {
        private readonly Random _random;
        private readonly List<Ship> _ships = new List<Ship>();
        private readonly List<GridCell> _missedShots = new List<GridCell>();

        public IReadOnlyList<Ship> Ships => _ships.AsReadOnly();
        public IReadOnlyList<GridCell> MissedShots => _missedShots.AsReadOnly();
        public int Size { get; private set; }

        public BattleshipGrid() : this(null)
        {
        }

        public BattleshipGrid(Random random)
        {
            _random = random ?? new Random();
        }

        public void Initialize(int size, IEnumerable<int> shipSizes)
        {
            if(size <= 0)
                throw new ArgumentException(nameof(size));
            if(shipSizes == null || !shipSizes.Any() || shipSizes.Any(ship => ship <= 0))
                throw new ArgumentException(nameof(shipSizes));

            Size = size;
            _ships.Clear();
            _missedShots.Clear();

            foreach (var shipSize in shipSizes)
                PlaceShipRandomly(shipSize);
        }

        public ShotResult Shot(int col, int row)
        {
            if(col <= 0 || col > Size)
                throw new ArgumentException(nameof(col));
            if(row <= 0 || row > Size)
                throw new ArgumentException(nameof(row));
            if(_ships == null || _ships.Count == 0)
                throw new InvalidOperationException("0 ships found in the grid");

            var shotCell = new GridCell(col, row);
            var ship = _ships.SingleOrDefault(t => t.Cells.Contains(shotCell));
            if (ship == null)
            {
                _missedShots.Add(shotCell);
                return ShotResult.Miss;
            }

            var shipCell = ship.Cells.Single(cell => cell.Equals(shotCell));
            if (shipCell.Hit)
                return ShotResult.Miss;

            shipCell.Hit = true;
            return ship.HasSink ? ShotResult.Sink : ShotResult.Hit;

        }

        private void PlaceShipRandomly(int shipSize)
        {
            bool overlapsWithExistingShip;
            do
            {
                // random orientation
                var orientation = _random.Next(2);
                
                // calculate max possible rows and cols for chosen orientation
                var maxPossibleCol = orientation == 0 ? Size : Size - shipSize + 1;
                var maxPossibleRow = orientation == 1 ? Size : Size - shipSize + 1;
                // random starting col and row
                var col = _random.Next(maxPossibleCol) + 1;
                var row = _random.Next(maxPossibleRow) + 1;
                // generate all cells for new ship
                var shipCells = Enumerable
                    .Range(0, shipSize)
                    .Select(offset =>
                        new GridCell(
                            orientation == 0 ? col : col + offset,
                            orientation == 0 ? row + offset : row))
                    .ToList();

                overlapsWithExistingShip = _ships
                    .Any(existingShip => existingShip.Cells.Any(shipCells.Contains));

                if(!overlapsWithExistingShip)
                    _ships.Add(new Ship(shipCells));

            } while (overlapsWithExistingShip);
        }
    }
}