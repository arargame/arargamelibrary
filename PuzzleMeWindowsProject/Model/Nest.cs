using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public class NestPiece
    {
        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public int Number { get; set; }
    }

    public class Nest : IXna
    {
        public List<NestPiece> NestPieces { get; set; }

        public List<Piece> Pieces { get; set; }

        public Board Board { get; set; }

        //public Image Image { get; set; }

        //public Vector2 Position { get; set; }

        //public int RowCount { get; set; }

        //public int ColumnCount { get; set; }

        public Nest(Board board, Image image)
        {
            NestPieces = new List<NestPiece>();

            Pieces = new List<Piece>();

            Board = board;

            SetPosition(board,image);

            //foreach (var imagePiece in image.Pieces)
            //{
            //    imagePiece.SetSize(board.PieceSize);

            //    imagePiece.SetPosition(new Vector2(image.Position.X + (imagePiece.Size.X * imagePiece.ColumnNumber), image.Position.Y + (imagePiece.Size.Y * imagePiece.RowNumber)));
            //}


            //nested area preparing

            //var nestedBoardPieces = Board.Pieces.OfType<Piece>()
            //                                .Where(bp => NestPieces.Any(p => bp.RowNumber == p.RowCount && bp.ColumnNumber == p.ColumnCount))
            //                                .ToList();

            //foreach (var piece in nestedBoardPieces)
            //{
            //    var imageNumber = NestPieces.FirstOrDefault(p => piece.RowNumber == p.RowCount && p.ColumnCount == piece.ColumnNumber).Number;

            //    var backgroundTexture = image.Pieces.FirstOrDefault(ip => ip.ImageNumber == piece.ImageNumber).Texture;

            //    piece.MakeNest(backgroundTexture, TextureManager.CreateTexture2DBySingleColor(Color.Magenta, (int)piece.Size.X, (int)piece.Size.Y));
            //}


            foreach (var piece in Pieces)
            {
                var backgroundTexture = image.Pieces.FirstOrDefault(ip => ip.ImageNumber == piece.ImageNumber).Texture;

                piece.MakeNest(backgroundTexture, Color.White, TextureManager.CreateTexture2DBySingleColor(Color.Magenta, (int)piece.Size.X, (int)piece.Size.Y), Color.White);
            }

        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            foreach (var piece in Pieces)
            {
                piece.Update();
            }
        }

        public void Draw()
        {
            foreach (var piece in Pieces)
            {
                piece.Draw();
            }
        }

        public void SetPosition(Board board, Image image)
        {
            var isCalculated = false;

            do
            {
                var rowNumber = Global.Random.Next(0, board.RowCount - image.RowCount + 1);

                var columnNumber = Global.Random.Next(0, board.ColumnCount - image.ColumnCount + 1);

                var predicate = new Func<Piece, bool>(p => p.RowNumber == rowNumber && p.ColumnNumber == columnNumber);

                isCalculated = board.Pieces.OfType<Piece>().Any(predicate);

                if (isCalculated)
                {
                    var upperTopPieceOfNest = board.Pieces.OfType<Piece>().FirstOrDefault(predicate);

                    //for (int i = upperTopPieceOfNest.RowNumber; i < Board.Pieces.OfType<Piece>().Count(); i++)
                    //{
                    //    for (int k = 0; k < Board.Pieces.OfType<>; k++)
                    //    {
                              
                    //    }
                    //}

                    for (int i = rowNumber,counter = 0; i < rowNumber + image.RowCount; i++)
                    {
                        for (int k = columnNumber; k < columnNumber + image.ColumnCount; k++,counter++)
                        {

                            NestPieces.Add(new NestPiece()
                            {
                                Number = counter,
                                RowCount = board.GetPiece(i,k).RowNumber,
                                ColumnCount = board.GetPiece(i,k).ColumnNumber
                            });


                            var xx = board.GetPiece(i,k);
                            var piece = Piece.CreateACopy(board.GetPiece(i,k));

                            piece.SetImageNumber(counter);
                            piece.ClearTypes();
                            piece.AddType(PieceType.Nest);


                            Pieces.Add(piece);
                        }
                    }

                    //foreach (var imagePiece in image.Pieces)
                    //{
                    //    imagePiece.SetSize(board.PieceSize);

                    //    imagePiece.SetPosition(new Vector2(upperTopPieceOfNest.Position.X + (imagePiece.Size.X * imagePiece.ColumnNumber), image.Position.Y + (upperTopPieceOfNest.Position.Y * imagePiece.RowNumber)));

                    //    var boardPiece = board.Pieces.OfType<Piece>().FirstOrDefault(p => p.Position.X == imagePiece.Position.X && p.Position.Y == imagePiece.Position.Y);

                    //    imagePiece.SetRowAndColumnNumber(boardPiece.RowNumber,boardPiece.ColumnNumber);

                    //    Pieces.Add(new NestPiece()
                    //    {
                    //        Number = imagePiece.ImageNumber,
                    //        RowCount = imagePiece.RowNumber,
                    //        ColumnCount = imagePiece.ColumnNumber
                    //    });
                    //}

                  //  Position = upperTopPieceOfNest.Position;
                }

            } while (!isCalculated);

            //image.SetPosition(Position);
        }

        public bool IsSettled()
        {
            var counter = NestPieces.Count;

            foreach (var boardPiece in Board.Pieces)
            {
                if (NestPieces.Any(p => boardPiece.ImageNumber == p.Number))
                    counter--;
            }

            return counter == 0;
        }


    }
}
