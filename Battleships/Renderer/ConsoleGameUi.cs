using System;
using System.Linq;
using Battleships.GameLogic;
using Konsole;

namespace Battleships.Renderer
{
    public class ConsoleGameUi : IGameUi
    {
        private readonly HighSpeedWriter _writer;
        private readonly IConsole _textWindow;
        private readonly IConsole _gridWindow;

        public ConsoleGameUi()
        {
            Window window;

            try
            {
                Console.SetWindowSize(55, 35);
                _writer = new HighSpeedWriter();
                window = new Window(_writer);
            }
            catch (Exception ex) when(ex is PlatformNotSupportedException || ex is InvalidOperationException)
            {
                window = new Window();
            }

            var split = window.SplitRows(new Split(3), new Split(0));
            _textWindow = split[0];
            _gridWindow = split[1];
        }

        public void Render(IBattleshipGrid battleshipGrid)
        {
            _textWindow.Clear();
            _gridWindow.Clear();
            DrawGrid(battleshipGrid);
            _writer?.Flush();
        }

        private void DrawGrid(IBattleshipGrid grid)
        {
            var size = grid.Size;
            var shipCells = grid.Ships.SelectMany(t => t.Cells);
            var line = new Draw(_gridWindow);
            DrawHeaders(size);

            const int cellWidth = 4, cellHeight = 2;
            const int startingX = 3, startingY = 1;

            int x = startingX, y = startingY;
            for (int row = 1; row <= size; ++row)
            {
                for (int column = 1; column <= size; ++column)
                {
                    var color = Colors.WhiteOnBlack;
                    var cell = new GridCell(column, row);
                    var shipHit = shipCells.Any(t => t.Equals(cell) && t.Hit);
                    if (shipHit || grid.MissedShots.Contains(cell))
                        color = Colors.BlackOnWhite;

                    line.Box(x, y, x + cellWidth, y + cellHeight, string.Empty, color, Colors.BlackOnWhite);

                    if (shipHit)
                    {
                        var ship = grid.Ships.Single(t => t.Cells.Contains(cell));
                        var hitColor = ship.HasSink ? ConsoleColor.Gray : ConsoleColor.Red;
                        _gridWindow.PrintAtColor(color.Foreground, x + 2, y + 1, "X", hitColor);
                    }

                    x += cellWidth + 1;
                }

                y += cellHeight + 1;
                x = startingX;
            }
        }

        private void DrawHeaders(int size)
        {
            int x = 5;
            for (char c = 'A'; c < 'A' + size; ++c)
            {
                _gridWindow.PrintAt(x, 0, c);
                x += 5;
            }

            int y = 2;
            for(int i = 1; i <= size; ++i)
            {
                _gridWindow.PrintAt(1, y, i.ToString());
                y += 3;
            }
        }

        public void Message(string message)
        {
            _textWindow.PrintAt(0, 2, message);
            _writer?.Flush();
        }

        public string AskForNextCell()
        {
            _textWindow.PrintAt(0, 0,"Enter square:");
            _writer?.Flush();
            Console.SetCursorPosition(0, 1);
            return Console.ReadLine();
        }
    }
}