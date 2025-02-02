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
            initializeComponent();
            r_BoardSize = int.Parse(i_BoardSizeText.Split('x')[0]);
            r_Board = new Board(
                (eBoardSize)r_BoardSize,
                i_Player1Name,
                i_Player2Name,
                i_IsPlayer2Computer ? eGameType.AgainstComputer : eGameType.AgainstHuman);
            this.Text = $"Checkers Game - {i_Player1Name} vs {(i_IsPlayer2Computer ? "Computer" : i_Player2Name)}";
            initializeBoardUI();
        }

        private void initializeBoardUI()
        {
            int buttonSize = 50;
            int boardPadding = 10;
            int boardWidth;
            int boardHeight;
            Panel panelPlayer1;
            Panel panelPlayer2;
            Panel boardOuterPanel;
            Panel checkerboardPanel;

            boardWidth = (buttonSize * r_BoardSize) + (2 * boardPadding);
            boardHeight = (buttonSize * r_BoardSize) + (2 * boardPadding);
            panelPlayer1 = createPlayerPanel(10, 10, m_LblPlayer1Score, out m_PanelPlayer1Captured);
            panelPlayer2 = createPlayerPanel(panelPlayer1.Right + 10, 10, m_LblPlayer2Score, out m_PanelPlayer2Captured);
            Controls.Add(panelPlayer1);
            Controls.Add(panelPlayer2);

            boardOuterPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };

            Controls.Add(boardOuterPanel);
            boardOuterPanel.SendToBack();

            checkerboardPanel = new Panel
            {
                Size = new Size(boardWidth, boardHeight),
                BackColor = Color.DarkGray,
                Anchor = AnchorStyles.None
            };

            boardOuterPanel.Controls.Add(checkerboardPanel);
            boardOuterPanel.Resize += (s, e) => centerPanel(boardOuterPanel, checkerboardPanel);
            m_CellButtons = new Button[r_BoardSize, r_BoardSize];
            initializeCheckerboard(checkerboardPanel, buttonSize, boardPadding);
            ClientSize = new Size(panelPlayer2.Right + 20, panelPlayer1.Bottom + boardHeight + 80);
            updateBoardUI();

            if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                triggerComputerMove();
            }
        }

        private void disablePlayerButtons()
        {
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    if (r_Board.GameBoard[row, col].CurrentCheckerPiece?.OwnerPlayer == r_Board.FirstPlayer)
                    {
                        m_CellButtons[row, col].Enabled = false;
                    }
                }
            }
        }

        private void enablePlayerButtons()
        {
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    if (r_Board.GameBoard[row, col].CurrentCheckerPiece?.OwnerPlayer == r_Board.FirstPlayer)
                    {
                        m_CellButtons[row, col].Enabled = true;
                    }
                }
            }
        }

        private void initializeCheckerboard(Panel i_CheckerboardPanel, int i_ButtonSize, int i_BoardPadding)
        {
            Button cellButton;
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    cellButton = new Button
                    {
                        Size = new Size(i_ButtonSize, i_ButtonSize),
                        Location = new Point(i_BoardPadding + col * i_ButtonSize, i_BoardPadding + row * i_ButtonSize),
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

                    i_CheckerboardPanel.Controls.Add(cellButton);
                    m_CellButtons[row, col] = cellButton;
                }
            }
        }

        private void centerPanel(Panel i_OuterPanel, Panel i_InnerPanel)
        {
            i_InnerPanel.Left = (i_OuterPanel.ClientSize.Width - i_InnerPanel.Width) / 2;
            i_InnerPanel.Top = (i_OuterPanel.ClientSize.Height - i_InnerPanel.Height) / 2;
        }

        private Panel createPlayerPanel(int x, int y, Label i_LabelScore, out Panel o_PanelCaptured)
        {
            Panel panelPlayer;
            panelPlayer = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(300, 80),
                BorderStyle = BorderStyle.FixedSingle
            };

            panelPlayer.Controls.Add(i_LabelScore);
            i_LabelScore.Location = new Point(10, 10);
            o_PanelCaptured = new Panel
            {
                Location = new Point(i_LabelScore.Left, i_LabelScore.Bottom + 5),
                Size = new Size(280, 30),
                AutoScroll = true
            };

            panelPlayer.Controls.Add(o_PanelCaptured);

            return panelPlayer;
        }

        private void updateCapturedPiecesUI()
        {
            int lostByPlayer1Count;
            int lostByPlayer2Count;
            Checker newlyLostChecker;

            lostByPlayer1Count = r_Board.FirstPlayer.CapturedPieces.Count;
            lostByPlayer2Count = r_Board.SecondPlayer.CapturedPieces.Count;
            while (m_Player1CapturedCount < lostByPlayer1Count)
            {
                newlyLostChecker = r_Board.FirstPlayer.CapturedPieces[m_Player1CapturedCount];
                addCapturedPieceToPanel(newlyLostChecker, m_PanelPlayer2Captured);
                m_Player1CapturedCount++;
            }

            while (m_Player2CapturedCount < lostByPlayer2Count)
            {
                newlyLostChecker = r_Board.SecondPlayer.CapturedPieces[m_Player2CapturedCount];
                addCapturedPieceToPanel(newlyLostChecker, m_PanelPlayer1Captured);
                m_Player2CapturedCount++;
            }
        }

        private void addCapturedPieceToPanel(Checker i_CapturedChecker, Panel i_TargetPanel)
        {
            int pieceSize;
            int offsetPixels;
            string imageFileName;
            string imagePath;
            PictureBox picBox;
            int index;
            int offsetX;
            int offsetY;

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

            picBox = new PictureBox
            {
                Size = new Size(pieceSize, pieceSize),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            imageFileName =
                i_CapturedChecker.PieceType == eCheckerType.King
                    ? (i_CapturedChecker.OwnerPlayer == r_Board.FirstPlayer
                        ? "player1_king.png"
                        : "player2_king.png")
                    : (i_CapturedChecker.OwnerPlayer == r_Board.FirstPlayer
                        ? "player1_piece.png"
                        : "player2_piece.png");
            imagePath = Path.Combine(Application.StartupPath, "Resources", imageFileName);
            if (File.Exists(imagePath))
            {
                picBox.Image = Image.FromFile(imagePath);
            }

            index = i_TargetPanel.Controls.Count;
            offsetX = index * offsetPixels;
            offsetY = index * offsetPixels;
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
                        deselectChecker();

                        return;
                    }

                    if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Human)
                    {
                        if (r_Board.TryMove(m_SelectedPosition, clickedPosition))
                        {
                            updateBoardUI();
                            updateCapturedPiecesUI();
                            if (!r_Board.IsGameFinished && r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
                            {
                                triggerComputerMove();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid move. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            deselectChecker();
                        }
                    }

                    if (r_Board.IsGameFinished)
                    {
                        handleGameEnd();
                    }

                    m_SelectedPosition = null;
                }
            }
        }

        private void deselectChecker()
        {
            if (m_SelectedPosition.HasValue)
            {
                Position position = m_SelectedPosition.Value;
                Button selectedButton = m_CellButtons[position.RowPositionOnBoard, position.ColumnPositionOnBoard];

                selectedButton.BackColor = (position.RowPositionOnBoard + position.ColumnPositionOnBoard) % 2 == 0
                    ? Color.White
                    : Color.Gray;
                m_SelectedPosition = null;
            }
        }

        private void updateBoardUI()
        {
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    updateCellUI(row, col);
                }
            }

            highlightCurrentPlayer();
            updatePlayerScores();
        }

        private void updateCellUI(int i_Row, int i_Col)
        {
            Button cellButton;
            BoardPosition boardPosition;

            cellButton = m_CellButtons[i_Row, i_Col];
            boardPosition = r_Board.GameBoard[i_Row, i_Col];
            if (boardPosition.IsEmpty())
            {
                resetCell(cellButton, i_Row, i_Col);
            }
            else
            {
                updateCellWithChecker(cellButton, boardPosition.CurrentCheckerPiece);
            }

            if ((i_Row + i_Col) % 2 != 0)
            {
                cellButton.Enabled = true;
            }
        }

        private void resetCell(Button i_CellButton, int i_Row, int i_Col)
        {
            i_CellButton.Text = string.Empty;
            i_CellButton.BackgroundImage = null;
            i_CellButton.BackColor = ((i_Row + i_Col) % 2 == 0) ? Color.White : Color.Gray;
        }

        private void updateCellWithChecker(Button i_CellButton, Checker i_Checker)
        {
            string imagePath;
            i_CellButton.Text = string.Empty;
            imagePath = getImagePathForChecker(i_Checker);

            if (File.Exists(imagePath))
            {
                i_CellButton.BackgroundImage = Image.FromFile(imagePath);
                i_CellButton.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                MessageBox.Show($"Missing image file: {imagePath}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string getImagePathForChecker(Checker i_Checker)
        {
            string fileName;

            if (i_Checker.PieceType == eCheckerType.King)
            {
                fileName = (i_Checker.OwnerPlayer == r_Board.FirstPlayer) ? "player1_king.png" : "player2_king.png";
            }
            else
            {
                fileName = (i_Checker.OwnerPlayer == r_Board.FirstPlayer) ? "player1_piece.png" : "player2_piece.png";
            }

            return Path.Combine(Application.StartupPath, "Resources", fileName);
        }

        private void highlightCurrentPlayer()
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

        private void updatePlayerScores()
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
            updateBoardUI();
            updateCapturedPiecesUI();
            m_SelectedPosition = null;

            if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                triggerComputerMove();
            }
        }

        private void triggerComputerMove()
        {
            disablePlayerButtons();
            System.Windows.Forms.Timer computerMoveTimer = new System.Windows.Forms.Timer();
            computerMoveTimer.Interval = 500;
            computerMoveTimer.Tick += (s, e) =>
            {
                if (!r_Board.IsGameFinished && r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
                {
                    r_Board.ExecuteComputerMove();
                    updateBoardUI();
                    updateCapturedPiecesUI();
                    if (r_Board.IsGameFinished)
                    {
                        computerMoveTimer.Stop();
                        handleGameEnd();
                        enablePlayerButtons();
                        return;
                    }

                    if (r_Board.CurrentPlayer.IsCaptureMovesListEmpty())
                    {
                        computerMoveTimer.Stop();
                        enablePlayerButtons();
                    }
                }
                else
                {
                    computerMoveTimer.Stop();
                    enablePlayerButtons();
                }
            };

            computerMoveTimer.Start();
        }


        private void handleGameEnd()
        {
            string message;
            string title;

            message = displayWinnerMessage();
            title = "Damka";
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
                displayFinalWinnerMessage();
                Application.Exit();
            }
        }

        private void playClappingSound()
        {
            string path;
            SoundPlayer player;

            path = Path.Combine(Application.StartupPath, "Resources", "applause.wav");
            player = new SoundPlayer(path);
            player.Play();
            System.Threading.Tasks.Task.Delay(1800).Wait();
            player.Stop();
        }

        private string displayWinnerMessage()
        {
            string message = string.Empty;

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

        private void displayFinalWinnerMessage()
        {
            string finalWinner;
            string finalMessage;
            string imagePath;
            Form msgForm;
            PictureBox pictureBox;
            Label messageLabel;
            Button okButton;

            playClappingSound();
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

            finalMessage =
                $"{finalWinner}" + Environment.NewLine + "Final Scores:" + Environment.NewLine +
                $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}" + Environment.NewLine +
                $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";
            imagePath = Path.Combine(Application.StartupPath, "Resources", "trophy.png");
            if (!File.Exists(imagePath))
            {
                MessageBox.Show($"Missing image file: {imagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            msgForm = new Form()
            {
                Text = "Game Over",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            pictureBox = new PictureBox();
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
            messageLabel = new Label()
            {
                Text = finalMessage,
                Location = new Point(70, 20),
                Size = new Size(300, 80),
                Font = new Font("Arial", 10)
            };

            okButton = new Button()
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