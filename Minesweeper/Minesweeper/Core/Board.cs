using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Minesweeper.Core
{
    public class Board
    {
        public Minesweeper Minesweeper { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumMines { get; set; }
        public Cell[,] Cells { get; set; }
        public int CellSize { get; set; }
        public bool IsGameOver { get; set; } = false;
        public bool IsGameWon { get; set; } = false;
        private bool IsGameStarded { get; set; } = false;

        public Board(Minesweeper minesweeper, int width, int height, int mines, int cellSize = 45)
        {
            this.Minesweeper = minesweeper;
            this.Width = width;
            this.Height = height;
            this.NumMines = mines;
            this.CellSize = cellSize;
            this.Cells = new Cell[width, height];
        }

        public Board(int width, int height, int mines)
        {
            this.Width = width;
            this.Height = height;
            this.NumMines = mines;
        }

        public void SetupBoard()
        {
            for (int h = 0; h < this.Height; h++)
            {
                for (int w = 0; w < this.Width; w++)
                {
                    Cell cell = new Cell
                    {
                        CellState = CellState.Closed,
                        CellType = CellType.Regular,
                        CellSize = this.CellSize,
                        Board = this,
                        XLoc = w, // !!! For some reason width goes to Xloc
                        YLoc = h,
                    };
                    cell.SetupDesign();
                    cell.MouseDown += Cell_MouseClick;

                    this.Cells[w, h] = cell;
                    this.Minesweeper.Controls.Add(cell);
                }
            }
            // open all cels for testing
            //foreach (var c in this.Cells)
            //{
            //    c.OnClick();
            //}
        }

        public void CreateNumThatShowsMinesAround()
        {
            for (int h = 0; h < this.Height; h++)
            {
                for (int w = 0; w < this.Width; w++)
                {
                    Cell c = this.Cells[w, h];

                    if(c.CellType != CellType.Mine)
                    {
                        int nearMinesNumber = this.GetNearMinesCount(c);
                        c.NumMines = nearMinesNumber;
                    }
                }
            }
        }

        // UnitTested inside CreateNumThatShowsMinesAround()
        private int GetNearMinesCount(Cell cell)
        {
            int minesCount = 0;

            for (int h = -1; h <= 1; h++)
            {
                for (int w = -1; w <= 1; w++)
                {
                    int newX = cell.XLoc + w;
                    int newY = cell.YLoc + h;
                    if (newX >= 0 && newX < this.Width && newY >= 0 && newY < this.Height)
                    {
                        bool isMine = this.Cells[newX, newY].CellType == CellType.Mine;
                        if (isMine)
                        {
                            minesCount++;
                        }
                    }
                }
            }

            return minesCount;
        }

        private void GenerateMinesOnBoard(Cell cell)
        {
            var arrayOfPos = GenerateRandomMinePositions(cell);
            foreach (var p in arrayOfPos)
            {
                this.Cells[p.X, p.Y].CellType = CellType.Mine;
            }

            //var r = new Random();
            //while (this.CountAllExistingMinesOnBoard() < this.NumMines)
            //{
            //    int h = r.Next(0, this.Height);
            //    int w = r.Next(0, this.Width);
            //    // Make sure first click is NEVER a mine(BOOOM! Game Over!)
            //    if (cell.XLoc != w && cell.YLoc != h)
            //    {
            //        this.Cells[w, h].CellType = CellType.Mine;
            //    }
            //}
        }
        // This new method by chatGPT allows to create field 9x9 and 80 mines
        // my method created very long loop.
        private List<Point> GenerateRandomMinePositions(Cell cell)
        {
            var allPossiblePositions = new List<Point>();

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // Skip the clicked cell
                    if (x == cell.XLoc && y == cell.YLoc)
                        continue;

                    allPossiblePositions.Add(new Point(x, y));
                }
            }

            // Shuffle the list
            var r = new Random();
            allPossiblePositions = allPossiblePositions.OrderBy(p => r.Next()).ToList();

            // Take the first N positions
            return allPossiblePositions.Take(this.NumMines).ToList();
        }
        // My methods...
        //private List<Point> GenerateRandomMinePositions(Cell cell)
        //{
        //    var r = new Random();
        //    var arrayOfPos = new List<Point>();

        //    while (arrayOfPos.Count < this.NumMines)
        //    {
        //        int h = r.Next(0, this.Height); // YLoc
        //        int w = r.Next(0, this.Width); // XLoc
        //        var p = new Point(w, h);
        //        var canNotContainThisPoint = new Point(cell.XLoc, cell.YLoc);
        //        if (!arrayOfPos.Contains(p)
        //            && !arrayOfPos.Contains(canNotContainThisPoint))
        //        {
        //            arrayOfPos.Add(p);
        //        }
        //    }

        //    return arrayOfPos;
        //}

        //private int CountAllExistingMinesOnBoard()
        //{
        //    int minesCount = 0;

        //    for (int h = 0; h < this.Height; h++)
        //    {
        //        for (int w = 0; w < this.Width; w++)
        //        {
        //            Cell c = this.Cells[w, h];
        //            if (c.CellType == CellType.Mine)
        //            {
        //                minesCount++;
        //            }
        //        }
        //    }

        //    return minesCount;
        //}

        private void Cell_MouseClick(object sender, MouseEventArgs e)
        {
            var cell = (Cell)sender;
            if (!this.IsGameStarded)
            {
                this.GenerateMinesOnBoard(cell);
                this.CreateNumThatShowsMinesAround();
                this.IsGameStarded = true;
            }

            if (cell.CellState == CellState.Opened)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    //cell.OnClick();
                    cell.OnClick(this.Cells);
                    break;

                case MouseButtons.Right:
                    cell.OnFlag();
                    break;

                default:
                    return;
            }

        }
    }
}
