namespace CheckersGameLogic
{
    public class BoardPosition
    {
        private readonly Position r_CurrentCellPostion;
        private Checker m_CellCheckerPiece;

        public BoardPosition(Position i_CurrentCellPosition, Checker i_CheckerPiece)
        {
            this.r_CurrentCellPostion = i_CurrentCellPosition;
            this.m_CellCheckerPiece = i_CheckerPiece;
        }

        public Checker CurrentCheckerPiece
        {
            get
            {
                return this.m_CellCheckerPiece;
            }
            set
            {
                if (value == null)
                {
                    this.m_CellCheckerPiece = null;
                }
                else
                {
                    this.m_CellCheckerPiece = value;
                    this.m_CellCheckerPiece.CheckerPiecePosition = this.r_CurrentCellPostion;
                }
            }
        }

        public bool IsEmpty()
        {
            return this.m_CellCheckerPiece == null;
        }

        public void Clear()
        {
            this.m_CellCheckerPiece = null;
        }
    }
}