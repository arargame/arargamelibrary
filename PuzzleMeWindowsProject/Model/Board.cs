using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public class Board : IXna
    {
        public Vector2 PieceSize { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

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

            for (int i = 0; i < RowCount; i++)
            {
                for (int k = 0; k < ColumnCount; k++)
                {
                    var color = new Color(Global.Random.Next(0, 255), Global.Random.Next(0, 255), Global.Random.Next(0, 255));

                    Pieces[i, k] = new Piece(new Vector2(PieceSize.X * k, PieceSize.Y * i), PieceSize, color)
                                        .SetRowAndColumnNumber(i,k)
                                        .UnSelect()
                                        .SetBoard(this);
                }
            }
        }

        public void LoadContent()
        {
            var counter = 0;

            foreach (var piece in Pieces)
            {
                piece.LoadContent();

                piece.SetNumber(counter++);

                piece.FontManager.SetText(string.Format("({0},{1})={2}",piece.RowNumber,piece.ColumnNumber,piece.Number));
            }

            var emptyPieceNumber = Global.Random.Next(0, counter + 1);

            var emptyPiece = Pieces.OfType<Piece>().FirstOrDefault(p=>p.Number==emptyPieceNumber);

            emptyPiece.MakeEmpty();
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
        }

        public void Draw()
        {
            foreach (var piece in Pieces)
            {
                piece.Draw();
            }
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


        public void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
