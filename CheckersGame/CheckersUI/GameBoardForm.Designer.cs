namespace CheckersUI
{
    partial class GameBoardForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblPlayer1Score;
        private Label lblPlayer2Score;

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
            lblPlayer1Score = new Label();
            lblPlayer2Score = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblPlayer1Score
            // 
            lblPlayer1Score.AutoSize = true;
            lblPlayer1Score.Location = new Point(83, 19);
            lblPlayer1Score.Margin = new Padding(5, 0, 5, 0);
            lblPlayer1Score.Name = "lblPlayer1Score";
            lblPlayer1Score.Size = new Size(93, 25);
            lblPlayer1Score.TabIndex = 0;
            lblPlayer1Score.Text = "Player 1: 0";
            // 
            // lblPlayer2Score
            // 
            lblPlayer2Score.AutoSize = true;
            lblPlayer2Score.Location = new Point(417, 19);
            lblPlayer2Score.Margin = new Padding(5, 0, 5, 0);
            lblPlayer2Score.Name = "lblPlayer2Score";
            lblPlayer2Score.Size = new Size(93, 25);
            lblPlayer2Score.TabIndex = 1;
            lblPlayer2Score.Text = "Player 2: 0";

            // 
            // GameBoardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 1050);
            Controls.Add(lblPlayer1Score);
            Controls.Add(lblPlayer2Score);
            Margin = new Padding(5, 6, 5, 6);
            Name = "GameBoardForm";
            Text = "Game Board";
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox textBox1;
    }
}
