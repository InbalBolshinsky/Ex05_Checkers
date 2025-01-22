﻿using System;
using System.Collections.Generic;

namespace CheckersGameLogic
{
    public class Board
    {
        private readonly eBoardSize r_BoardSize;
        private readonly BoardPosition[,] r_Board;
        private readonly Player r_FirstPlayer;
        private readonly Player r_SecondPlayer;
        private readonly eGameType r_GameType;
        private readonly Player[] r_PlayersReferencesArray;
        private Player m_WinnerPlayer;
        private Player m_LastPlayer;
        private ePlayerTurn m_PlayerTurn;
        private bool m_IsGameFinished;
        private const int k_KingScore = 4;

        public Board(eBoardSize i_BoardSize, String i_FirstPlayerName, String i_SecondPlayerName, eGameType i_GameMode)
        {
            this.r_GameType = i_GameMode;
            this.r_BoardSize = i_BoardSize;
            this.r_Board = new BoardPosition[(int)this.r_BoardSize, (int)this.r_BoardSize];
            this.r_FirstPlayer = new Player(i_FirstPlayerName, ePlayerType.Human, 'X', 'K');
            this.r_SecondPlayer = new Player(i_SecondPlayerName, (ePlayerType)r_GameType, 'O', 'U');
            this.r_PlayersReferencesArray = new Player[] { r_FirstPlayer, r_SecondPlayer };
            initializeGame();
            initializeBoard();
        }

        private void initializeGame()
        {
            this.m_PlayerTurn = ePlayerTurn.Player1;
            this.m_LastPlayer = this.r_FirstPlayer;
            this.m_IsGameFinished = false;
            this.m_WinnerPlayer = null;
            this.r_FirstPlayer.InitializePlayer();
            this.r_SecondPlayer.InitializePlayer();
        }

        private void initializeBoard()
        {
            int firstSeparateRowIndex = ((int)this.r_BoardSize - 2) / 2;
            int secondSeparateRowIndex = firstSeparateRowIndex + 1;
            Position currentCellPosition;
            Checker currentCheckerPiece;

            for (int rowIndex = 0; rowIndex < (int)this.r_BoardSize; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < (int)this.r_BoardSize; ++columnIndex)
                {
                    currentCellPosition = new Position(rowIndex, columnIndex);
                    currentCheckerPiece = null;

                    if (isAlternatingCell(rowIndex, columnIndex))
                    {
                        if (rowIndex < firstSeparateRowIndex)
                        {
                            currentCheckerPiece = new Checker(this.r_SecondPlayer, eCheckerType.Regular, currentCellPosition);
                            r_SecondPlayer.AddPieceToPlayerListOfPieces(currentCheckerPiece);
                        }
                        else if (rowIndex > secondSeparateRowIndex)
                        {
                            currentCheckerPiece = new Checker(this.r_FirstPlayer, eCheckerType.Regular, currentCellPosition);
                            r_FirstPlayer.AddPieceToPlayerListOfPieces(currentCheckerPiece);
                        }
                    }

                    this.r_Board[rowIndex, columnIndex] = new BoardPosition(currentCellPosition, currentCheckerPiece);
                }
            }
        }

        private bool isAlternatingCell(int i_RowIndex, int i_ColumnIndex)
        {
            return (i_RowIndex % 2 == 0 && i_ColumnIndex % 2 != 0) || (i_RowIndex % 2 != 0 && i_ColumnIndex % 2 == 0);
        }

        public void EndGame()
        {
            if (this.OpponentPlayer.IsPiecesListEmpty() || isOpponentBlocked())
            {
                this.m_WinnerPlayer = this.CurrentPlayer;
                this.m_WinnerPlayer.Score += calculateScoresToAdd();
            }
            else if (!this.IsGameFinished)
            {
                this.m_LastPlayer = this.CurrentPlayer;
                this.m_WinnerPlayer = this.OpponentPlayer;
                this.OpponentPlayer.Score += calculateScoresToAdd();
                this.m_IsGameFinished = true;
            }
            else
            {
                this.m_WinnerPlayer = null;
            }
        }

