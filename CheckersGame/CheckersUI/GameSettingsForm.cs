using System;
using System.Windows.Forms;

namespace CheckersGameSettings
{
    public partial class GameSettingsForm : Form
    {
        public GameSettingsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Game Settings";
            this.ClientSize = new System.Drawing.Size(300, 250);

            // Labels
            Label lblBoardSize = new Label
            {
                Text = "Board Size:",
                Location = new System.Drawing.Point(20, 20),
            };

            Label lblPlayers = new Label
            {
                Text = "Players:",
                Location = new System.Drawing.Point(20, 80),
                AutoSize = truegg
            };

            Label lblPlayer1 = new Label
            {
                Text = "Player 1:",
                Location = new System.Drawing.Point(20, 110),
                AutoSize = true
            };

            // Radio Buttons
            RadioButton rb6x6 = new RadioButton
            {
                Text = "6 x 6",
                Location = new System.Drawing.Point(20, 40),
                AutoSize = true,
                Checked = true
            };

            RadioButton rb8x8 = new RadioButton
            {
                Text = "8 x 8",
                Location = new System.Drawing.Point(120, 40),
                AutoSize = true
            };

            RadioButton rb10x10 = new RadioButton
            {
                Text = "10 x 10",
                Location = new System.Drawing.Point(220, 40),
                AutoSize = true
            };

            // TextBoxes
            TextBox tbPlayer1 = new TextBox
            {
                Location = new System.Drawing.Point(100, 105),
                Width = 150
            };

            TextBox tbPlayer2 = new TextBox
            {
                Location = new System.Drawing.Point(100, 140),
                Width = 150,
                Enabled = false,
                Text = "[Computer]"
            };

            // CheckBox
            CheckBox cbPlayer2 = new CheckBox
            {
                Text = "Player 2:",
                Location = new System.Drawing.Point(20, 140),
                AutoSize = true
            };

            cbPlayer2.CheckedChanged += (sender, args) =>
            {
                tbPlayer2.Enabled = cbPlayer2.Checked;
                tbPlayer2.Text = cbPlayer2.Checked ? "" : "[Computer]";
            };

            // Done Button
            Button btnDone = new Button
            {
                Text = "Done",
                Location = new System.Drawing.Point(200, 180),
                Width = 70
            };

            btnDone.Click += (sender, args) =>
            {
                // Logic to process settings and close the form
                string boardSize = rb6x6.Checked ? "6x6" : rb8x8.Checked ? "8x8" : "10x10";
                string player1Name = tbPlayer1.Text;
                string player2Name = cbPlayer2.Checked ? tbPlayer2.Text : "Computer";

                if (string.IsNullOrWhiteSpace(player1Name) || (cbPlayer2.Checked && string.IsNullOrWhiteSpace(player2Name)))
                {
                    MessageBox.Show("Please enter valid player names.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Board Size: {boardSize}\nPlayer 1: {player1Name}\nPlayer 2: {player2Name}", "Game Settings");
                this.Close();
            };

            // Add Controls to Form
            this.Controls.Add(rb6x6);
            this.Controls.Add(rb8x8);
            this.Controls.Add(rb10x10);
            this.Controls.Add(lblPlayers);
            this.Controls.Add(lblPlayer1);
            this.Controls.Add(tbPlayer1);
            this.Controls.Add(tbPlayer2);
            this.Controls.Add(cbPlayer2);
            this.Controls.Add(btnDone);
        }

    }
}