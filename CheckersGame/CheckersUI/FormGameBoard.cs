using CheckersGameLogic;
using System.Media;
namespace CheckersUI
{
    public partial class FormGameBoard : Form
    {
        private readonly Board r_Board;
        private readonly int r_BoardSize;
        private Button[,] m_CellButtons;
        private Position? m_SelectedPosition;
        private int m_FirstPlayerWins = 0;
        private int m_SecondPlayerWins = 0;
        private Panel m_PanelPlayer1Captured;
        private Panel m_PanelPlayer2Captured;
        private int m_Player1CapturedCount = 0;
        private int m_Player2CapturedCount = 0;

        public FormGameBoard(string i_BoardSizeText, string i_Player1Name, string i_Player2Name, bool i_IsPlayer2Computer)
        {
            InitializeComponent();
            r_BoardSize = int.Parse(i_BoardSizeText.Split('x')[0]);
            r_Board = new Board(
                (eBoardSize)r_BoardSize,
                i_Player1Name,
                i_Player2Name,
                i_IsPlayer2Computer ? eGameType.AgainstComputer : eGameType.AgainstHuman);
            this.Text = $"Checkers Game - {i_Player1Name} vs {(i_IsPlayer2Computer ? "Computer" : i_Player2Name)}";
            InitializeBoardUI();
        }