        private bool isOpponentBlocked()
        {
            return !this.CurrentPlayer.IsPossibleMovesListEmpty() && this.OpponentPlayer.IsPossibleMovesListEmpty();
        }

        public void Restart()
        {
            initializeGame();
            initializeBoard();
        }

        private int calculateScoresToAdd()
        {
            return Math.Abs((this.CurrentPlayer.RegularPiecesCounter + (this.CurrentPlayer.KingsCounter * k_KingScore))
                          - (this.OpponentPlayer.RegularPiecesCounter + (this.OpponentPlayer.KingsCounter * k_KingScore)));
        }

        public Move ActivateComputerMove()
        {
            Random randomObject = new Random();
            int randomMoveIndex;
            Move computerMove;

            if (!this.CurrentPlayer.IsCaptureMovesListEmpty())
            {
                randomMoveIndex = randomObject.Next(0, this.CurrentPlayer.PlayerCaptureMovesList.Count);
                computerMove = this.CurrentPlayer.PlayerCaptureMovesList[randomMoveIndex];
            }
            else
            {
                randomMoveIndex = randomObject.Next(0, this.CurrentPlayer.PlayerPossibleMovesList.Count);
                computerMove = this.CurrentPlayer.PlayerPossibleMovesList[randomMoveIndex];
            }

            makeMove(computerMove);

            return computerMove;
        }

        public bool TryMove(String i_StartPositionAsString, String i_EndPositionAsString)
        {
            bool isMoveSucceed = false;
            Position startPosition = convertStringToPosition(i_StartPositionAsString);
            Position endPosition = convertStringToPosition(i_EndPositionAsString);
            Move playerMove = new Move(startPosition, endPosition);

            if (isMoveValid(playerMove, this.CurrentPlayer))
            {
                if (!playerMove.ShouldCapture || (playerMove.ShouldCapture && isMoveInCaptureMovesList(playerMove, this.CurrentPlayer)))
                {
                    makeMove(playerMove);
                    isMoveSucceed = true;
                }
            }

            return isMoveSucceed;
        }

        private void makeMove(Move i_PlayerMove)
        {
            int startPositionRowIndex = i_PlayerMove.StartPosition.RowPositionOnBoard;
            int startPositionColumnIndex = i_PlayerMove.StartPosition.ColumnPositionOnBoard;
            int endPositionRowIndex = i_PlayerMove.EndPosition.RowPositionOnBoard;
            int endPositionColumnIndex = i_PlayerMove.EndPosition.ColumnPositionOnBoard;

            this.r_Board[endPositionRowIndex, endPositionColumnIndex].CurrentCheckerPiece = this.r_Board[startPositionRowIndex, startPositionColumnIndex].CurrentCheckerPiece;
            this.r_Board[startPositionRowIndex, startPositionColumnIndex].Clear();
            checkIfShouldChangePieceState(this.r_Board[endPositionRowIndex, endPositionColumnIndex].CurrentCheckerPiece);

            if (i_PlayerMove.ShouldCapture)
            {
                capturePiece(i_PlayerMove);
            }

            checkIfGameEnds(i_PlayerMove);
            this.m_LastPlayer = this.CurrentPlayer;

            if (!i_PlayerMove.ShouldCapture || this.CurrentPlayer.IsCaptureMovesListEmpty())
            {
                switchTurn();
            }
        }

        private bool checkIfGameEnds(Move i_LastMove)
        {
            updatePlayersPossibleAndCaptureMovesLists(i_LastMove);
            this.m_IsGameFinished = this.OpponentPlayer.IsPiecesListEmpty() ||
                                    (this.OpponentPlayer.IsPossibleMovesListEmpty() && this.OpponentPlayer.IsCaptureMovesListEmpty());
            if (m_IsGameFinished)
            {
                this.EndGame();
            }

            return this.m_IsGameFinished;
        }

        private void updatePlayersPossibleAndCaptureMovesLists(Move i_LastMove)
        {
            foreach (Player player in this.r_PlayersReferencesArray)
            {
                player.ClearPossibleMovesList();
                player.ClearCaptureMovesList();

                if (player == this.CurrentPlayer && i_LastMove.ShouldCapture)
                {
                    updatePlayerCapturingMovesPerPiece(player, i_LastMove.EndPosition);
                }
                else
                {
                    updateAllPlayerCapturingMoves(player);
                }

                if (player.IsCaptureMovesListEmpty())
                {
                    updateAllPlayerPossibleMoves(player);
                }
            }
        }

