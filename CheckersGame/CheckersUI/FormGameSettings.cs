namespace CheckersUI
{
    public partial class FormGameSettings : Form
    {
        public FormGameSettings()
        {
            initializeComponent();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            string boardSize = m_Rb6x6.Checked ? "6x6" : m_Rb8x8.Checked ? "8x8" : "10x10";
            string player1Name = m_TbPlayer1.Text;
            string player2Name = m_CbPlayer2.Checked ? m_TbPlayer2.Text : "Computer";
            bool isPlayer2Computer = !m_CbPlayer2.Checked;
            bool isValidPlayerName = true;

            if (string.IsNullOrWhiteSpace(player1Name))
            {
                MessageBox.Show("Player 1 name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValidPlayerName = false;
            }

            if (m_CbPlayer2.Checked && string.IsNullOrWhiteSpace(player2Name))
            {
                MessageBox.Show("Player 2 name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValidPlayerName = false;
            }

            if(player1Name.Length > 20 || player2Name.Length > 20)
            {
                MessageBox.Show("Players name cannot be longer than 20 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValidPlayerName = false;
            }

            if(!isValidPlayerName)
            {
                return;
            }

            var gameBoardForm = new FormGameBoard(boardSize, player1Name, player2Name, isPlayer2Computer);
            gameBoardForm.Show();
            this.Hide();
        }

        private void cbPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            m_TbPlayer2.Enabled = m_CbPlayer2.Checked;
            m_TbPlayer2.Text = m_CbPlayer2.Checked ? string.Empty : "[Computer]";
        }
    }
}