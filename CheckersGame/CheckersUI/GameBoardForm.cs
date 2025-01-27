﻿using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersUI
{
    public partial class GameBoardForm : Form
    {
        private readonly Board board;
        private readonly int boardSize;
        private Button[,] cellButtons; // 2D array to hold references to the grid buttons
        private Position? selectedPosition; // Tracks the selected piece for movement

        public GameBoardForm(string boardSizeText, string player1Name, string player2Name, bool isPlayer2Computer)
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
                        Tag = new Position(row, col)
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
                        cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    }
                    else
                    {
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
            if (sender is Button clickedButton && clickedButton.Tag is Position clickedPosition)
            {
                if (selectedPosition == null)
                {
                    // Select a piece to move
                    if (!board.GameBoard[clickedPosition.RowPositionOnBoard, clickedPosition.ColumnPositionOnBoard].IsEmpty())
                    {
                        selectedPosition = clickedPosition;
                        clickedButton.BackColor = Color.Yellow; // Highlight selected piece
                    }
                }
                else
                {
                    // Attempt to move the selected piece
                    if (board.TryMove(selectedPosition.ToString(), clickedPosition.ToString()))
                    {
                        UpdateBoardUI(); // Update the board after a successful move

                        if (board.IsGameFinished)
                        {
                            string winner = board.WinnerPlayer != null ? board.WinnerPlayer.PlayerName : "No one";
                            MessageBox.Show($"{winner} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid move. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Deselect the piece
                    selectedPosition = null;
                    //ResetBoardColors();
                }
            }
        }

        private void ResetBoardColors()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button cellButton = cellButtons[row, col];
                    cellButton.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                }
            }
        }
    }
}