        private void updateAllPlayerCapturingMoves(Player i_Player)
        {
            foreach (Checker playerPiece in i_Player.PlayerPiecesList)
            {
                Position startPosition = playerPiece.CheckerPiecePosition;
                List<Position> potentialCaptureMovesPositions = getPotentialPositions(playerPiece, startPosition, 1);

                foreach (Position endPosition in potentialCaptureMovesPositions)
                {
                    Move potentialCaptureMove = new Move(startPosition, endPosition);

                    if (isMoveValid(potentialCaptureMove, i_Player))
                    {
                        i_Player.AppendMoveToCaptureMovesList(potentialCaptureMove);
                    }
                }
            }
        }

        private void updatePlayerCapturingMovesPerPiece(Player i_Player, Position i_LastMoveEndPosition)
        {
            Position startPosition = i_LastMoveEndPosition;
            Checker currentCheckerPiece = this.r_Board[startPosition.RowPositionOnBoard, startPosition.ColumnPositionOnBoard].CurrentCheckerPiece;
            List<Position> potentialCaptureMovesPositions = getPotentialPositions(currentCheckerPiece, startPosition, 1);

            foreach (Position endPosition in potentialCaptureMovesPositions)
            {
                Move potentialCaptureMove = new Move(startPosition, endPosition);

                if (isMoveValid(potentialCaptureMove, i_Player))
                {
                    i_Player.AppendMoveToCaptureMovesList(potentialCaptureMove);
                }
            }
        }

        private void updateAllPlayerPossibleMoves(Player i_Player)
        {
            foreach (Checker playerPiece in i_Player.PlayerPiecesList)
            {
                Position startPosition = playerPiece.CheckerPiecePosition;
                List<Position> potentialMovesPositions = getPotentialPositions(playerPiece, startPosition, 0);

                foreach (Position endPosition in potentialMovesPositions)
                {
                    Move potentialPossibleMove = new Move(startPosition, endPosition);

                    if (isMoveValid(potentialPossibleMove, i_Player))
                    {
                        i_Player.AppendMoveToPossibleMovesList(potentialPossibleMove);
                    }
                }
            }
        }

        private void switchTurn()
        {
            this.m_PlayerTurn = (ePlayerTurn)((int)this.m_PlayerTurn ^ 1);
        }

        private void capturePiece(Move i_PlayerMove)
        {
            int middleRowIndex = (i_PlayerMove.StartPosition.RowPositionOnBoard + i_PlayerMove.EndPosition.RowPositionOnBoard) / 2;
            int middleColumnIndex = (i_PlayerMove.StartPosition.ColumnPositionOnBoard + i_PlayerMove.EndPosition.ColumnPositionOnBoard) / 2;

            this.OpponentPlayer.RemovePieceFromPlayerListOfPieces(this.r_Board[middleRowIndex, middleColumnIndex].CurrentCheckerPiece);
            this.r_Board[middleRowIndex, middleColumnIndex].Clear();
        }

        private List<Position> getPotentialPositions(Checker i_Piece, Position i_StartPosition, int i_Offset)
        {
            List<Position> potentialPositions = new List<Position>();

            if (i_Piece.PieceType == eCheckerType.King)
            {
                potentialPositions.Add(getLeftUpDiagonal(i_StartPosition, i_Offset));
                potentialPositions.Add(getRightUpDiagonal(i_StartPosition, i_Offset));
                potentialPositions.Add(getLeftDownDiagonal(i_StartPosition, i_Offset));
                potentialPositions.Add(getRightDownDiagonal(i_StartPosition, i_Offset));
            }
            else if (i_Piece.OwnerPlayer == this.r_FirstPlayer)
            {
                potentialPositions.Add(getLeftUpDiagonal(i_StartPosition, i_Offset));
                potentialPositions.Add(getRightUpDiagonal(i_StartPosition, i_Offset));
            }
            else
            {
                potentialPositions.Add(getRightDownDiagonal(i_StartPosition, i_Offset));
                potentialPositions.Add(getLeftDownDiagonal(i_StartPosition, i_Offset));
            }

            return potentialPositions;
        }

