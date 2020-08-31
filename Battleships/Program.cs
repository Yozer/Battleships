using System;
using Battleships.GameLogic;
using Battleships.Renderer;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(new BattleshipGrid(), new ConsoleGameUi());
            game.Start();
        }
    }
}
