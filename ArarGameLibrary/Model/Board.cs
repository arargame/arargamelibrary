using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class Board : BaseObject, IXna, IPieceContainer
    {
        public Level Level { get; set; }

        //public Image Image { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public Vector2 PieceSize { get; set; }

        //public Piece[,] Pieces { get; set; }
        public List<Piece> Pieces { get; set; }

        public Board(int rowCount, int columnCount)
        {
            //Pieces = new Piece[rowCount, columnCount];
            RowCount = rowCount;
            ColumnCount = columnCount;

            Initialize();
        }

        public void Initialize()
        {
            PieceSize = new Vector2((float)Global.ViewportWidth / ColumnCount, (float)Global.ViewportHeight / RowCount);


            //Board'un başlangıç pozisyonu belirlenebilir
            Pieces = Piece.To2DPieceArray(PieceSize, RowCount, ColumnCount, Vector2.Zero).OfType<Piece>().ToList();

            foreach (var piece in Pieces)
            {
                piece.UnSelect()
                    .SetContainer(this);
            }
        }

        public void LoadContent(Texture2D texture = null)
        {
            foreach (var piece in Pieces)
            {
                piece.LoadContent();

                piece.SetText();
            }

            var emptyPieceNumber = Global.Random.Next(0, Pieces.OfType<Piece>().Count());

            var emptyPiece = Pieces.OfType<Piece>().FirstOrDefault(p => p.Number == emptyPieceNumber);

            emptyPiece.MakeEmpty();
        }


        public void Update(GameTime gameTime = null)
        {
            var pieceList = Pieces.OfType<Piece>().ToList();

            foreach (var piece in pieceList)
            {
                if (InputManager.Selected(piece.DestinationRectangle))
                {
                    if (!pieceList.Any(p => p.State == PieceState.Selected))
                        piece.Select();
                    else
                    {
                        var previouslySelectedPiece = pieceList.FirstOrDefault(p => p.State == PieceState.Selected);

                        previouslySelectedPiece.UnSelect();

                        if (previouslySelectedPiece.Id != piece.Id)
                        {
                            if (piece.IsEmpty && previouslySelectedPiece.IsNeighborWith(pieceList, piece))
                            {
                                Piece.Replace(piece, previouslySelectedPiece, Pieces);
                            }
                            else
                                piece.Select();
                        }
                    }
                }

                piece.Update();
            }

            UnSelectOthersPieces();

            // Image.Update();
        }

        public void Draw(SpriteBatch spriteBatch = null)
        {
            foreach (var piece in Pieces)
            {
                piece.Draw();
            }

            //Image.Draw();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public Board SetImage(Image image)
        {
            //Image = image;

            return this;
        }

        public void UnSelectOthersPieces()
        {
            var pieceList = Pieces.OfType<Piece>();

            foreach (var piece in pieceList)
            {
                var selectedPiece = pieceList.FirstOrDefault(p => p.State == PieceState.Selected);

                if (selectedPiece != null && piece.Id != selectedPiece.Id)
                    piece.UnSelect();
            }
        }

        public Piece GetPiece(int rowNumber, int columnNumber)
        {
            return Pieces.FirstOrDefault(p => p.RowNumber == rowNumber && p.ColumnNumber == columnNumber);
        }
    }
}
