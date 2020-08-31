using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Battleships.Renderer;

namespace Battleships.GameLogic
{
    public class Game
    {
        private readonly IBattleshipGrid _battleshipGrid;
        private readonly IGameUi _ui;

        private readonly int _gridSize = 10;
        private readonly IEnumerable<int> _defaultShipSizes = new[] {5, 4, 4};

        public Game(IBattleshipGrid battleshipGrid, IGameUi ui)
        {
            _battleshipGrid = battleshipGrid;
            _ui = ui;
        }

        public void Start()
        {
            _battleshipGrid.Initialize(_gridSize, _defaultShipSizes);
            GameLoop();
        }

        private void GameLoop()
        {
            _ui.Render(_battleshipGrid);

            do
            {
                var cell = _ui.AskForNextCell();
                var parsedCell = ParseInput(cell);
                if (parsedCell == default)
                {
                    _ui.Message("Invalid input");
                    continue;
                }

                var result = _battleshipGrid.Shot(parsedCell.X, parsedCell.Y);
                _ui.Render(_battleshipGrid);
                _ui.Message(result.ToString());

            } while (!_battleshipGrid.Ships.All(ship => ship.HasSink));

            _ui.Message("Congratulations! You won.");
        }

        private Point ParseInput(string cell)
        {
            if (string.IsNullOrWhiteSpace(cell) || 
                cell.Length < 2 || 
                !int.TryParse(cell.Substring(1), out var row))
                return default;

            var column = cell[0] - 'A' + 1;
            if (row <= 0 || row > _gridSize || column <=0 || column > _gridSize)
                return default;

            return new Point(column, row);
        }
    }
}
