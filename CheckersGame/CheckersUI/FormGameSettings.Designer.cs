namespace CheckersUI
{
    partial class FormGameSettings
    {
        private System.ComponentModel.IContainer m_Components = null;
        private Label m_LblBoardSize;
        private RadioButton m_Rb6x6;
        private RadioButton m_Rb8x8;
        private RadioButton m_Rb10x10;
        private Label m_LblPlayers;
        private Label m_LblPlayer1;
        private TextBox m_TbPlayer1;
        private CheckBox m_CbPlayer2;
        private TextBox m_TbPlayer2;
        private Button m_BtnDone;

        protected override void Dispose(bool i_Disposing)
        {
            if (i_Disposing && (m_Components != null))
            {
                m_Components.Dispose();
            }
            base.Dispose(i_Disposing);
        }

        private void initializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGameSettings));
            m_LblBoardSize = new Label();
            m_Rb6x6 = new RadioButton();
            m_Rb8x8 = new RadioButton();
            m_Rb10x10 = new RadioButton();
            m_LblPlayers = new Label();
            m_LblPlayer1 = new Label();
            m_TbPlayer1 = new TextBox();
            m_CbPlayer2 = new CheckBox();
            m_TbPlayer2 = new TextBox();
            m_BtnDone = new Button();
            SuspendLayout();
            // 
            // lblBoardSize
            // 
            m_LblBoardSize.AutoSize = true;
            m_LblBoardSize.BackColor = Color.MistyRose;
            m_LblBoardSize.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            m_LblBoardSize.Location = new Point(92, 119);
            m_LblBoardSize.Name = "lblBoardSize";
            m_LblBoardSize.Size = new Size(140, 29);
            m_LblBoardSize.TabIndex = 0;
            m_LblBoardSize.Text = "Board Size:";
            // 
            // rb6x6
            // 
            m_Rb6x6.AutoSize = true;
            m_Rb6x6.BackColor = Color.MistyRose;
            m_Rb6x6.Checked = true;
            m_Rb6x6.Font = new Font("Trebuchet MS", 10F, FontStyle.Bold);
            m_Rb6x6.Location = new Point(251, 117);
            m_Rb6x6.Name = "rb6x6";
            m_Rb6x6.Size = new Size(84, 30);
            m_Rb6x6.TabIndex = 1;
            m_Rb6x6.TabStop = true;
            m_Rb6x6.Text = "6 x 6";
            m_Rb6x6.UseVisualStyleBackColor = false;
            // 
            // rb8x8
            // 
            m_Rb8x8.AutoSize = true;
            m_Rb8x8.BackColor = Color.MistyRose;
            m_Rb8x8.Font = new Font("Trebuchet MS", 10F, FontStyle.Bold);
            m_Rb8x8.Location = new Point(353, 117);
            m_Rb8x8.Name = "rb8x8";
            m_Rb8x8.Size = new Size(84, 30);
            m_Rb8x8.TabIndex = 2;
            m_Rb8x8.Text = "8 x 8";
            m_Rb8x8.UseVisualStyleBackColor = false;
            // 
            // rb10x10
            // 
            m_Rb10x10.AutoSize = true;
            m_Rb10x10.BackColor = Color.MistyRose;
            m_Rb10x10.Font = new Font("Trebuchet MS", 10F, FontStyle.Bold);
            m_Rb10x10.Location = new Point(459, 117);
            m_Rb10x10.Name = "rb10x10";
            m_Rb10x10.Size = new Size(108, 30);
            m_Rb10x10.TabIndex = 3;
            m_Rb10x10.Text = "10 x 10";
            m_Rb10x10.UseVisualStyleBackColor = false;
            // 
            // lblPlayers
            // 
            m_LblPlayers.AutoSize = true;
            m_LblPlayers.BackColor = Color.MistyRose;
            m_LblPlayers.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
            m_LblPlayers.Location = new Point(92, 174);
            m_LblPlayers.Name = "lblPlayers";
            m_LblPlayers.Size = new Size(103, 29);
            m_LblPlayers.TabIndex = 4;
            m_LblPlayers.Text = "Players:";
            // 
            // lblPlayer1
            // 
            m_LblPlayer1.AutoSize = true;
            m_LblPlayer1.BackColor = Color.MistyRose;
            m_LblPlayer1.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
            m_LblPlayer1.Location = new Point(92, 229);
            m_LblPlayer1.Name = "lblPlayer1";
            m_LblPlayer1.Size = new Size(114, 29);
            m_LblPlayer1.TabIndex = 5;
            m_LblPlayer1.Text = "Player 1:";
            // 
            // tbPlayer1
            // 
            m_TbPlayer1.Location = new Point(251, 227);
            m_TbPlayer1.Name = "tbPlayer1";
            m_TbPlayer1.Size = new Size(150, 31);
            m_TbPlayer1.TabIndex = 6;
            // 
            // cbPlayer2
            // 
            m_CbPlayer2.AutoSize = true;
            m_CbPlayer2.BackColor = Color.MistyRose;
            m_CbPlayer2.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
            m_CbPlayer2.Location = new Point(92, 301);
            m_CbPlayer2.Name = "cbPlayer2";
            m_CbPlayer2.Size = new Size(140, 33);
            m_CbPlayer2.TabIndex = 7;
            m_CbPlayer2.Text = "Player 2:";
            m_CbPlayer2.UseVisualStyleBackColor = false;
            m_CbPlayer2.CheckedChanged += cbPlayer2_CheckedChanged;
            // 
            // tbPlayer2
            // 
            m_TbPlayer2.Enabled = false;
            m_TbPlayer2.Location = new Point(251, 302);
            m_TbPlayer2.Name = "tbPlayer2";
            m_TbPlayer2.Size = new Size(162, 31);
            m_TbPlayer2.TabIndex = 8;
            m_TbPlayer2.Text = "[Computer]";
            // 
            // btnDone
            // 
            m_BtnDone.BackColor = Color.MistyRose;
            m_BtnDone.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
            m_BtnDone.Location = new Point(433, 382);
            m_BtnDone.Name = "btnDone";
            m_BtnDone.Size = new Size(115, 41);
            m_BtnDone.TabIndex = 9;
            m_BtnDone.Text = "Done";
            m_BtnDone.UseVisualStyleBackColor = false;
            m_BtnDone.Click += buttonDone_Click;
            // 
            // FormGameSettings
            // 
            BackColor = Color.MistyRose;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(592, 516);
            Controls.Add(m_LblBoardSize);
            Controls.Add(m_Rb6x6);
            Controls.Add(m_Rb8x8);
            Controls.Add(m_Rb10x10);
            Controls.Add(m_LblPlayers);
            Controls.Add(m_LblPlayer1);
            Controls.Add(m_TbPlayer1);
            Controls.Add(m_CbPlayer2);
            Controls.Add(m_TbPlayer2);
            Controls.Add(m_BtnDone);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormGameSettings";
            Text = "Game Settings";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
