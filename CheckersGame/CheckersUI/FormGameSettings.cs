namespace CheckersUI
{
    public partial class FormGameSettings : Form
    {
        public FormGameSettings()
        {
            InitializeComponent();
        }

        private void btnDone_Click(object i_Sender, EventArgs i_Event)
        {
            string boardSize = rb6x6.Checked ? "6x6" : rb8x8.Checked ? "8x8" : "10x10";
            string player1Name = tbPlayer1.Text;
            string player2Name = cbPlayer2.Checked ? tbPlayer2.Text : "Computer";
            bool isPlayer2Computer = !cbPlayer2.Checked;
            if (string.IsNullOrWhiteSpace(player1Name))
            {
                MessageBox.Show("Player 1 name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cbPlayer2.Checked && string.IsNullOrWhiteSpace(player2Name))
            {
                MessageBox.Show("Player 2 name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var gameBoardForm = new FormGameBoard(boardSize, player1Name, player2Name, isPlayer2Computer);
            gameBoardForm.Show();
            this.Hide();
        }

        private void cbPlayer2_CheckedChanged(object i_Sender, EventArgs i_Event)
        {
            tbPlayer2.Enabled = cbPlayer2.Checked;
            tbPlayer2.Text = cbPlayer2.Checked ? string.Empty : "[Computer]";
        }
    }
}
