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
    //public class NestPiece
    //{
    //    public int RowCount { get; set; }

    //    public int ColumnCount { get; set; }

    //    public int Number { get; set; }
    //}

    public class Nest : IXna
    {
        //public List<NestPiece> NestPieces { get; set; }

        public List<Piece> Pieces { get; set; }

        public Board Board { get; set; }


        public Nest(Board board, Image image)
        {
            //NestPieces = new List<NestPiece>();


            Pieces = new List<Piece>();

            Board = board;

            SetPosition(board,image);


            //foreach (var piece in Pieces)
            //{
            //    var backgroundTexture = image.Pieces.FirstOrDefault(ip => ip.ImageNumber == piece.ImageNumber).Texture;

            //    piece.MakeNest(backgroundTexture, Color.White, TextureManager.CreateTexture2DBySingleColor(Color.Magenta, (int)piece.Size.X, (int)piece.Size.Y), Color.White);
            //}

        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Texture2D texture = null)
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime = null)
        {
            foreach (var piece in Pieces)
            {
                piece.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch = null)
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

                isCalculated = board.Pieces.OfType<Piece>().Any(p => p.RowNumber == rowNumber && p.ColumnNumber == columnNumber);

                if (isCalculated)
                {
                    //var upperTopPieceOfNest = board.Pieces.OfType<Piece>().FirstOrDefault(predicate);

                    for (int i = rowNumber,counter = 0; i < rowNumber + image.RowCount; i++)
                    {
                        for (int k = columnNumber; k < columnNumber + image.ColumnCount; k++,counter++)
                        {

                            //NestPieces.Add(new NestPiece()
                            //{
                            //    Number = counter,
                            //    RowCount = board.GetPiece(i,k).RowNumber,
                            //    ColumnCount = board.GetPiece(i,k).ColumnNumber
                            //});


                            var piece = Piece.CreateACopy(board.GetPiece(i,k));

                            piece.SetImageNumber(counter);

                            Pieces.Add(piece);
                        }
                    }

                }

            } while (!isCalculated);


            foreach (var piece in Pieces)
            {
                if (image.Pieces.Any(ip => ip.ImageNumber == piece.ImageNumber))
                {
                    var img = image.Pieces.FirstOrDefault(ip => ip.ImageNumber == piece.ImageNumber);

                    piece.MakeNest(img.Texture,Color.White);
                }
            }

        }

        public bool IsSettled()
        {
            //var counter = NestPieces.Count;

            //foreach (var boardPiece in Board.Pieces)
            //{
            //    if (NestPieces.Any(p => boardPiece.ImageNumber == p.Number))
            //        counter--;
            //}

            //return counter == 0;

            return true;
        }


    }
}
