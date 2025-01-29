namespace CheckersUI
{
    partial class FormGameBoard
    {
        private System.ComponentModel.IContainer m_Components = null;
        private Label m_LblPlayer1Score;
        private Label m_LblPlayer2Score;

        protected override void Dispose(bool i_Disposing)
        {
            if (i_Disposing && (m_Components != null))
            {
                m_Components.Dispose();
            }
            base.Dispose(i_Disposing);
        }

        private void InitializeComponent()
        {
            m_LblPlayer1Score = new Label();
            m_LblPlayer2Score = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblPlayer1Score
            // 
            m_LblPlayer1Score.AutoSize = true;
            m_LblPlayer1Score.Location = new Point(83, 19);
            m_LblPlayer1Score.Margin = new Padding(5, 0, 5, 0);
            m_LblPlayer1Score.Name = "lblPlayer1Score";
            m_LblPlayer1Score.Size = new Size(93, 25);
            m_LblPlayer1Score.TabIndex = 0;
            m_LblPlayer1Score.Text = "Player 1: 0";
            // 
            // lblPlayer2Score
            // 
            m_LblPlayer2Score.AutoSize = true;
            m_LblPlayer2Score.Location = new Point(417, 19);
            m_LblPlayer2Score.Margin = new Padding(5, 0, 5, 0);
            m_LblPlayer2Score.Name = "lblPlayer2Score";
            m_LblPlayer2Score.Size = new Size(93, 25);
            m_LblPlayer2Score.TabIndex = 1;
            m_LblPlayer2Score.Text = "Player 2: 0";

            // 
            // GameBoardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 1050);
            Controls.Add(m_LblPlayer1Score);
            Controls.Add(m_LblPlayer2Score);
            Margin = new Padding(5, 6, 5, 6);
            Name = "GameBoardForm";
            Text = "Game Board";
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox textBox1;
    }
}
