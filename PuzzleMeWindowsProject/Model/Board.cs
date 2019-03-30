using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public class Board : BaseObject,IXna
    {
        public Level Level { get; set; }

        //public Image Image { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public Vector2 PieceSize { get; set; }

        public Piece[,] Pieces { get; set; }

        public Board(int rowCount, int columnCount)
        {
            Pieces = new Piece[rowCount, columnCount];
            RowCount = rowCount;
            ColumnCount = columnCount;

            Initialize();
        }

        public void Initialize()
        {
            PieceSize = new Vector2(Global.ViewportWidth / ColumnCount, Global.ViewportHeight / RowCount);

            Pieces = Piece.To2DPieceArray(PieceSize,RowCount,ColumnCount);

            foreach (var piece in Pieces)
            {
                piece.UnSelect()
                    .SetBoard(this);
            }
        }

        public void LoadContent()
        {
            foreach (var piece in Pieces)
            {
                piece.LoadContent();

                piece.SetText();
            }

            var emptyPieceNumber = Global.Random.Next(0, Pieces.OfType<Piece>().Count());

            var emptyPiece = Pieces.OfType<Piece>().FirstOrDefault(p=>p.Number==emptyPieceNumber);

            emptyPiece.MakeEmpty();

            //var image = new Image("Textures/shutterstock_360399314");
            //image.LoadContent();
            //image.SetRowAndColumnCount(2, 2);

            //image.Pieces.ForEach(p=>p.SetSize(PieceSize));

            //SetImage(image);
        }


        public void Update()
        {
            var pieceList = Pieces.OfType<Piece>().ToList();

            foreach (var piece in pieceList)
            {
                if (InputManager.Selected(piece.DestinationRectangle))
                {
                    if(!pieceList.Any(p=>p.State==PieceState.Selected))
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

        public void Draw()
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
                var selectedPiece = pieceList.FirstOrDefault(p=>p.State==PieceState.Selected);

                if (selectedPiece!=null && piece.Id != selectedPiece.Id)
                    piece.UnSelect();
            }
        }
    }
}
