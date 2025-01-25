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
            this.lblPlayer1Score = new Label();
            this.lblPlayer2Score = new Label();
            // 
            // lblPlayer1Score
            // 
            this.lblPlayer1Score.AutoSize = true;
            this.lblPlayer1Score.Location = new System.Drawing.Point(50, 10); // Keep this position as it is
            this.lblPlayer1Score.Name = "lblPlayer1Score";
            this.lblPlayer1Score.Size = new System.Drawing.Size(48, 13);
            this.lblPlayer1Score.Text = "Player 1: 0";

            // 
            // lblPlayer2Score
            // 
            this.lblPlayer2Score.AutoSize = true;
            // Adjust the X-coordinate to move Player 2's label closer to Player 1's label
            this.lblPlayer2Score.Location = new System.Drawing.Point(250, 10);
            this.lblPlayer2Score.Name = "lblPlayer2Score";
            this.lblPlayer2Score.Size = new System.Drawing.Size(48, 13);
            this.lblPlayer2Score.Text = "Player 2: 0";


            // 
            // GameBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.lblPlayer1Score);
            this.Controls.Add(this.lblPlayer2Score);
            this.Name = "GameBoardForm";
            this.Text = "Game Board";
        }
    }
}
