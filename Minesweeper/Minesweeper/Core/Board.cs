﻿using System;
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

        public Board(Minesweeper minesweeper, int width, int height, int mines)
        {
            this.Minesweeper = minesweeper;
            this.Width = width;
            this.Height = height;
            this.NumMines = mines;
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
                        //
                        CellSize = 50,
                        Board = this,
                        //NumMines = h,
                        XLoc = w, // !!! For some reason width goes to Xloc
                        YLoc = h,
                    };
                    //if (h == 0 && w == 1)
                    //{
                    //    cell.NumMines = -9;
                    //}
                    //if (cell.XLoc == 0 && cell.YLoc == 1)
                    //{
                    //    cell.NumMines = -5;
                    //}

                    cell.SetupDesign();
                    cell.MouseDown += Cell_MouseClick;

                    this.Cells[w, h] = cell;
                    this.Minesweeper.Controls.Add(cell);
                }
            }
            //this.Cells[1, 1].CellType = CellType.Mine;
            //Console.WriteLine(this.Cells[1,1].CellType);
            this.GenerateMinesOnBoard();
            this.GenerateMinesCountOnBoard();
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

        private void GenerateMinesCountOnBoard()
        {
            for (int h = 1; h < this.Height-1; h++)
            {
                for (int w = 1; w < this.Width-1; w++)
                {
                    Cell c = this.Cells[w, h];
                    //if (c.CellType == CellType.Mine)
                    //{
                    //    minesCount++;
                    //}
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
                    bool isMine = this.Cells[cell.XLoc + w,cell.YLoc + h].CellType == CellType.Mine;
                    if (isMine)
                    {
                        minesCount++;
                    }
                }
            }

            return minesCount;
        }

        private void GenerateMinesOnBoard()
        {
            var r = new Random();

            while (this.CountExistingMinesOnBoard() < this.NumMines)
            {
                int h = r.Next(0, this.Height);
                int w = r.Next(0, this.Width);
                this.Cells[w, h].CellType = CellType.Mine;
                this.Cells[w, h].OnClick(); // OPEN WHERE ARE MINES

            }
        }

        private int CountExistingMinesOnBoard()
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

            if (cell.CellState == CellState.Opened)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    cell.OnClick();
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
