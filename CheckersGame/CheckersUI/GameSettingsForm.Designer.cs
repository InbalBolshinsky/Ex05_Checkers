namespace CheckersUI
{
    partial class GameSettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBoardSize = new Label();
            this.rb6x6 = new RadioButton();
            this.rb8x8 = new RadioButton();
            this.rb10x10 = new RadioButton();
            this.lblPlayers = new Label();
            this.lblPlayer1 = new Label();
            this.tbPlayer1 = new TextBox();
            this.cbPlayer2 = new CheckBox();
            this.tbPlayer2 = new TextBox();
            this.btnDone = new Button();

            // 
            // lblBoardSize
            // 
            this.lblBoardSize.AutoSize = true;
            this.lblBoardSize.Location = new System.Drawing.Point(20, 20);
            this.lblBoardSize.Name = "lblBoardSize";
            this.lblBoardSize.Size = new System.Drawing.Size(65, 13);
            this.lblBoardSize.Text = "Board Size:";

            // 
            // rb6x6
            // 
            this.rb6x6.AutoSize = true;
            this.rb6x6.Location = new System.Drawing.Point(100, 20);
            this.rb6x6.Name = "rb6x6";
            this.rb6x6.Size = new System.Drawing.Size(47, 17);
            this.rb6x6.Text = "6 x 6";
            this.rb6x6.Checked = true;

            // 
            // rb8x8
            // 
            this.rb8x8.AutoSize = true;
            this.rb8x8.Location = new System.Drawing.Point(160, 20);
            this.rb8x8.Name = "rb8x8";
            this.rb8x8.Size = new System.Drawing.Size(47, 17);
            this.rb8x8.Text = "8 x 8";

            // 
            // rb10x10
            // 
            this.rb10x10.AutoSize = true;
            this.rb10x10.Location = new System.Drawing.Point(220, 20);
            this.rb10x10.Name = "rb10x10";
            this.rb10x10.Size = new System.Drawing.Size(59, 17);
            this.rb10x10.Text = "10 x 10";

            // 
            // lblPlayers
            // 
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.Location = new System.Drawing.Point(20, 70);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(44, 13);
            this.lblPlayers.Text = "Players:";

            // 
            // lblPlayer1
            // 
            this.lblPlayer1.AutoSize = true;
            this.lblPlayer1.Location = new System.Drawing.Point(20, 100);
            this.lblPlayer1.Name = "lblPlayer1";
            this.lblPlayer1.Size = new System.Drawing.Size(48, 13);
            this.lblPlayer1.Text = "Player 1:";

            // 
            // tbPlayer1
            // 
            this.tbPlayer1.Location = new System.Drawing.Point(100, 100);
            this.tbPlayer1.Name = "tbPlayer1";
            this.tbPlayer1.Size = new System.Drawing.Size(150, 20);

            // 
            // cbPlayer2
            // 
            this.cbPlayer2.AutoSize = true;
            this.cbPlayer2.Location = new System.Drawing.Point(20, 140);
            this.cbPlayer2.Name = "cbPlayer2";
            this.cbPlayer2.Size = new System.Drawing.Size(65, 17);
            this.cbPlayer2.Text = "Player 2:";
            this.cbPlayer2.CheckedChanged += new System.EventHandler(this.cbPlayer2_CheckedChanged);

            // 
            // tbPlayer2
            // 
            this.tbPlayer2.Location = new System.Drawing.Point(100, 140);
            this.tbPlayer2.Name = "tbPlayer2";
            this.tbPlayer2.Size = new System.Drawing.Size(150, 20);
            this.tbPlayer2.Text = "[Computer]";
            this.tbPlayer2.Enabled = false;

            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(200, 180);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.Text = "Done";
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);

            // 
            // GameSettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(300, 250);
            this.Controls.Add(this.lblBoardSize);
            this.Controls.Add(this.rb6x6);
            this.Controls.Add(this.rb8x8);
            this.Controls.Add(this.rb10x10);
            this.Controls.Add(this.lblPlayers);
            this.Controls.Add(this.lblPlayer1);
            this.Controls.Add(this.tbPlayer1);
            this.Controls.Add(this.cbPlayer2);
            this.Controls.Add(this.tbPlayer2);
            this.Controls.Add(this.btnDone);
            this.Name = "GameSettingsForm";
            this.Text = "Game Settings";
        }
    }
}
