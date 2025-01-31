namespace CheckersUI
{
    partial class FormGameSettings
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblBoardSize;
        private RadioButton rb6x6;
        private RadioButton rb8x8;
        private RadioButton rb10x10;
        private Label lblPlayers;
        private Label lblPlayer1;
        private TextBox tbPlayer1;
        private CheckBox cbPlayer2;
        private TextBox tbPlayer2;
        private Button btnDone;

        protected override void Dispose(bool i_Disposing)
        {
            if (i_Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(i_Disposing);
        }

        private void InitializeComponent()
        {
            lblBoardSize = new Label();
            rb6x6 = new RadioButton();
            rb8x8 = new RadioButton();
            rb10x10 = new RadioButton();
            lblPlayers = new Label();
            lblPlayer1 = new Label();
            tbPlayer1 = new TextBox();
            cbPlayer2 = new CheckBox();
            tbPlayer2 = new TextBox();
            btnDone = new Button();
            SuspendLayout();
            // 
            // lblBoardSize
            // 
            lblBoardSize.AutoSize = true;
            lblBoardSize.Location = new Point(20, 20);
            lblBoardSize.Name = "lblBoardSize";
            lblBoardSize.Size = new Size(99, 25);
            lblBoardSize.TabIndex = 0;
            lblBoardSize.Text = "Board Size:";
            // 
            // rb6x6
            // 
            rb6x6.AutoSize = true;
            rb6x6.Checked = true;
            rb6x6.Location = new Point(100, 20);
            rb6x6.Name = "rb6x6";
            rb6x6.Size = new Size(75, 29);
            rb6x6.TabIndex = 1;
            rb6x6.TabStop = true;
            rb6x6.Text = "6 x 6";
            // 
            // rb8x8
            // 
            rb8x8.AutoSize = true;
            rb8x8.Location = new Point(160, 20);
            rb8x8.Name = "rb8x8";
            rb8x8.Size = new Size(75, 29);
            rb8x8.TabIndex = 2;
            rb8x8.Text = "8 x 8";
            // 
            // rb10x10
            // 
            rb10x10.AutoSize = true;
            rb10x10.Location = new Point(220, 20);
            rb10x10.Name = "rb10x10";
            rb10x10.Size = new Size(95, 29);
            rb10x10.TabIndex = 3;
            rb10x10.Text = "10 x 10";
            // 
            // lblPlayers
            // 
            lblPlayers.AutoSize = true;
            lblPlayers.Location = new Point(20, 70);
            lblPlayers.Name = "lblPlayers";
            lblPlayers.Size = new Size(71, 25);
            lblPlayers.TabIndex = 4;
            lblPlayers.Text = "Players:";
            // 
            // lblPlayer1
            // 
            lblPlayer1.AutoSize = true;
            lblPlayer1.Location = new Point(20, 100);
            lblPlayer1.Name = "lblPlayer1";
            lblPlayer1.Size = new Size(78, 25);
            lblPlayer1.TabIndex = 5;
            lblPlayer1.Text = "Player 1:";
            // 
            // tbPlayer1
            // 
            tbPlayer1.Location = new Point(100, 100);
            tbPlayer1.Name = "tbPlayer1";
            tbPlayer1.Size = new Size(150, 31);
            tbPlayer1.TabIndex = 6;
            // 
            // cbPlayer2
            // 
            cbPlayer2.AutoSize = true;
            cbPlayer2.Location = new Point(20, 140);
            cbPlayer2.Name = "cbPlayer2";
            cbPlayer2.Size = new Size(104, 29);
            cbPlayer2.TabIndex = 7;
            cbPlayer2.Text = "Player 2:";
            cbPlayer2.CheckedChanged += cbPlayer2_CheckedChanged;
            // 
            // tbPlayer2
            // 
            tbPlayer2.Enabled = false;
            tbPlayer2.Location = new Point(100, 140);
            tbPlayer2.Name = "tbPlayer2";
            tbPlayer2.Size = new Size(150, 31);
            tbPlayer2.TabIndex = 8;
            tbPlayer2.Text = "[Computer]";
            // 
            // btnDone
            // 
            btnDone.Location = new Point(200, 180);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(75, 23);
            btnDone.TabIndex = 9;
            btnDone.Text = "Done";
            btnDone.Click += btnDone_Click;
            // 
            // GameSettingsForm
            // 
            ClientSize = new Size(374, 307);
            Controls.Add(lblBoardSize);
            Controls.Add(rb6x6);
            Controls.Add(rb8x8);
            Controls.Add(rb10x10);
            Controls.Add(lblPlayers);
            Controls.Add(lblPlayer1);
            Controls.Add(tbPlayer1);
            Controls.Add(cbPlayer2);
            Controls.Add(tbPlayer2);
            Controls.Add(btnDone);
            Name = "GameSettingsForm";
            Text = "Game Settings";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
