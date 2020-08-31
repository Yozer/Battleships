using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.GameLogic;
using FluentAssertions;
using NUnit.Framework;

namespace Battleships.Tests
{
    public class BattleshipGridTests : TestBase
    {
        private readonly IEnumerable<int> _defaultShipSizes = new[] {5, 4, 4};
        private readonly int _gridSize = 10;

        private BattleshipGrid _battleship;

        [SetUp]
        public void Setup()
        {
            _battleship = new BattleshipGrid(new Random(1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrow_WhenSizeIsZeroOrNegative(int size)
        {
            Action act = () => _battleship.Initialize(size, new List<int>());
            act.Should().ThrowExactly<ArgumentException>().WithMessage(nameof(size));
        }

        [Test]
        public void ShouldThrow_WhenShipSizesAreNullOrEmpty()
        {
            Action actNull = () => _battleship.Initialize(1, null);
            Action actEmpty = () => _battleship.Initialize(1, new List<int>());
            // assert
            actNull.Should().ThrowExactly<ArgumentException>();
            actEmpty.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void ShouldThrow_WhenShipSizesAreNegativeOrZero()
        {
            Action actZero = () => _battleship.Initialize(1, new []{0});
            Action actNegative = () => _battleship.Initialize(1, new []{-1});
            // assert
            actZero.Should().ThrowExactly<ArgumentException>();
            actNegative.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void AfterInitialization_GridShouldReturnCorrectSize()
        {
            InitBattleShip();
            // assert
            _battleship.Size.Should().Be(_gridSize);
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(10, 11)]
        [TestCase(11, 10)]
        public void Shot_ShouldThrowOnInvalidColumnOrRow(int col, int row)
        {
            // arrange
            InitBattleShip();
            // act
            Action act = () =>_battleship.Shot(col, row);
            // arrange
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void Shot_ShouldThrowIfNotInitialized()
        {
            // act
            Action act = () =>_battleship.Shot(_gridSize, _gridSize);
            // arrange
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void Shot_ShouldReturnMiss_WhenShipWasNotHit_AndHitOtherwise()
        {
            // arrange
            InitBattleShip();
            
            // act & assert
            foreach (var row in Enumerable.Range(1, _gridSize))
            foreach (var col in Enumerable.Range(1, _gridSize))
            {
                var cell = new GridCell(col, row);
                var ship = _battleship.Ships.SingleOrDefault(s => s.Cells.Contains(cell));
                if (ship != null)
                {
                    var result = _battleship.Shot(col, row);
                    result.Should().Be(ship.HasSink ? ShotResult.Sink : ShotResult.Hit);
                }
                else
                {
                    _battleship.Shot(col, row).Should().Be(ShotResult.Miss);
                }
            }
        }

        [Test]
        public void AfterInitialization_ShouldPutShipsRandomly()
        {
            InitBattleShip();

            // assert
            var ships = _battleship.Ships;
            ships.Should().HaveSameCount(_defaultShipSizes);
            ships.Select(t => t.Cells.Count).Should().BeEquivalentTo(_defaultShipSizes);

            ships.Should().ContainEquivalentOf(new Ship(new[]
            {
                new GridCell(2, 3),
                new GridCell(2, 4),
                new GridCell(2, 5),
                new GridCell(2, 6),
                new GridCell(2, 7),
            }));

            ships.Should().ContainEquivalentOf(new Ship(new[]
            {
                new GridCell(5, 5),
                new GridCell(6, 5),
                new GridCell(7, 5),
                new GridCell(8, 5)
            }));

            ships.Should().ContainEquivalentOf(new Ship(new[]
            {
                new GridCell(10, 1),
                new GridCell(10, 2),
                new GridCell(10, 3),
                new GridCell(10, 4)
            }));
        }

        private void InitBattleShip() => _battleship.Initialize(_gridSize, _defaultShipSizes);
    }
}