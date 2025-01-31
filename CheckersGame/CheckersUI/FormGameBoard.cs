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
            r_Board = new Board((eBoardSize)r_BoardSize, i_Player1Name, i_Player2Name, i_IsPlayer2Computer ? eGameType.AgainstComputer : eGameType.AgainstHuman);
            this.Text = $"Checkers Game - {i_Player1Name} vs {(i_IsPlayer2Computer ? "Computer" : i_Player2Name)}";
            InitializeBoardUI();
            //SetupNearTieScenario();
        }

        private void UpdateCapturedPiecesUI()
        {
            int player1TotalCaptured = r_Board.FirstPlayer.CapturedPieces.Count;
            while (m_Player1CapturedCount < player1TotalCaptured)
            {
                Checker newlyCapturedChecker = r_Board.FirstPlayer.CapturedPieces[m_Player1CapturedCount];
                AddCapturedPieceToPanel(newlyCapturedChecker, m_PanelPlayer1Captured);
                m_Player1CapturedCount++;
            }

            int player2TotalCaptured = r_Board.SecondPlayer.CapturedPieces.Count;
            while (m_Player2CapturedCount < player2TotalCaptured)
            {
                Checker newlyCapturedChecker = r_Board.SecondPlayer.CapturedPieces[m_Player2CapturedCount];
                AddCapturedPieceToPanel(newlyCapturedChecker, m_PanelPlayer2Captured);
                m_Player2CapturedCount++;
            }
        }
        private void AddCapturedPieceToPanel(Checker i_CapturedChecker, Panel i_TargetPanel)
        {
            PictureBox picBox = new PictureBox
            {
                Size = new Size(30, 30),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            string imageFileName;
            if (i_CapturedChecker.PieceType == eCheckerType.King)
            {
                // If the piece belongs to the first player, we use player1_king.png, otherwise player2_king.png
                imageFileName = i_CapturedChecker.OwnerPlayer == r_Board.FirstPlayer
                    ? "player1_king.png"
                    : "player2_king.png";
            }
            else
            {
                // Regular piece
                imageFileName = i_CapturedChecker.OwnerPlayer == r_Board.FirstPlayer
                    ? "player1_piece.png"
                    : "player2_piece.png";
            }

            string imagePath = Path.Combine(Application.StartupPath, "Resources", imageFileName);
            if (File.Exists(imagePath))
            {
                picBox.Image = Image.FromFile(imagePath);
            }
            else
            {
                // Optional: fallback or error handling
                // For example, set a default image or show a small error icon
            }

            // Add the PictureBox to the target panel
            i_TargetPanel.Controls.Add(picBox);
        }


        //private void SetupNearTieScenario()
        //{
        //    for (int row = 0; row < boardSize; row++)
        //    {
        //        for (int col = 0; col < boardSize; col++)
        //        {
        //            board.GameBoard[row, col].Clear(); // Empty the board
        //        }
        //    }

        //    // Place pieces in a way that allows one last move before a tie
        //    board.GameBoard[5, 1].CurrentCheckerPiece = new Checker(board.FirstPlayer, eCheckerType.Regular, new Position(5, 1));
        //    board.GameBoard[5, 3].CurrentCheckerPiece = new Checker(board.FirstPlayer, eCheckerType.Regular, new Position(5, 3));
        //    board.GameBoard[5, 5].CurrentCheckerPiece = new Checker(board.FirstPlayer, eCheckerType.Regular, new Position(5, 5));

        //    board.GameBoard[4, 0].CurrentCheckerPiece = new Checker(board.SecondPlayer, eCheckerType.Regular, new Position(4, 0));
        //    board.GameBoard[3, 1].CurrentCheckerPiece = new Checker(board.SecondPlayer, eCheckerType.Regular, new Position(3, 1));
        //    board.GameBoard[4, 2].CurrentCheckerPiece = new Checker(board.SecondPlayer, eCheckerType.Regular, new Position(4, 2));
        //    board.GameBoard[3, 3].CurrentCheckerPiece = new Checker(board.SecondPlayer, eCheckerType.Regular, new Position(3, 3));
        //    board.GameBoard[4, 5].CurrentCheckerPiece = new Checker(board.SecondPlayer, eCheckerType.Regular, new Position(4, 5));
        //    board.GameBoard[3, 5].CurrentCheckerPiece = new Checker(board.SecondPlayer, eCheckerType.Regular, new Position(3, 5));

        //    // Update the UI
        //    UpdateBoardUI();
        //}

        private void InitializeBoardUI()
        {
            int buttonSize = 50;
            int boardPadding = 50;
            int labelHeight = 50;
            int formWidth = (r_BoardSize * buttonSize) + (2 * boardPadding);
            int formHeight = (r_BoardSize * buttonSize) + labelHeight + (2 * boardPadding);
            this.ClientSize = new Size(formWidth, formHeight);
            m_CellButtons = new Button[r_BoardSize, r_BoardSize];

            // Initialize Panels for captured soldiers
            m_PanelPlayer1Captured = new Panel
            {
                Location = new Point(m_LblPlayer1Score.Right + 10, m_LblPlayer1Score.Top),
                Size = new Size(100, 30), // Adjust size as needed
                AutoScroll = true
            };

            m_PanelPlayer2Captured = new Panel
            {
                Location = new Point(m_LblPlayer2Score.Right + 10, m_LblPlayer2Score.Top),
                Size = new Size(100, 30), // Adjust size as needed
                AutoScroll = true
            };

            // Add the panels to the form
            this.Controls.Add(m_PanelPlayer1Captured);
            this.Controls.Add(m_PanelPlayer2Captured);

            // Create board UI
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    Button cellButton = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(boardPadding + col * buttonSize, boardPadding + labelHeight + row * buttonSize),
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
                    this.Controls.Add(cellButton);
                    m_CellButtons[row, col] = cellButton;
                }
            }

            PositionScoreLabels(boardPadding);
            UpdateBoardUI();
            if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                TriggerComputerMove();
            }
        }


        private void PositionScoreLabels(int i_BoardPadding)
        {
            m_LblPlayer1Score.Location = new Point(i_BoardPadding, 10);
            m_LblPlayer2Score.Location = new Point(this.ClientSize.Width - m_LblPlayer2Score.Width - i_BoardPadding, 10);
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
                        cellButton.BackgroundImage = null; // Remove any existing image
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    }
                    else
                    {
                        Checker checker = boardPosition.CurrentCheckerPiece;
                        cellButton.Text = "";
                        string imagePath = Path.Combine(Application.StartupPath, "Resources",
                            checker.PieceType == eCheckerType.King
                            ? (checker.OwnerPlayer == r_Board.FirstPlayer ? "player1_king.png" : "player2_king.png")
                            : (checker.OwnerPlayer == r_Board.FirstPlayer ? "player1_piece.png" : "player2_piece.png"));

                        if (File.Exists(imagePath))
                        {
                            cellButton.BackgroundImage = Image.FromFile(imagePath);
                            cellButton.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                        else
                        {
                            MessageBox.Show($"Missing image file: {imagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            HighlightCurrentPlayer();
            UpdatePlayerScores();
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
                    ? Color.Gray : Color.White;
                }
                m_SelectedPosition = null;
            }
        }

        private void RestartGame()
        {
            r_Board.Restart();
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
            string message;
            string title = "Damka";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            PlayClappingSound();
            message = DisplayWinnerMessage();
            DialogResult result = MessageBox.Show(message, title, buttons, icon);
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
            System.Threading.Tasks.Task.Delay(2000).Wait();
            player.Stop();
        }

        private void DisplayFinalWinnerMessage()
        {
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
                                  $"{finalWinner}" + Environment.NewLine + $"Final Scores:" + Environment.NewLine +
                                  $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}"
                                  + Environment.NewLine +
                                  $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";
            MessageBox.Show(finalMessage, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string DisplayWinnerMessage()
        {
            string message = "";
            //if (board.FirstPlayer.IsPossibleMovesListEmpty() && board.SecondPlayer.IsPossibleMovesListEmpty())
            //{
            //    message = "Tie! Both players have no possible moves." + Environment.NewLine + "Another Round?";
            //}
            if (r_Board.SecondPlayer.IsPiecesListEmpty())
            {
                m_FirstPlayerWins++;
                message = $"{r_Board.FirstPlayer.PlayerName} Wins! {r_Board.SecondPlayer.PlayerName} has no pieces left." + Environment.NewLine + "Another Round?";
            }
            else if (r_Board.FirstPlayer.IsPiecesListEmpty())
            {
                m_SecondPlayerWins++;
                message = $"{r_Board.SecondPlayer.PlayerName} Wins! {r_Board.FirstPlayer.PlayerName} has no pieces left." + Environment.NewLine + "Another Round?";
            }
            else if (r_Board.FirstPlayer.IsPossibleMovesListEmpty())
            {
                m_SecondPlayerWins++;
                message = $"{r_Board.SecondPlayer.PlayerName} Wins! {r_Board.FirstPlayer.PlayerName} has no available moves." + Environment.NewLine + "Another Round?";
            }
            else if (r_Board.SecondPlayer.IsPossibleMovesListEmpty())
            {
                m_FirstPlayerWins++;
                message = $"{r_Board.FirstPlayer.PlayerName} Wins! {r_Board.SecondPlayer.PlayerName} has no available moves." + Environment.NewLine + "Another Round?";
            }
            PlayClappingSound();
            return message;
        }

        private void UpdatePlayerScores()
        {
            m_LblPlayer1Score.Text = $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}";
            m_LblPlayer2Score.Text = $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";
        }
    }
}