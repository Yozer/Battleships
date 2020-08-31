using System.Collections.Generic;

namespace Battleships.GameLogic
{
    public interface IBattleshipGrid
    {
        IReadOnlyList<Ship> Ships { get; }
        IReadOnlyList<GridCell> MissedShots { get; }
        int Size { get; }
        void Initialize(int size, IEnumerable<int> shipsSizes);
        ShotResult Shot(int col, int row);
    }

    public enum ShotResult
    {
        Miss,
        Hit,
        Sink
    }
}