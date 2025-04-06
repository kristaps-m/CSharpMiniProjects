using System;
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
            this.CellType = CellType.Flagged;
            if (this.CellState == CellState.Closed && this.CellType == CellType.Flagged)
            {
                this.Text = "F";
            }
        }

        public void OnClick(bool recursiveCall = false)
        {
            this.CellState = CellState.Opened;
            //this.GetCellColour();
            if (this.CellType == CellType.Mine)
            {
                this.BackColor = Color.Red;
                this.Text = "X";
            }
            //if (this.CellState == CellState.Opened)
            //{
            //    this.ForeColor = this.GetCellColour();
            //    this.Text = $"{NumMines}";
            //}
        }

        /// <summary>
        /// Return the colour code associated with the number of surrounding mines
        /// </summary>
        /// <returns></returns>
        private Color GetCellColour()
        {
            switch (this.NumMines)
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