        private Position getLeftUpDiagonal(Position i_StartPosition, int i_Offset)
        {
            return new Position(i_StartPosition.RowPositionOnBoard - 1 - i_Offset, i_StartPosition.ColumnPositionOnBoard - 1 - i_Offset);
        }

        private Position getRightUpDiagonal(Position i_StartPosition, int i_Offset)
        {
            return new Position(i_StartPosition.RowPositionOnBoard - 1 - i_Offset, i_StartPosition.ColumnPositionOnBoard + 1 + i_Offset);
        }

        private Position getLeftDownDiagonal(Position i_StartPosition, int i_Offset)
        {
            return new Position(i_StartPosition.RowPositionOnBoard + 1 + i_Offset, i_StartPosition.ColumnPositionOnBoard - 1 - i_Offset);
        }

        private Position getRightDownDiagonal(Position i_StartPosition, int i_Offset)
        {
            return new Position(i_StartPosition.RowPositionOnBoard + 1 + i_Offset, i_StartPosition.ColumnPositionOnBoard + 1 + i_Offset);
        }

        private void checkIfShouldChangePieceState(Checker i_CheckerPiece)
        {
            if (i_CheckerPiece.PieceType != eCheckerType.King)
            {
                if (i_CheckerPiece.OwnerPlayer == this.r_FirstPlayer && i_CheckerPiece.CheckerPiecePosition.RowPositionOnBoard == 0)
                {
                    i_CheckerPiece.PieceType = eCheckerType.King;
                    this.r_FirstPlayer.KingsCounter++;
                }
                else if (i_CheckerPiece.OwnerPlayer == this.r_SecondPlayer && i_CheckerPiece.CheckerPiecePosition.RowPositionOnBoard == (int)r_BoardSize - 1)
                {
                    i_CheckerPiece.PieceType = eCheckerType.King;
                    this.r_SecondPlayer.KingsCounter++;
                }
            }
        }


        private bool isInBounds(Move i_PlayerMove)
        {
            return i_PlayerMove.StartPosition.RowPositionOnBoard >= 0 && i_PlayerMove.StartPosition.RowPositionOnBoard < (int)this.r_BoardSize
                && i_PlayerMove.StartPosition.ColumnPositionOnBoard >= 0 && i_PlayerMove.StartPosition.ColumnPositionOnBoard < (int)this.r_BoardSize
                && i_PlayerMove.EndPosition.RowPositionOnBoard >= 0 && i_PlayerMove.EndPosition.RowPositionOnBoard < (int)this.r_BoardSize
                && i_PlayerMove.EndPosition.ColumnPositionOnBoard >= 0 && i_PlayerMove.EndPosition.ColumnPositionOnBoard < (int)this.r_BoardSize;
        }

        private bool isMoveValid(Move i_PlayerMove, Player i_CurrentPlayer)
        {
            bool isValid = false;

            if (isInBounds(i_PlayerMove))
            {
                Position moveStartPosition = i_PlayerMove.StartPosition;
                Position moveEndPosition = i_PlayerMove.EndPosition;
                BoardPosition moveStartCell = this.r_Board[moveStartPosition.RowPositionOnBoard, moveStartPosition.ColumnPositionOnBoard];
                BoardPosition moveEndCell = this.r_Board[moveEndPosition.RowPositionOnBoard, moveEndPosition.ColumnPositionOnBoard];
                int columnDifference = Math.Abs(moveStartPosition.ColumnPositionOnBoard - moveEndPosition.ColumnPositionOnBoard);
                int rowDifference;

                if (!moveStartCell.IsEmpty() && moveEndCell.IsEmpty() && moveStartCell.CurrentCheckerPiece.OwnerPlayer == i_CurrentPlayer)
                {
                    if (moveStartCell.CurrentCheckerPiece.PieceType == eCheckerType.King)
                    {
                        rowDifference = Math.Abs(moveStartPosition.RowPositionOnBoard - moveEndPosition.RowPositionOnBoard);
                    }
                    else
                    {
                        if (i_CurrentPlayer == this.r_FirstPlayer)
                        {
                            rowDifference = moveStartPosition.RowPositionOnBoard - moveEndPosition.RowPositionOnBoard;
                        }
                        else
                        {
                            rowDifference = moveEndPosition.RowPositionOnBoard - moveStartPosition.RowPositionOnBoard;
                        }
                    }

                    isValid = rowDifference == columnDifference &&
                        ((rowDifference == 1 && i_CurrentPlayer.IsCaptureMovesListEmpty()) ||
                         (rowDifference == 2 && shouldCapture(i_PlayerMove, i_CurrentPlayer)));
                }
            }

            return isValid;
        }

