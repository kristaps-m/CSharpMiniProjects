using System;
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
        //private int SpaceFromTopForButtons { get; } = 70;

        public Board(Minesweeper minesweeper, int width, int height, int mines, int cellSize)
        {
            this.Minesweeper = minesweeper;
            this.Width = width;
            this.Height = height;
            this.NumMines = mines;
            this.CellSize = cellSize;
            this.Cells = new Cell[width, height];
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
                        //
                        CellSize = this.CellSize,
                        Board = this,
                        //NumMines = h,
                        XLoc = w, // !!! For some reason width goes to Xloc
                        YLoc = h,
                    };

                    //cell.OnClick();
                    cell.SetupDesign();
                    cell.MouseDown += Cell_MouseClick;

                    this.Cells[w, h] = cell;
                    this.Minesweeper.Controls.Add(cell);
                }
            }
            //this.Cells[1, 1].CellType = CellType.Mine;
            //Console.WriteLine(this.Cells[1,1].CellType);
            //this.GenerateMinesOnBoard();
            //this.CreateNumThatShowsMinesAround();

            //foreach (var c in this.Cells)
            //{
            //    c.OnClick();
            //}
            //var c = new Cell
            //{
            //    CellState = CellState.Closed,
            //    CellType = CellType.Mine,
            //    CellSize = 50,
            //    Board = this,
            //    NumMines = 1,
            //    XLoc = 0,
            //    YLoc = 0,
            //};
            //c.SetupDesign();
            //c.MouseDown += Cell_MouseClick;

            //this.Cells[0, 0] = c;
            //this.Minesweeper.Controls.Add(c);
        }

        private void CreateNumThatShowsMinesAround()
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
            var r = new Random();

            while (this.CountAllExistingMinesOnBoard() < this.NumMines)
            {
                int h = r.Next(0, this.Height);
                int w = r.Next(0, this.Width);
                // Make sure first click is NEVER a mine(BOOOM! Game Over!)
                if (cell.XLoc != w && cell.YLoc != h)
                {
                    this.Cells[w, h].CellType = CellType.Mine;
                }
            }
        }

        private int CountAllExistingMinesOnBoard()
        {
            int minesCount = 0;

            for (int h = 0; h < this.Height; h++)
            {
                for (int w = 0; w < this.Width; w++)
                {
                    Cell c = this.Cells[w, h];
                    if (c.CellType == CellType.Mine)
                    {
                        minesCount++;
                    }
                }
            }

            return minesCount;
        }

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
