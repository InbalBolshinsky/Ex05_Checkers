﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckersGameLogic
{
    public class Player
    {
        private readonly String r_PlayerName;
        private readonly ePlayerType r_PlayerType;
        private readonly List<Checker> r_PlayerPiecesList;
        private readonly List<Move> r_PossibleMovesList;
        private readonly List<Move> r_CaptureMovesList;
        private readonly char r_PlayerPieceSymbol;
        private readonly char r_PlayerKingSymbol;
        private int m_Score;
        private int m_KingsCounter;
        public List<Checker> m_CapturedPieces = new List<Checker>();

        public Player(String i_PlayerName, ePlayerType i_PlayerType, char i_PlayerPieceSymbol, char i_PlayerKingSymbol)
        {
            this.r_PlayerName = i_PlayerName;
            this.r_PlayerType = i_PlayerType;
            this.r_PlayerPieceSymbol = i_PlayerPieceSymbol;
            this.r_PlayerKingSymbol = i_PlayerKingSymbol;
            this.m_Score = 0;
            this.m_KingsCounter = 0;
            this.r_PlayerPiecesList = new List<Checker>();
            this.r_PossibleMovesList = new List<Move>();
            this.r_CaptureMovesList = new List<Move>();
        }

        public String PlayerName
        {
            get
            {
                return this.r_PlayerName;
            }
        }

        public ePlayerType PlayerType
        {
            get
            {
                return this.r_PlayerType;
            }
        }

        public char PieceSymbol
        {
            get
            {
                return this.r_PlayerPieceSymbol;
            }
        }

        public char KingSymbol
        {
            get
            {
                return this.r_PlayerKingSymbol;
            }
        }

        public int Score
        {
            get
            {
                return this.m_Score;
            }
            set
            {
                this.m_Score = value;
            }
        }

        public int KingsCounter
        {
            get
            {
                return this.m_KingsCounter;
            }
            set
            {
                this.m_KingsCounter = value;
            }
        }

        public int RegularPiecesCounter
        {
            get
            {
                return this.r_PlayerPiecesList.Count - this.m_KingsCounter;
            }
        }

        public List<Checker> PlayerPiecesList
        {
            get
            {
                return this.r_PlayerPiecesList;
            }
        }

        public List<Move> PlayerPossibleMovesList
        {
            get
            {
                return this.r_PossibleMovesList;
            }
        }

        public List<Move> PlayerCaptureMovesList
        {
            get
            {
                return this.r_CaptureMovesList;
            }
        }

        public List<Checker> CapturedPieces
        {
            get 
            {
                return m_CapturedPieces; 
            }
            set 
            {
                m_CapturedPieces = value; 
            }
        }

        public static bool IsNameValid(String i_PlayerName)
        {
            return !i_PlayerName.Contains(' ') && i_PlayerName.Length > 0 && i_PlayerName.Length <= 20;
        }

        public void AddPieceToPlayerListOfPieces(Checker i_CheckerPiece)
        {
            this.r_PlayerPiecesList.Add(i_CheckerPiece);
        }

        public void RemovePieceFromPlayerListOfPieces(Checker i_CheckerPiece)
        {
            this.r_PlayerPiecesList.Remove(i_CheckerPiece);
            if (i_CheckerPiece.PieceType == eCheckerType.King)
            {
                this.m_KingsCounter--;
            }
        }

        public bool IsPiecesListEmpty()
        {
            return this.r_PlayerPiecesList.Count == 0;
        }

        public bool IsPossibleMovesListEmpty()
        {
            return this.r_PossibleMovesList.Count == 0;
        }

        public bool IsCaptureMovesListEmpty()
        {
            return this.r_CaptureMovesList.Count == 0;
        }

        public void ClearPossibleMovesList()
        {
            this.r_PossibleMovesList.Clear();
        }

        public void ClearCaptureMovesList()
        {
            this.r_CaptureMovesList.Clear();
        }

        public void AppendMoveToPossibleMovesList(Move i_PossibleMove)
        {
            this.r_PossibleMovesList.Add(i_PossibleMove);
        }

        public void AppendMoveToCaptureMovesList(Move i_CaptureMove)
        {
            this.r_CaptureMovesList.Add(i_CaptureMove);
        }

        public void InitializePlayer()
        {
            this.m_KingsCounter = 0;
            this.r_PlayerPiecesList.Clear();
            this.r_PossibleMovesList.Clear();
            this.r_CaptureMovesList.Clear();
        }
    }
}