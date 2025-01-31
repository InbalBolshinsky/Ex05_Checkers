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

        // Now these are just Panels; we’ll put them inside each player’s panel container
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
            //SetupNearTieScenario();
        }

        private void InitializeBoardUI()
        {
            // ---------------------------------------------------------------------
            // 1) Create two "player info" panels at the top (one for each player).
            //    Each contains:
            //      - The player's label (m_LblPlayerXScore).
            //      - A small panel to hold captured pieces (m_PanelPlayerXCaptured).
            // ---------------------------------------------------------------------

            // Player 1 Panel
            Panel panelPlayer1 = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(300, 80),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelPlayer1);

            // Place the Player1 label inside panelPlayer1
            // (The label is created in the Designer; we just position it.)
            m_LblPlayer1Score.Location = new Point(10, 10);
            panelPlayer1.Controls.Add(m_LblPlayer1Score);

            // Create a panel for captured pieces below the label
            m_PanelPlayer1Captured = new Panel
            {
                Location = new Point(m_LblPlayer1Score.Left, m_LblPlayer1Score.Bottom + 5),
                Size = new Size(280, 30),
                AutoScroll = true,
                BorderStyle = BorderStyle.None
            };
            panelPlayer1.Controls.Add(m_PanelPlayer1Captured);

            // Player 2 Panel
            Panel panelPlayer2 = new Panel
            {
                Location = new Point(320, 10), // offset from player1
                Size = new Size(300, 80),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelPlayer2);

            // Place the Player2 label inside panelPlayer2
            m_LblPlayer2Score.Location = new Point(10, 10);
            panelPlayer2.Controls.Add(m_LblPlayer2Score);

            // Create a panel for captured pieces below the label
            m_PanelPlayer2Captured = new Panel
            {
                Location = new Point(m_LblPlayer2Score.Left, m_LblPlayer2Score.Bottom + 5),
                Size = new Size(280, 30),
                AutoScroll = true,
                BorderStyle = BorderStyle.None
            };
            panelPlayer2.Controls.Add(m_PanelPlayer2Captured);

            // ---------------------------------------------------
            // 2) Create the checkers board (buttons) below the top panels
            //    We’ll place it at (10, panelPlayer1.Bottom + 20).
            //    You can also compute a dynamic size for the form, etc.
            // ---------------------------------------------------
            int buttonSize = 50;
            int boardPadding = 10;
            // We'll put it further down from the top panels:
            int boardTop = panelPlayer1.Bottom + 20; // or panelPlayer2, same height

            // Optionally, you can keep the old logic to define form size:
            // int formWidth = ...
            // int formHeight = ...
            // this.ClientSize = new Size(formWidth, formHeight);

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
                            boardTop + row * buttonSize),
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

            // 3) Finally, update the UI to reflect board state.
            UpdateBoardUI();

            // If it's the computer's turn, trigger AI.
            if (r_Board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                TriggerComputerMove();
            }
        }

        // -------------------------------------------------------------------------
        // The rest of your existing logic remains the same, except that we removed
        // PositionScoreLabels(...) and references to it.
        // -------------------------------------------------------------------------

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
            // 1) Decide pieceSize and offsetPixels based on the board size:
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

            // 2) Create the PictureBox with the chosen size:
            PictureBox picBox = new PictureBox
            {
                Size = new Size(pieceSize, pieceSize),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // 3) Load the correct piece image (same logic you already have):
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

            // 4) Determine the offset. Each new piece is offset by offsetPixels times the number
            //    of existing child controls (so they fan out instead of fully overlapping).
            int index = i_TargetPanel.Controls.Count;
            int offsetX = index * offsetPixels;
            int offsetY = index * offsetPixels;
            picBox.Location = new Point(offsetX, 0);

            // 5) Add it to the panel
            i_TargetPanel.Controls.Add(picBox);

            // Bring the newly added piece to the top (so you see it)
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
            PlayClappingSound();

            string message = DisplayWinnerMessage();
            string title = "Damka";
            var result = MessageBox.Show(
                message,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

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

        private string DisplayWinnerMessage()
        {
            string message = "";
            if (r_Board.SecondPlayer.IsPiecesListEmpty())
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

            PlayClappingSound();
            return message;
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
                $"{finalWinner}\nFinal Scores:\n"
                + $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}\n"
                + $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";

            MessageBox.Show(finalMessage, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}