using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
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
    public class Board : BaseObject, IXna, IPieceContainer
    {
        public GameLevel Level { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public Vector2 PieceSize { get; set; }

        public List<Piece> Pieces { get; set; }

        public Vector2 Size { get; set; }

        public Board(int rowCount, int columnCount, Vector2 size)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            Size = size;

            Initialize();
        }

        public void Initialize()
        {
            //Burada board ın sınırları çizilmeis gerekiyor ,bütün ekrana göre hesaplanmamalı
            PieceSize = new Vector2((int)(Size.X / ColumnCount),(int)(Size.Y / RowCount));


            //Board'un başlangıç pozisyonu belirlenebilir
            Pieces = Piece.To2DPieceArray(PieceSize, RowCount, ColumnCount, Vector2.Zero).OfType<Piece>().ToList();

            foreach (var piece in Pieces)
            {
                piece.Unselect()
                    .SetContainer(this);
            }
        }

        public void LoadContent(Texture2D texture = null)
        {
            foreach (var piece in Pieces)
            {
                piece.LoadContent();

                piece.SetDrawMethodType(5);
            }

            var emptyPieceNumber = Global.RandomNext(0, Pieces.OfType<Piece>().Count());

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

                        previouslySelectedPiece.Unselect();

                        if (previouslySelectedPiece.Id != piece.Id)
                        {
                            if (piece.IsEmpty && previouslySelectedPiece.IsNeighborWith(piece))
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
        }

        public void Draw(SpriteBatch spriteBatch = null)
        {
            foreach (var piece in Pieces)
            {
                piece.Draw();
            }
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void UnSelectOthersPieces()
        {
            var pieceList = Pieces.OfType<Piece>();

            foreach (var piece in pieceList)
            {
                var selectedPiece = pieceList.FirstOrDefault(p => p.State == PieceState.Selected);

                if (selectedPiece != null && piece.Id != selectedPiece.Id)
                    piece.Unselect();
            }
        }

        public Piece GetPiece(int rowNumber, int columnNumber)
        {
            return Pieces.FirstOrDefault(p => p.RowNumber == rowNumber && p.ColumnNumber == columnNumber);
        }

        public Board SpreadImagePiecesOnTheBoard(Image image)
        {
            var spreadedImagePieces = General.PopulateListRandomlyFromAnother<Piece>(Pieces.Where(p => !p.IsEmpty).ToList(), image.Pieces.Count);

            for (int i = 0; i < image.Pieces.Count; i++)
            {
                var imagePiece = image.Pieces[i];

                spreadedImagePieces[i].Id = imagePiece.Id;

                spreadedImagePieces[i].MakeImage(imagePiece.ImageNumber, imagePiece.Texture);
            }

            return this;
        }
    }
}