        private bool shouldCapture(Move i_PlayerMove, Player i_CurrentPlayer)
        {
            Position moveStartPosition = i_PlayerMove.StartPosition;
            Position moveEndPosition = i_PlayerMove.EndPosition;
            int middleRowIndex = (moveStartPosition.RowPositionOnBoard + moveEndPosition.RowPositionOnBoard) / 2;
            int middleColumnIndex = (moveStartPosition.ColumnPositionOnBoard + moveEndPosition.ColumnPositionOnBoard) / 2;
            BoardPosition middleCell = this.r_Board[middleRowIndex, middleColumnIndex];

            i_PlayerMove.ShouldCapture = !middleCell.IsEmpty() && middleCell.CurrentCheckerPiece.OwnerPlayer != i_CurrentPlayer;

            return i_PlayerMove.ShouldCapture;
        }

        private bool isMoveInCaptureMovesList(Move i_PlayerMove, Player i_CurrentPlayer)
        {
            bool isMoveInList = false;

            foreach (Move playerCaptureMove in i_CurrentPlayer.PlayerCaptureMovesList)
            {
                isMoveInList = i_PlayerMove.StartPosition.RowPositionOnBoard == playerCaptureMove.StartPosition.RowPositionOnBoard &&
                               i_PlayerMove.StartPosition.ColumnPositionOnBoard == playerCaptureMove.StartPosition.ColumnPositionOnBoard;

                if (isMoveInList)
                {
                    break;
                }
            }

            return isMoveInList;
        }

        private Position convertStringToPosition(String i_PositionAsString)
        {
            int cellRowIndex = i_PositionAsString[0] - 'A';
            int cellColumnIndex = i_PositionAsString[1] - 'a';

            return new Position(cellRowIndex, cellColumnIndex);
        }

        public static bool IsNameValid(String i_PlayerName)
        {
            return Player.IsNameValid(i_PlayerName);
        }

        public static bool IsSizeValid(eBoardSize i_BoardSize)
        {
            return i_BoardSize == eBoardSize.Small || i_BoardSize == eBoardSize.Medium || i_BoardSize == eBoardSize.Large;
        }

        public static bool IsGameModeValid(eGameType i_GameMode)
        {
            return i_GameMode == eGameType.AgainstHuman || i_GameMode == eGameType.AgainstComputer;
        }

        public BoardPosition[,] GameBoard
        {
            get
            {
                return this.r_Board;
            }
        }

        public eBoardSize BoardSize
        {
            get
            {
                return this.r_BoardSize;
            }
        }

        public Player FirstPlayer
        {
            get
            {
                return this.r_FirstPlayer;
            }
        }

        public Player SecondPlayer
        {
            get
            {
                return this.r_SecondPlayer;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return this.r_PlayersReferencesArray[(int)this.m_PlayerTurn];
            }
        }

        public Player OpponentPlayer
        {
            get
            {
                return this.r_PlayersReferencesArray[(int)this.m_PlayerTurn ^ 1];
            }
        }

        public Player WinnerPlayer
        {
            get
            {
                return this.m_WinnerPlayer;
            }
            set
            {
                this.m_WinnerPlayer = value;
            }
        }

        public Player LastPlayer
        {
            get
            {
                return this.m_LastPlayer;
            }
            set
            {
                this.m_LastPlayer = value;
            }
        }

        public bool IsGameFinished
        {
            get
            {
                return this.m_IsGameFinished;
            }
            set
            {
                this.m_IsGameFinished = value;
            }
        }
    }
}