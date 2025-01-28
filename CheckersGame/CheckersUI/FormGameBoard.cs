using CheckersGameLogic;

namespace CheckersUI
{
    public partial class FormGameBoard : Form
    {
        private readonly Board board;
        private readonly int boardSize;
        private Button[,] cellButtons;
        private Position? selectedPosition;

        public FormGameBoard(string boardSizeText, string player1Name, string player2Name, bool isPlayer2Computer)
        {
            InitializeComponent();

            boardSize = int.Parse(boardSizeText.Split('x')[0]);
            board = new Board((eBoardSize)boardSize, player1Name, player2Name, isPlayer2Computer ? eGameType.AgainstComputer : eGameType.AgainstHuman);

            this.Text = $"Checkers Game - {player1Name} vs {(isPlayer2Computer ? "Computer" : player2Name)}";

            //lblPlayer1Score.Text = $"{player1Name}: ";
            //lblPlayer2Score.Text = isPlayer2Computer ? "Computer: 0" : $"{player2Name}: 0";

           InitializeBoardUI();
        }

        private void InitializeBoardUI()
        {
            int buttonSize = 50;
            cellButtons = new Button[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button cellButton = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(50 + col * buttonSize, 50 + row * buttonSize),
                        BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray,
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Position(row, col),
                        TabStop = false
                    };

                    cellButton.Click += OnCellButtonClick;
                    this.Controls.Add(cellButton);
                    cellButtons[row, col] = cellButton;
                }
            }

            UpdateBoardUI();

            // Trigger the computer's move automatically if it starts the game
            if (board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                TriggerComputerMove();
            }
        }

        private void UpdateBoardUI()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button cellButton = cellButtons[row, col];
                    BoardPosition boardPosition = board.GameBoard[row, col];

                    if (boardPosition.IsEmpty())
                    {
                        cellButton.Text = string.Empty;
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    }
                    else
                    {
                        Checker checker = boardPosition.CurrentCheckerPiece;
                        cellButton.Text = checker.PieceType == eCheckerType.King
                           ? (checker.OwnerPlayer == board.FirstPlayer ? "K" : "U")
                           : (checker.OwnerPlayer == board.FirstPlayer ? "X" : "O");
                        cellButton.BackColor = checker.OwnerPlayer == board.FirstPlayer
                            ? Color.LightBlue
                            : Color.LightCoral;
                    }
                }
            }
            HighlightCurrentPlayer();
            UpdatePlayerScores();
        }

        private void OnCellButtonClick(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is Position clickedPosition)
            {
                if (selectedPosition == null)
                {
                    if (!board.GameBoard[clickedPosition.RowPositionOnBoard, clickedPosition.ColumnPositionOnBoard].IsEmpty())
                    {
                        selectedPosition = clickedPosition;
                        clickedButton.BackColor = Color.LightYellow;
                    }
                }
                else
                {
                    if (selectedPosition.HasValue &&
                        selectedPosition.Value.RowPositionOnBoard == clickedPosition.RowPositionOnBoard &&
                        selectedPosition.Value.ColumnPositionOnBoard == clickedPosition.ColumnPositionOnBoard)
                    {
                        DeselectChecker();
                        return;
                    }

                    if (board.CurrentPlayer.PlayerType == ePlayerType.Human)
                    {
                        if (board.TryMove(selectedPosition, clickedPosition))
                        {
                            UpdateBoardUI();

                            if (!board.IsGameFinished && board.CurrentPlayer.PlayerType == ePlayerType.Computer)
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

                    if (board.IsGameFinished)
                    {
                        HandleGameEnd();
                    }

                    selectedPosition = null;
                }
            }
        }

        private void HighlightCurrentPlayer()
        {
            if (board.CurrentPlayer == board.FirstPlayer)
            {
                lblPlayer1Score.BackColor = Color.LightBlue;
                lblPlayer2Score.BackColor = Color.White;
            }
            else
            {
                lblPlayer1Score.BackColor = Color.White;
                lblPlayer2Score.BackColor = Color.LightCoral;
            }

            lblPlayer1Score.ForeColor = Color.Black;
            lblPlayer2Score.ForeColor = Color.Black;
            lblPlayer1Score.BorderStyle = BorderStyle.FixedSingle;
            lblPlayer2Score.BorderStyle = BorderStyle.FixedSingle;
        }

        private void DeselectChecker()
        {
            if (selectedPosition.HasValue)
            {
                Position position = selectedPosition.Value;
                Button selectedButton = cellButtons[position.RowPositionOnBoard, position.ColumnPositionOnBoard];
                Checker checker = board.GameBoard[position.RowPositionOnBoard, position.ColumnPositionOnBoard].CurrentCheckerPiece;
                if (checker != null)
                {
                    selectedButton.BackColor = checker.OwnerPlayer == board.FirstPlayer
                        ? Color.LightBlue
                        : Color.LightCoral;
                }
                else
                {
                    // Reset to default colors if there's no checker (unlikely)
                    selectedButton.BackColor = (position.RowPositionOnBoard + position.ColumnPositionOnBoard) % 2 == 0
                    ? Color.Gray : Color.White;
                }
                selectedPosition = null;
            }
        }

        private void RestartGame()
        {
            board.Restart();
            UpdateBoardUI();
            selectedPosition = null;

            // Trigger the computer's move automatically if it starts the game
            if (board.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                TriggerComputerMove();
            }
        }

        private void TriggerComputerMove()
        {
            // Add a slight delay to make the AI move feel natural
            System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    board.ActivateComputerMove();
                    UpdateBoardUI();

                    // Check if the game ends after the computer's move
                    if (board.IsGameFinished)
                    {
                        HandleGameEnd();
                    }
                });
            });
        }

        private void HandleGameEnd()
        {
            string message = board.WinnerPlayer != null
                ? $"{board.WinnerPlayer.PlayerName} Won!\nAnother Round?"
                : "Tie!\nAnother Round?";

            string title = "Damka";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;

            DialogResult result = MessageBox.Show(message, title, buttons, icon);

            if (result == DialogResult.Yes)
            {
                RestartGame();
            }
            else
            {
                Application.Exit();
            }
        }

        private void UpdatePlayerScores()
        {
            lblPlayer1Score.Text = $"{board.FirstPlayer.PlayerName}: {board.FirstPlayer.Score}";
            lblPlayer2Score.Text = $"{board.SecondPlayer.PlayerName}: {board.SecondPlayer.Score}";
        }
    }
}

