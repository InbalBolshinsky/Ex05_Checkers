using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersUI
{
    public partial class GameBoardForm : Form
    {
        private readonly Board board; // Instance of the game board logic
        private readonly int boardSize; // Board size as an integer

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
                }
            }
        }

        private void OnCellButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Cast the Tag property directly to Position
                Position position = (Position)clickedButton.Tag;

                // Use the position for move logic
                if (board.TryMove(position.ToString(), position.ToString())) // Replace with actual move logic
                {
                    UpdateBoardUI();
                }
            }
        }

        private void UpdateBoardUI()
        {
            // Update the board's state and refresh the UI (to be implemented)
            MessageBox.Show("Update board logic goes here!");
        }
    }
}
