using Minesweeper.Core;
using System;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Minesweeper : Form
    {
        private Board Board { get; set; }

        public Minesweeper()
        {
            InitializeComponent();
            this.Board  = new Board(this, 9, 9, 10);
            //var board = new Board(this, 9, 9, 10);
            this.Board.SetupBoard();
        }

        private void Minesweeper_Load(object sender, EventArgs e)
        {

        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Does `NewGame` button works?");
            //InitializeComponent();

            this.Board = new Board(this, 9, 9, 10);
            this.Board.SetupBoard();
        }
    }
}
