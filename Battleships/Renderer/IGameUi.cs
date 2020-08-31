using Battleships.GameLogic;

namespace Battleships.Renderer
{
    public interface IGameUi
    {
        void Render(IBattleshipGrid battleshipGrid);
        void Message(string message);
        string AskForNextCell();
    }
}