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
            var c = new Cell
            {
                CellState = CellState.Closed,
                CellType = CellType.Mine,
                CellSize = 50,
                Board = this,
                NumMines = 1,
                XLoc = 0,
                YLoc = 0,
            };
            c.SetupDesign();
            c.MouseDown += Cell_MouseClick;

            this.Cells[0, 0] = c;
            this.Minesweeper.Controls.Add(c);

            var c2 = new Cell
            {
                CellState = CellState.Closed,
                CellType = CellType.Regular,
                CellSize = 50,
                Board = this,
                NumMines = 0,
                XLoc = 0,
                YLoc = 1,
            };
            c2.SetupDesign();
            c2.MouseDown += Cell_MouseClick;

            this.Cells[0, 1] = c2;
            this.Minesweeper.Controls.Add(c2);

            // cell 3
            var c3 = new Cell
            {
                CellState = CellState.Closed,
                CellType = CellType.Mine,
                CellSize = 50,
                Board = this,
                NumMines = 8,
                XLoc = 8,
                YLoc = 8,
            };
            c3.SetupDesign();
            c3.MouseDown += Cell_MouseClick;

            this.Cells[8, 8] = c3;
            this.Minesweeper.Controls.Add(c3);
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
