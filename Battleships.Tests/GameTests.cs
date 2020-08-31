using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using Battleships.GameLogic;
using Battleships.Renderer;
using NSubstitute;
using NUnit.Framework;

namespace Battleships.Tests
{
    public class GameTests : TestBase
    {
        private IBattleshipGrid _battleship;
        private IGameUi _ui;

        [SetUp]
        public void Setup()
        {
            _battleship = Freeze<IBattleshipGrid>();
            _ui = Freeze<IGameUi>();
        }

        [Test]
        public void Start_ShouldInitializeGridWithDefaultShipSizes()
        {
            // act
            StartGame();

            // assert
            _battleship.Received(1)
                .Initialize(10, Arg.Is<IEnumerable<int>>(ships => ships.SequenceEqual(new []{5,4,4})));
        }

        [Test]
        public void Start_ShouldRenderGridThenAskForInput()
        {
            // act
            StartGame();

            // assert
            Received.InOrder(() => {
                _ui.Received(1).Render(_battleship);
                _ui.Received(1).AskForNextCell();
            });
        }

        [TestCase((string) null)]
        [TestCase("")]
        [TestCase("AA1")]
        [TestCase("A")]
        [TestCase("1")]
        [TestCase("12")]
        [TestCase("¥5")]
        [TestCase("12Z")]
        [TestCase("A+")]
        [TestCase("++A")]
        [TestCase("A11")]
        [TestCase("Z5")]
        public void InvalidInput_ShouldInformAboutInvalidSquare(string input)
        {
            // arrange
            MockInput(input);

            // act
            StartGame();

            // assert
            _ui.Received(1).Message("Invalid input");
            _battleship.DidNotReceiveWithAnyArgs().Shot(default, default);
        }

        [Test]
        public void ValidInput_ShouldPerformAShotAndRenderGrid()
        {
            // arrange
            const string input = "D3";
            MockInput(input);

            // act
            StartGame();

            // arrange
            Received.InOrder(() => {
                _ui.Received(1).Render(_battleship);
                _battleship.Received(1).Shot(4, 3);
                _ui.Received(1).Render(_battleship);
            });
        }

        [Theory, AutoData]
        public void ValidInput_ShouldInformUserAboutShotResult(ShotResult result)
        {
            // arrange
            const string input = "F10";
            MockInput(input);
            _battleship.Shot(6, 10).Returns(result);

            // act
            StartGame();

            // arrange
            _ui.Received(1).Message(result.ToString());
        }

        [Test]
        public void ShouldAskForCellsUntilAllShipsAreSunk()
        {
            // arrange
            var ship = Substitute.For<Ship>(Fixture.CreateMany<GridCell>());
            _battleship.Ships.Returns(new List<Ship>
            {
                ship
            });

            const int expectedLoopRuns = 10;
            int asksCount = 0;
            _ui.AskForNextCell()
                .Returns("J1")
                .AndDoes(call =>
                {
                    asksCount++;
                    if (asksCount == expectedLoopRuns)
                    {
                        ship.HasSink.Returns(true);
                    }
                });

            // act
            StartGame();

            // arrange
            _ui.Received(expectedLoopRuns + 1).Render(_battleship);
            _ui.ReceivedWithAnyArgs(expectedLoopRuns + 1).Message(default);
            _ui.Received(1).Message("Congratulations! You won.");
        }

        private void MockInput(params string[] inputs) => _ui.AskForNextCell().Returns(inputs[0], inputs.Skip(1).ToArray());
        private void StartGame() => Create<Game>().Start();

    }
}