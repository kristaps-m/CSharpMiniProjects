using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper.Core
{
    public enum CellType
    {
        Regular, Mine, Flagged, FlaggedMine
    }

    public enum CellState
    {
        Opened, Closed
    }

    public class Cell : Button
    {
        public int XLoc { get; set; }
        public int YLoc { get; set; }
        public int CellSize { get; set; }
        public CellState CellState { get; set; }
        public CellType CellType { get; set; }
        public int NumMines { get; set; }
        public Board Board { get; set; }

        public void SetupDesign()
        {
            this.BackColor = SystemColors.ButtonFace;
            this.Location = new Point(XLoc * CellSize, YLoc * CellSize);
            this.Size = new Size(CellSize, CellSize);
            this.UseVisualStyleBackColor = false;
            this.Font = new Font("Verdana", 15.75F, FontStyle.Bold);
            //this.ForeColor = Color.Red; // sets all num colors to red :)
            //if(this.CellType == CellType.Mine)
            //{
            //    this.BackColor = Color.Red;
            //    this.Text = "X";
            //}
        }

        public void OnFlag()
        {
            if (this.CellState == CellState.Closed && this.CellType != CellType.Flagged)
            {
                this.CellType = CellType.Flagged;
                this.Text = "F";
            }
        }

        private List<Point> directions = new List<Point> {
            new Point(-1,0), new Point(1,0), new Point(0,-1), new Point(0,1),
            new Point(-1,-1), new Point(1,1), new Point(-1,1), new Point(1,-1)
        };

        public void OnClick(Cell[,] theCels, bool recursiveCall = false)
        {

            //----------------
            if (this.CellType != CellType.Flagged && this.CellType == CellType.Mine)
            {
                this.BackColor = Color.Red;
                this.Text = "X";
                // gameOver = true;
                // isPaused = true;
            }
            else
            {
                var queue = new List<Cell>();
                queue.Add(this);

                while (queue.Count > 0)
                {
                    var currentCell = queue[queue.Count - 1];
                    queue.RemoveAt(queue.Count - 1);

                    if (currentCell != null)
                    {
                        int x = currentCell.XLoc;
                        int y = currentCell.YLoc;

                        if (theCels[x,y].CellState == CellState.Closed
                            && theCels[x,y].CellType != CellType.Flagged)
                        //if (currentCell.CellState == CellState.Closed && currentCell.CellType != CellType.Flagged)
                        {
                            theCels[x,y].CellState = CellState.Opened;
                            //this.OnClick(true);
                            //if (this.CellState == CellState.Opened && this.CellType != CellType.Mine)
                            if (theCels[x, y].CellState == CellState.Opened && theCels[x, y].CellType != CellType.Mine)
                            {
                                theCels[x, y].ForeColor = this.GetCellColour(theCels[x, y].NumMines);
                                theCels[x, y].Text = $"{theCels[x, y].NumMines}";
                            }

                            if (this.NumMines == 0)
                            {
                                // Add adjacent cells to the queue
                                /*
                                for (const direction of DIRECTIONS) {
                                    const NEW_ROW = row + direction.row;
                                    const NEW_COL = col + direction.col;

                                    if (
                                      NEW_ROW >= 0 &&
                                      NEW_ROW < TEMP_MINE_FIELD.length &&
                                      NEW_COL >= 0 &&
                                      NEW_COL < TEMP_MINE_FIELD[0].length
                                    )
                                    {
                                        QUEUE.push({ row: NEW_ROW, col: NEW_COL });
                                    }
                                }
                                */
                                foreach (var item in this.directions)
                                {
                                    int newX = x + item.X;
                                    int newY = y + item.Y;
                                    if (newX >= 0 && newX < this.Board.Width
                                        && newY >= 0 && newY < this.Board.Height)
                                    {
                                        Cell c = new Cell();
                                        c.XLoc = newX;
                                        c.YLoc = newY;
                                        queue.Add(theCels[newX,newY]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //theCels[this.XLoc, this.YLoc].CellState = CellState.Opened;
            //if (theCels[this.XLoc, this.YLoc].CellState == CellState.Closed && theCels[this.XLoc, this.YLoc].CellType == CellType.Regular)
            //{
            //    theCels[this.XLoc, this.YLoc].CellState = CellState.Opened;
            //    theCels[this.XLoc, this.YLoc].ForeColor = this.GetCellColour();
            //    theCels[this.XLoc, this.YLoc].Text = $"{this.NumMines}";
            //}

            //this.CellState = CellState.Opened;
            //if (this.CellType == CellType.Mine)
            //{
            //    this.CellState = CellState.Opened;
            //    this.BackColor = Color.Red;
            //    this.Text = "X";
            //}
            //else if (this.CellState == CellState.Closed && this.CellType == CellType.Regular)
            //{
            //    this.CellState = CellState.Opened;
            //    this.ForeColor = this.GetCellColour();
            //    this.Text = $"{this.NumMines}";
            //}
            //if (this.CellType != CellType.Mine)
            //{
            //    this.ForeColor = this.GetCellColour();
            //    this.Text = $"{NumMines}";
            //}
        }

        /// <summary>
        /// Return the colour code associated with the number of surrounding mines
        /// </summary>
        /// <returns></returns>
        private Color GetCellColour(int theNum)
        {
            switch (theNum)
            {
                case 1:
                    return ColorTranslator.FromHtml("0x0000FE"); // 1
                case 2:
                    return ColorTranslator.FromHtml("0x186900"); // 2
                case 3:
                    return ColorTranslator.FromHtml("0xAE0107"); // 3
                case 4:
                    return ColorTranslator.FromHtml("0x000177"); // 4
                case 5:
                    return ColorTranslator.FromHtml("0x8D0107"); // 5
                case 6:
                    return ColorTranslator.FromHtml("0x007A7C"); // 6
                case 7:
                    return ColorTranslator.FromHtml("0x902E90"); // 7
                case 8:
                    return ColorTranslator.FromHtml("0x000000"); // 8
                default:
                    return ColorTranslator.FromHtml("0xffffff");
            }
        }
    }
}