        private void InitializeBoardUI()
        {
            Panel panelPlayer1 = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(300, 80),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelPlayer1);
            m_LblPlayer1Score.Location = new Point(10, 10);
            panelPlayer1.Controls.Add(m_LblPlayer1Score);
            m_PanelPlayer1Captured = new Panel
            {
                Location = new Point(m_LblPlayer1Score.Left, m_LblPlayer1Score.Bottom + 5),
                Size = new Size(280, 30),
                AutoScroll = true
            };
            panelPlayer1.Controls.Add(m_PanelPlayer1Captured);
            Panel panelPlayer2 = new Panel
            {
                Location = new Point(panelPlayer1.Right + 10, 10),
                Size = new Size(300, 80),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelPlayer2);
            m_LblPlayer2Score.Location = new Point(10, 10);
            panelPlayer2.Controls.Add(m_LblPlayer2Score);
            m_PanelPlayer2Captured = new Panel
            {
                Location = new Point(m_LblPlayer2Score.Left, m_LblPlayer2Score.Bottom + 5),
                Size = new Size(280, 30),
                AutoScroll = true
            };
            panelPlayer2.Controls.Add(m_PanelPlayer2Captured);
            Panel boardOuterPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };
            boardOuterPanel.SendToBack();
            this.Controls.Add(boardOuterPanel);
            int buttonSize = 50;
            int boardPadding = 10;
            int boardWidth = (buttonSize * r_BoardSize) + (2 * boardPadding);
            int boardHeight = (buttonSize * r_BoardSize) + (2 * boardPadding);
            Panel checkerboardPanel = new Panel
            {
                Size = new Size(boardWidth, boardHeight),
                BackColor = Color.DarkGray,
                Anchor = AnchorStyles.None
            };
            boardOuterPanel.Controls.Add(checkerboardPanel);
            boardOuterPanel.Resize += (s, e) =>
            {
                checkerboardPanel.Left = (boardOuterPanel.ClientSize.Width - checkerboardPanel.Width) / 2;
                checkerboardPanel.Top = (boardOuterPanel.ClientSize.Height - checkerboardPanel.Height) / 2;
            };
            m_CellButtons = new Button[r_BoardSize, r_BoardSize];

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    Button cellButton = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(
                            boardPadding + col * buttonSize,
                            boardPadding + row * buttonSize),
                        BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray,
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Position(row, col),
                        TabStop = false
                    };

                    if ((row + col) % 2 == 0)
                    {
                        cellButton.Enabled = false;
                    }
                    else
                    {
                        cellButton.Click += cellButton_Click;
                    }

                    checkerboardPanel.Controls.Add(cellButton);
                    m_CellButtons[row, col] = cellButton;
                }
            }

            this.ClientSize = new Size(
                panelPlayer2.Right + 20,
                panelPlayer1.Bottom + boardHeight + 80);
            UpdateBoardUI();

            if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                TriggerComputerMove();
            }
        }

        private void UpdateCapturedPiecesUI()
        {
            int lostByPlayer1Count = r_Board.FirstPlayer.CapturedPieces.Count;

            while (m_Player1CapturedCount < lostByPlayer1Count)
            {
                Checker newlyLostChecker = r_Board.FirstPlayer.CapturedPieces[m_Player1CapturedCount];
                AddCapturedPieceToPanel(newlyLostChecker, m_PanelPlayer2Captured);
                m_Player1CapturedCount++;
            }

            int lostByPlayer2Count = r_Board.SecondPlayer.CapturedPieces.Count;

            while (m_Player2CapturedCount < lostByPlayer2Count)
            {
                Checker newlyLostChecker = r_Board.SecondPlayer.CapturedPieces[m_Player2CapturedCount];
                AddCapturedPieceToPanel(newlyLostChecker, m_PanelPlayer1Captured);
                m_Player2CapturedCount++;
            }
        }

        private void AddCapturedPieceToPanel(Checker i_CapturedChecker, Panel i_TargetPanel)
        {
            int pieceSize;
            int offsetPixels;

            switch (r_BoardSize)
            {
                case 6:
                    pieceSize = 30;
                    offsetPixels = 12;
                    break;
                case 8:
                    pieceSize = 30;
                    offsetPixels = 10;
                    break;
                case 10:
                    pieceSize = 25;
                    offsetPixels = 8;
                    break;
                default:
                    pieceSize = 30;
                    offsetPixels = 10;
                    break;
            }
            PictureBox picBox = new PictureBox
            {
                Size = new Size(pieceSize, pieceSize),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            string imageFileName =
                i_CapturedChecker.PieceType == eCheckerType.King
                    ? (i_CapturedChecker.OwnerPlayer == r_Board.FirstPlayer
                        ? "player1_king.png"
                        : "player2_king.png")
                    : (i_CapturedChecker.OwnerPlayer == r_Board.FirstPlayer
                        ? "player1_piece.png"
                        : "player2_piece.png");
            string imagePath = Path.Combine(Application.StartupPath, "Resources", imageFileName);

            if (File.Exists(imagePath))
            {
                picBox.Image = Image.FromFile(imagePath);
            }

            int index = i_TargetPanel.Controls.Count;
            int offsetX = index * offsetPixels;
            int offsetY = index * offsetPixels;
            picBox.Location = new Point(offsetX, 0);
            i_TargetPanel.Controls.Add(picBox);
            picBox.BringToFront();
        }

        private void cellButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is Position clickedPosition)
            {
                if (m_SelectedPosition == null)
                {
                    if (!r_Board.GameBoard[clickedPosition.RowPositionOnBoard, clickedPosition.ColumnPositionOnBoard].IsEmpty())
                    {
                        m_SelectedPosition = clickedPosition;
                        clickedButton.BackColor = Color.LightYellow;
                    }
                }
                else
                {
                    if (m_SelectedPosition.HasValue &&
                        m_SelectedPosition.Value.RowPositionOnBoard == clickedPosition.RowPositionOnBoard &&
                        m_SelectedPosition.Value.ColumnPositionOnBoard == clickedPosition.ColumnPositionOnBoard)
                    {
                        DeselectChecker();
                        return;
                    }

                    if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Human)
                    {
                        if (r_Board.TryMove(m_SelectedPosition, clickedPosition))
                        {
                            UpdateBoardUI();
                            UpdateCapturedPiecesUI();

                            if (!r_Board.IsGameFinished && r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
                            {
                                TriggerComputerMove();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid move. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            DeselectChecker();
                        }
                    }

                    if (r_Board.IsGameFinished)
                    {
                        HandleGameEnd();
                    }

                    m_SelectedPosition = null;
                }
            }
        }

        private void DeselectChecker()
        {
            if (m_SelectedPosition.HasValue)
            {
                Position position = m_SelectedPosition.Value;
                Button selectedButton = m_CellButtons[position.RowPositionOnBoard, position.ColumnPositionOnBoard];
                Checker checker = r_Board.GameBoard[position.RowPositionOnBoard, position.ColumnPositionOnBoard].CurrentCheckerPiece;
                if (checker != null)
                {
                    selectedButton.BackColor = checker.OwnerPlayer == r_Board.FirstPlayer
                        ? Color.LightBlue
                        : Color.LightCoral;
                }
                else
                {
                    selectedButton.BackColor = (position.RowPositionOnBoard + position.ColumnPositionOnBoard) % 2 == 0
                        ? Color.Gray
                        : Color.White;
                }
                m_SelectedPosition = null;
            }
        }

        private void UpdateBoardUI()
        {
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    Button cellButton = m_CellButtons[row, col];
                    BoardPosition boardPosition = r_Board.GameBoard[row, col];
                    if (boardPosition.IsEmpty())
                    {
                        cellButton.Text = string.Empty;
                        cellButton.BackgroundImage = null;
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    }
                    else
                    {
                        Checker checker = boardPosition.CurrentCheckerPiece;
                        cellButton.Text = "";
                        string imagePath = Path.Combine(
                            Application.StartupPath,
                            "Resources",
                            checker.PieceType == eCheckerType.King
                                ? (checker.OwnerPlayer == r_Board.FirstPlayer
                                    ? "player1_king.png"
                                    : "player2_king.png")
                                : (checker.OwnerPlayer == r_Board.FirstPlayer
                                    ? "player1_piece.png"
                                    : "player2_piece.png"));
                        if (File.Exists(imagePath))
                        {
                            cellButton.BackgroundImage = Image.FromFile(imagePath);
                            cellButton.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                        else
                        {
                            MessageBox.Show($"Missing image file: {imagePath}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            HighlightCurrentPlayer();
            UpdatePlayerScores();
        }

        private void HighlightCurrentPlayer()
        {
            if (r_Board.CurrentPlayer == r_Board.FirstPlayer)
            {
                m_LblPlayer1Score.BackColor = Color.LightBlue;
                m_LblPlayer2Score.BackColor = Color.White;
            }
            else
            {
                m_LblPlayer1Score.BackColor = Color.White;
                m_LblPlayer2Score.BackColor = Color.LightCoral;
            }
            m_LblPlayer1Score.ForeColor = Color.Black;
            m_LblPlayer2Score.ForeColor = Color.Black;
            m_LblPlayer1Score.BorderStyle = BorderStyle.FixedSingle;
            m_LblPlayer2Score.BorderStyle = BorderStyle.FixedSingle;
        }

        private void UpdatePlayerScores()
        {
            m_LblPlayer1Score.Text = $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}";
            m_LblPlayer2Score.Text = $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";
        }

        private void RestartGame()
        {
            r_Board.Restart();
            m_PanelPlayer1Captured.Controls.Clear();
            m_PanelPlayer2Captured.Controls.Clear();
            m_Player1CapturedCount = 0;
            m_Player2CapturedCount = 0;
            r_Board.FirstPlayer.CapturedPieces.Clear();
            r_Board.SecondPlayer.CapturedPieces.Clear();
            UpdateBoardUI();
            UpdateCapturedPiecesUI();
            m_SelectedPosition = null;

            if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                TriggerComputerMove();
            }
        }

        private void TriggerComputerMove()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                while (!r_Board.IsGameFinished && r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
                {
                    System.Threading.Tasks.Task.Delay(500).Wait();

                    this.Invoke((MethodInvoker)delegate
                    {
                        r_Board.ActivateComputerMove();
                        UpdateBoardUI();
                        UpdateCapturedPiecesUI();
                    });

                    if (r_Board.IsGameFinished)
                    {
                        this.Invoke((MethodInvoker)HandleGameEnd);
                        break;
                    }
                    System.Threading.Tasks.Task.Delay(500).Wait();

                    if (r_Board.CurrentPlayer.IsCaptureMovesListEmpty())
                    {
                        break;
                    }
                }
            });
        }

        private void HandleGameEnd()
        {
            string message = DisplayWinnerMessage();
            string title = "Damka";
            var result = MessageBox.Show(
                message,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );
            m_PanelPlayer1Captured.Controls.Clear();
            m_PanelPlayer2Captured.Controls.Clear();
            m_Player1CapturedCount = 0;
            m_Player2CapturedCount = 0;
            r_Board.FirstPlayer.CapturedPieces.Clear();
            r_Board.SecondPlayer.CapturedPieces.Clear();

            if (result == DialogResult.Yes)
            {
                RestartGame();
            }
            else
            {
                DisplayFinalWinnerMessage();
                Application.Exit();
            }
        }

        private void PlayClappingSound()
        {
            string path = Path.Combine(Application.StartupPath, "Resources", "applause.wav");
            SoundPlayer player = new SoundPlayer(path);
            player.Play();
            System.Threading.Tasks.Task.Delay(1800).Wait();
            player.Stop();
        }

        private string DisplayWinnerMessage()
        {
            string message = "";

            if (r_Board.FirstPlayer.IsPossibleMovesListEmpty() && r_Board.SecondPlayer.IsPossibleMovesListEmpty())
            {
                message = "Tie! The game is over." + Environment.NewLine + "Another Round?";
            }
            else if (r_Board.SecondPlayer.IsPiecesListEmpty())
            {
                m_FirstPlayerWins++;
                message = $"{r_Board.FirstPlayer.PlayerName} Wins! {r_Board.SecondPlayer.PlayerName} has no pieces left."
                          + Environment.NewLine
                          + "Another Round?";
            }
            else if (r_Board.FirstPlayer.IsPiecesListEmpty())
            {
                m_SecondPlayerWins++;
                message = $"{r_Board.SecondPlayer.PlayerName} Wins! {r_Board.FirstPlayer.PlayerName} has no pieces left."
                          + Environment.NewLine
                          + "Another Round?";
            }
            else if (r_Board.FirstPlayer.IsPossibleMovesListEmpty())
            {
                m_SecondPlayerWins++;
                message = $"{r_Board.SecondPlayer.PlayerName} Wins! {r_Board.FirstPlayer.PlayerName} has no available moves."
                          + Environment.NewLine
                          + "Another Round?";
            }
            else if (r_Board.SecondPlayer.IsPossibleMovesListEmpty())
            {
                m_FirstPlayerWins++;
                message = $"{r_Board.FirstPlayer.PlayerName} Wins! {r_Board.SecondPlayer.PlayerName} has no available moves."
                          + Environment.NewLine
                          + "Another Round?";
            }

            return message;
        }

        private void DisplayFinalWinnerMessage()
        {
            PlayClappingSound();
            string finalWinner;

            if (m_FirstPlayerWins > m_SecondPlayerWins)
            {
                finalWinner = $"{r_Board.FirstPlayer.PlayerName} is the overall winner!";
            }
            else if (m_SecondPlayerWins > m_FirstPlayerWins)
            {
                finalWinner = $"{r_Board.SecondPlayer.PlayerName} is the overall winner!";
            }
            else
            {
                finalWinner = "It's a tie! Both players have the same number of wins.";
            }

            string finalMessage =
                $"{finalWinner}\nFinal Scores:\n"
                + $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}\n"
                + $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";
            string imagePath = Path.Combine(
                            Application.StartupPath,
                            "Resources", "trophy.png");

            if (!File.Exists(imagePath))
            {
                MessageBox.Show($"Missing image file: {imagePath}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Form msgForm = new Form()
            {
                Text = "Game Over",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            PictureBox pictureBox = new PictureBox();

            try
            {
                pictureBox.Image = Image.FromFile(imagePath);
            }
            catch
            {
                pictureBox.Image = SystemIcons.Information.ToBitmap();
            }

            pictureBox.Size = new Size(50, 50);
            pictureBox.Location = new Point(10, 20);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Label messageLabel = new Label()
            {
                Text = finalMessage,
                Location = new Point(70, 20),
                Size = new Size(300, 80),
                Font = new Font("Arial", 10)
            };
            Button okButton = new Button()
            {
                Text = "OK",
                Location = new Point(150, 120),
                DialogResult = DialogResult.OK
            };
            msgForm.Controls.Add(pictureBox);
            msgForm.Controls.Add(messageLabel);
            msgForm.Controls.Add(okButton);
            msgForm.AcceptButton = okButton;
            msgForm.ShowDialog();
        }
    }
}