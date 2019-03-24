using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public enum PieceState
    {
        Selected,
        UnSelected
    }

    public class Piece : Sprite
    {
        public Board Board { get; set; }

        public FontManager FontManager { get; set; }

        public int Number { get; set; }

        public int RowNumber { get; set; }

        public int ColumnNumber { get; set; }

        public Color BackgroundColor { get; set; }

        public Texture2D BackgroundTexture { get; set; }

        public Texture2D SelectedTexture { get; set; }

        public Vector2 StartingPosition { get; set; }

        public Vector2 StartingSize { get; set; }

        public PieceState State { get; set; }

        public bool IsEmpty { get; set; }

        public Piece() { }

        public Piece(Vector2 startingPosition, Vector2 startingSize, Color bgColor)
        {
            BackgroundColor = bgColor;            
            SetPosition(startingPosition);
            SetSize(startingSize);
            State = PieceState.UnSelected;
        }

        public override void LoadContent()
        {
            SetBackgroundTexture();

            SelectedTexture = TextureManager.CreateTexture2DBySingleColor(Color.IndianRed,(int)Size.X, (int)Size.Y);

            FontManager = new FontManager("Fonts/MenuFont", string.Format("R:{0},C:{1},N:{2}",RowNumber,ColumnNumber,Number.ToString()), Position, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f, Vector2.Zero);
        }

        public override void Update()
        {
            FontManager.CalculateCenterVector2(DestinationRectangle);

            base.Update();
        }

        public override void Draw()
        {
            Global.SpriteBatch.Draw(BackgroundTexture, DestinationRectangle, Color);

            if (!IsEmpty)
            {
                if (Texture != null)
                    base.Draw();

                if(State==PieceState.Selected)
                    Global.SpriteBatch.Draw(SelectedTexture, DestinationRectangle, Color);
            }

            FontManager.Draw();
        }

        public Piece MakeEmpty()
        {
            //BackgroundColor = new Color((byte)0, (byte)0, (byte)0, (byte)(0.5 * 255));

            //SetBackgroundTexture();

            BackgroundTexture = TextureManager.CreateTexture2D("Textures/emptyPiece");

            IsEmpty = true;

            return this;
        }

        public Piece SetBoard(Board board)
        {
            Board = board;

            return this;
        }

        public Piece SetBackgroundTexture()
        {
            BackgroundTexture = TextureManager.CreateTexture2DBySingleColor(BackgroundColor, (int)Size.X, (int)Size.Y);

            return this;
        }

        public Piece SetNumber(int number)
        {
            Number = number;

            //FontManager.SetText(Number.ToString());

            return this;
        }

        public Piece Select()
        {
            if (!IsEmpty)
            {
                State = PieceState.Selected;

                SetColor(Color.Red);
            }

            return this;
        }

        public Piece UnSelect()
        {
            State = PieceState.UnSelected;

            SetColor(Color.White);

            return this;
        }

        public Piece SetRowAndColumnNumber(int rowNumber,int columnNumber)
        {
            RowNumber = rowNumber;

            ColumnNumber = columnNumber;

            return this;
        }

        public Piece GetTopNeighbor(List<Piece> pieceList)
        {
            return pieceList.FirstOrDefault(p => p.Number == (Number - 8));
        }

        public Piece GetBottomNeighbor(List<Piece> pieceList)
        {
            return pieceList.FirstOrDefault(p => p.Number == (Number + 8));
        }

        public Piece GetLeftNeighbor(List<Piece> pieceList)
        {
            return ColumnNumber!=0 ? pieceList.FirstOrDefault(p => p.Number == (Number - 1)) : null;
        }

        public Piece GetRightNeighbor(List<Piece> pieceList)
        {
            return Board!=null && ColumnNumber!=Board.ColumnCount-1 ? pieceList.FirstOrDefault(p => p.Number == (Number + 1)) : null;
        }

        public bool IsNeighborWith(List<Piece> pieceList,Piece piece)
        {
            var topNeighbor = GetTopNeighbor(pieceList);
            var bottomNeighbor = GetBottomNeighbor(pieceList);
            var leftNeighbor = GetLeftNeighbor(pieceList);
            var rightNeighbor = GetRightNeighbor(pieceList);

            if ((topNeighbor != null && topNeighbor.Id == piece.Id) ||
                (bottomNeighbor != null && bottomNeighbor.Id == piece.Id) ||
                (leftNeighbor != null && leftNeighbor.Id == piece.Id) ||
                (rightNeighbor != null && rightNeighbor.Id == piece.Id))
                return true;
            else 
                return false;
        }

        public static void Replace(Piece selectedPiece,Piece previouslySelectedPiece,Piece[,] Pieces)
        {
            var emptyPiece = Pieces[selectedPiece.RowNumber, selectedPiece.ColumnNumber];
            var tempPiece = emptyPiece;
            var filledPiece = Pieces[previouslySelectedPiece.RowNumber, previouslySelectedPiece.ColumnNumber];
            emptyPiece = filledPiece;
            filledPiece = tempPiece;

            var tempPositon = emptyPiece.Position;
            emptyPiece.Position = filledPiece.Position;
            filledPiece.Position = tempPositon;

            var tempRowNumber = emptyPiece.RowNumber;
            var tempColumnNumber = emptyPiece.ColumnNumber;
            emptyPiece.SetRowAndColumnNumber(filledPiece.RowNumber, filledPiece.ColumnNumber);
            filledPiece.SetRowAndColumnNumber(tempRowNumber, tempColumnNumber);

            var tempNumber = emptyPiece.Number;
            emptyPiece.SetNumber(filledPiece.Number);
            filledPiece.SetNumber(tempNumber);

            emptyPiece.FontManager.SetText(string.Format("({0},{1})={2}", emptyPiece.RowNumber, emptyPiece.ColumnNumber, emptyPiece.Number.ToString()));
            filledPiece.FontManager.SetText(string.Format("({0},{1})={2}", filledPiece.RowNumber, filledPiece.ColumnNumber, filledPiece.Number.ToString()));

            Pieces[emptyPiece.RowNumber, emptyPiece.ColumnNumber] = emptyPiece;
            Pieces[filledPiece.RowNumber, filledPiece.ColumnNumber] = filledPiece;
        }

    }
}
