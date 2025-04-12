using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Core;
using System;
using System.Collections.Generic;

namespace UnitTestMinesweeper.Tests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var a = 100;
            var b = 33;
            //Act
            var result = a + b;
            // Assert
            result.Should().Be(133);
        }

        [TestMethod]
        public void ItCountsMinesAroundCorrectlySimpleCase()
        {
            // Arrange
            Board board = new Board(2, 2, 1);
            board.Cells = new Cell[,]
            {
                { new Cell { CellType = CellType.Mine }, new Cell() },
                { new Cell(), new Cell() }
            };
            // Act
            board.CreateNumThatShowsMinesAround();

            // Assert
            board.Cells[0, 1].NumMines.Should().Be(1);
            board.Cells[1, 0].NumMines.Should().Be(1);
            board.Cells[1, 1].NumMines.Should().Be(1);
        }

        [TestMethod]
        public void ItCountsMinesAroundCorrectly_2()
        {
            // Arrange
            Board board = new Board(3, 3, 2);
            board.Cells = new Cell[,]
            {
                { new Cell { CellType = CellType.Mine, XLoc=0, YLoc=0 }, new Cell{XLoc=0, YLoc=1}, new Cell{XLoc=0, YLoc=2} },
                { new Cell{ CellType = CellType.Mine,XLoc=1, YLoc=0 } , new Cell{XLoc=1, YLoc=1}, new Cell{XLoc=1, YLoc=2} },
                { new Cell{XLoc=2, YLoc=0}, new Cell{XLoc=2, YLoc=1}, new Cell{XLoc=2, YLoc=2}}
            };
            // Act
            board.CreateNumThatShowsMinesAround();
            // Assert
            /*
             [*,2,0],
             [*,2,0],
             [1,1,0]
             */
            board.Cells[0, 1].NumMines.Should().Be(2);
            board.Cells[0, 2].NumMines.Should().Be(0);

            board.Cells[1, 1].NumMines.Should().Be(2);
            board.Cells[1, 2].NumMines.Should().Be(0);

            board.Cells[2, 0].NumMines.Should().Be(1);
            board.Cells[2, 1].NumMines.Should().Be(1);
            board.Cells[2, 2].NumMines.Should().Be(0);
        }

        [TestMethod]
        public void ItCountsMinesAroundCorrectly_3()
        {
            // Arrange
            int h = 4; int w = 4;
            Board board = new Board(w, h, 5);
            board.Cells = MakeGrid(w,h, new List<int[]> {
                new int[] { 0, 1 }, new int[] { 1, 1 },
                new int[] { 1, 0 }, new int[] { 2, 3 },
                new int[] { 3, 2 }
            });
            // Act
            board.CreateNumThatShowsMinesAround();
            // Assert
            /*
             [3,*,2,0],
             [*,*,3,1],
             [2,3,3,*],
             [0,1,*,2]
             */
            board.Cells[0, 0].NumMines.Should().Be(3);
            board.Cells[0, 2].NumMines.Should().Be(2);

            board.Cells[1, 2].NumMines.Should().Be(3);
            board.Cells[1, 3].NumMines.Should().Be(1);

            board.Cells[2, 0].NumMines.Should().Be(2);
            board.Cells[2, 1].NumMines.Should().Be(3);
            board.Cells[2, 2].NumMines.Should().Be(3);

            board.Cells[3, 1].NumMines.Should().Be(1);
            board.Cells[3, 3].NumMines.Should().Be(2);
        }

        [TestMethod]
        public void ItCountsMinesAroundCorrectlyBigBoard()
        {
            // Arrange
            int h = 6; int w = 6;
            Board board = new Board(w, h, 7);
            board.Cells = MakeGrid(w, h, new List<int[]> {
                new int[] { 2, 2 }, new int[] { 2, 3 }, new int[] { 2, 4 },
                new int[] { 3, 2 }, new int[] { 3, 4 },
                new int[] { 4, 3 },new int[] { 4, 4 },
            });
            // Act
            board.CreateNumThatShowsMinesAround();
            // Assert
            /*
            [0,0,0,0,0,0],
            [0,1,2,3,2,1],
            [0,2,*,*,*,2],
            [0,2,*,7,*,3],
            [0,1,2,*,*,2],
            [0,0,1,2,2,1]
             */
            board.Cells[1, 1].NumMines.Should().Be(1);
            board.Cells[1, 3].NumMines.Should().Be(3);

            board.Cells[2, 1].NumMines.Should().Be(2);
            board.Cells[2, 5].NumMines.Should().Be(2);

            board.Cells[3, 1].NumMines.Should().Be(2);
            board.Cells[3, 5].NumMines.Should().Be(3);
            board.Cells[3, 3].NumMines.Should().Be(7);

            board.Cells[5, 5].NumMines.Should().Be(1);
            board.Cells[5, 3].NumMines.Should().Be(2);
        }

        private Cell[,] MakeGrid(int width,int height, List<int[]> mines)
        {
            Cell[,] cells = new Cell[width, height];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Cell cell = new Cell
                    {
                        CellState = CellState.Closed,
                        CellType = CellType.Regular,
                        XLoc = w,
                        YLoc = h,
                    };
                    cells[w, h] = cell;

                    for (int i = 0; i < mines.Count; i++)
                    {
                        if (cells[w,h].XLoc == mines[i][0] && cells[w, h].YLoc == mines[i][1])
                        {
                            cell.CellType = CellType.Mine;
                        }
                    }
                }
            }

            return cells;
        }

        private void PrintOutBoardCellsContentInsideTestForDebugging(Board board)
        {
            for (int h = 0; h < board.Height; h++)
            {
                for (int w = 0; board.Width < 3; w++)
                {
                    Console.Write($"h{h}:w{w}{board.Cells[w, h].CellType}-" +
                        $"N:{board.Cells[w, h].NumMines}-" +
                        $"xL:yL:{board.Cells[w, h].XLoc}:{board.Cells[w, h].YLoc}  ");
                }
                Console.WriteLine();
            }
        }
    }
}
