using Minesweeper.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Minesweeper : Form
    {
        private Board Board { get; set; }

        public Minesweeper()
        {
            InitializeComponent();
            this.Board  = new Board(this, 9, 9, 10, 45);
            this.Board.SetupBoard();
        }

        private void Minesweeper_Load(object sender, EventArgs e)
        {
            difficultyComboBox.SelectedIndex = 0; // Default to Beginner
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Do you want to start a new game?",
                "New Game",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            // Determine difficulty
            int width = 9, height = 9, mines = 10, cellSize = 45;
            int offsetY = topPanel.Height;

            switch (difficultyComboBox.SelectedItem?.ToString())
            {
                case "Intermediate":
                    width = 16; height = 16; mines = 40; cellSize = 30;
                    break;
                case "Expert":
                    width = 30; height = 16; mines = 99; cellSize = 25;
                    break;
            }

            // Adjust form size to fit grid (optional: fine-tune later)
            this.ClientSize = new Size(width * cellSize + 120, height * cellSize + offsetY + 10); // 50 = cell size

            //if ( result == DialogResult.Yes )
            //{
                //MessageBox.Show("Does `NewGame` button works?");
                var cellsToRemove = new List<Control>();
                foreach (Control control in this.Controls)
                {
                    if (control is Cell)
                    {
                        cellsToRemove.Add(control);
                    }
                }

                // Now remove them safely outside the loop
                foreach (var cell in cellsToRemove)
                {
                    this.Controls.Remove(cell);
                    cell.Dispose(); // Important to release resources
                }

                this.Board = new Board(this, width, height, mines, cellSize);
                this.Board.SetupBoard();
            //}
        }

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
