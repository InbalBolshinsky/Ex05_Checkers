using System;
using System.Windows.Forms;

namespace CheckersUI
{
    public partial class GameSettingsForm : Form
    {
        public GameSettingsForm()
        {
            InitializeComponent();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            // Gather user settings
            string boardSize = rb6x6.Checked ? "6x6" : rb8x8.Checked ? "8x8" : "10x10";
            string player1Name = tbPlayer1.Text;
            string player2Name = cbPlayer2.Checked ? tbPlayer2.Text : "Computer";

            // Validate input
            if (string.IsNullOrWhiteSpace(player1Name) || (cbPlayer2.Checked && string.IsNullOrWhiteSpace(player2Name)))
            {
                MessageBox.Show("Please enter valid player names.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Open the game board form
            var gameBoardForm = new GameBoardForm(boardSize, player1Name, player2Name);
            gameBoardForm.Show();
            this.Hide(); // Optionally hide this form
        }

        private void cbPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            // Enable or disable the Player 2 textbox based on the checkbox state
            tbPlayer2.Enabled = cbPlayer2.Checked;
            tbPlayer2.Text = cbPlayer2.Checked ? string.Empty : "[Computer]";
        }
    }
}
