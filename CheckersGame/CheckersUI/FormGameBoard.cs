using System;
using System.Drawing;
using System.Windows.Forms;
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

            lblPlayer1Score.Text = $"{player1Name}: ";
            lblPlayer2Score.Text = isPlayer2Computer ? "Computer: 0" : $"{player2Name}: 0";

            InitializeBoardUI();
        }

        private void InitializeBoardUI()
        {
            int buttonSize = 50; // Each cell's size
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
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.Gray : Color.White;
                    }
                    else
                    {
                        Checker checker = boardPosition.CurrentCheckerPiece;
                        if (checker.PieceType == eCheckerType.King)
                        {
                            cellButton.Text = checker.OwnerPlayer == board.FirstPlayer ? "K" : "U";
                        }
                        else
                        {
                            cellButton.Text = checker.OwnerPlayer == board.FirstPlayer ? "X" : "O";
                        }
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.Gray : Color.White;
                    }
                }
            }
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
                        clickedButton.BackColor = Color.LightBlue;
                    }
                }
                else
                {
                    if (selectedPosition.HasValue &&
                        selectedPosition?.RowPositionOnBoard == clickedPosition.RowPositionOnBoard &&
                        selectedPosition?.ColumnPositionOnBoard == clickedPosition.ColumnPositionOnBoard)
                    {
                        DeselectChecker();
                        return;
                    }
                    if (board.TryMove(selectedPosition, clickedPosition))
                    {
                        UpdateBoardUI();
                        if (board.IsGameFinished)
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
                    }
                    else
                    {
                        MessageBox.Show("Invalid move. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DeselectChecker();
                    }
                    selectedPosition = null;
                }
            }
        }

        private void DeselectChecker()
        {
            if (selectedPosition.HasValue)
            {
                Position position = selectedPosition.Value;
                Button selectedButton = cellButtons[position.RowPositionOnBoard, position.ColumnPositionOnBoard];

                Checker checker = board.GameBoard[position.RowPositionOnBoard, position.ColumnPositionOnBoard].CurrentCheckerPiece;
                selectedButton.BackColor = (position.RowPositionOnBoard + position.ColumnPositionOnBoard) % 2 == 0
                ? Color.Gray : Color.White;

                selectedPosition = null;
            }
        }

        private void RestartGame()
        { 
            board.Restart();  
            UpdateBoardUI();  
            selectedPosition = null; 
        }
    }
}