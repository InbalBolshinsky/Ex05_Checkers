using CheckersGameLogic;
using System.Windows.Forms;
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

        public FormGameBoard(string i_BoardSizeText, string i_Player1Name, string i_Player2Name, bool i_IsPlayer2Computer)
        {
            InitializeComponent();

            r_BoardSize = int.Parse(i_BoardSizeText.Split('x')[0]);
            r_Board = new Board((eBoardSize)r_BoardSize, i_Player1Name, i_Player2Name, i_IsPlayer2Computer ? eGameType.AgainstComputer : eGameType.AgainstHuman);

            this.Text = $"Checkers Game - {i_Player1Name} vs {(i_IsPlayer2Computer ? "Computer" : i_Player2Name)}";

           InitializeBoardUI();
           //SetupNearTieScenario();
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
                        cellButton.Click += OnCellButtonClick; 
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
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    }
                    else
                    {
                        Checker checker = boardPosition.CurrentCheckerPiece;
                        cellButton.Text = checker.PieceType == eCheckerType.King
                           ? (checker.OwnerPlayer == r_Board.FirstPlayer ? "K" : "U")
                           : (checker.OwnerPlayer == r_Board.FirstPlayer ? "X" : "O");
                        cellButton.BackColor = checker.OwnerPlayer == r_Board.FirstPlayer
                            ? Color.LightBlue
                            : Color.LightCoral;
                    }
                }
            }
            HighlightCurrentPlayer();
            UpdatePlayerScores();
        }

        private void OnCellButtonClick(object? i_Sender, EventArgs i_Event)
        {
            if (i_Sender is Button clickedButton && clickedButton.Tag is Position clickedPosition)
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
            message = FormWinnerMessage();

            DialogResult result = MessageBox.Show(message, title, buttons, icon);
            if (result == DialogResult.Yes)
            {
                RestartGame();
            }
            else
            {
                FormFinalWinnerMessage();
                Application.Exit();
            }
        }

        private void FormFinalWinnerMessage()
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
        private string FormWinnerMessage()
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
            return message;
        }

        private void UpdatePlayerScores()
        {
            m_LblPlayer1Score.Text = $"{r_Board.FirstPlayer.PlayerName}: {m_FirstPlayerWins}";
            m_LblPlayer2Score.Text = $"{r_Board.SecondPlayer.PlayerName}: {m_SecondPlayerWins}";
        }
    }
}