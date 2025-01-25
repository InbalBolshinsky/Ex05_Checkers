using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersUI
{
    public partial class GameBoardForm : Form
    {
        private readonly Board board;
        private readonly int boardSize;
        private Button[,] cellButtons; // Array to hold button references for the board cells

        public GameBoardForm(string boardSizeText, string player1Name, string player2Name)
        {
            InitializeComponent();

            // Parse the board size from the text (e.g., "6x6")
            boardSize = int.Parse(boardSizeText.Substring(0, 1));

            // Initialize the Board logic
            board = new Board((eBoardSize)boardSize, player1Name, player2Name, eGameType.AgainstHuman);

            // Set the form title
            this.Text = $"Checkers Game - {player1Name} vs {player2Name}";

            // Initialize the UI for the game board
            InitializeBoardUI();
        }

        private void InitializeBoardUI()
        {
            int buttonSize = 50; // Size of each button representing a board cell
            cellButtons = new Button[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    // Create a button for each board cell
                    Button cellButton = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(50 + col * buttonSize, 50 + row * buttonSize),
                        BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray,
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Position(row, col) // Store the cell's position in the Tag
                    };

                    // Add the click event
                    cellButton.Click += OnCellButtonClick;

                    // Add the button to the form
                    this.Controls.Add(cellButton);

                    // Store the button in the array for later use
                    cellButtons[row, col] = cellButton;
                }
            }

            // Update the UI with the initial state of the board
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
                        // Empty cell
                        cellButton.Text = string.Empty;
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    }
                    else
                    {
                        // Cell contains a checker piece
                        Checker checker = boardPosition.CurrentCheckerPiece;
                        cellButton.Text = checker.OwnerPlayer == board.FirstPlayer ? "X" : "O";
                        cellButton.BackColor = checker.OwnerPlayer == board.FirstPlayer
                            ? Color.LightBlue
                            : Color.LightCoral;
                    }
                }
            }
        }

        private void OnCellButtonClick(object sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is Position position)
            {
                // Get the start and end positions (example, should be modified based on actual gameplay logic)
                Position startPosition = position; // Placeholder for start position
                Position endPosition = position;   // Placeholder for end position

                if (board.TryMove(startPosition.ToString(), endPosition.ToString()))
                {
                    // After a successful move, update the board UI
                    UpdateBoardUI();

                    if (board.IsGameFinished)
                    {
                        string winner = board.WinnerPlayer != null ? board.WinnerPlayer.PlayerName : "No one";
                        MessageBox.Show($"{winner} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid move. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}