using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Manager
{
    public class Level : IXna
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Level Next { get; set; }

        Image Image { get; set; }

        Board Board { get; set; }

        Nest Nest { get; set; }

        public static int MinRowCount = 3;
        public static int MaxRowCount = Global.ViewportHeight / Piece.MaxHeight;

        public static int MinColumnCount = 3;
        public static int MaxColumnCount = Global.ViewportWidth / Piece.MaxWidth;

        public Level()
        {
            Initialize();
        }

        public void Initialize()
        {
            //Board = new Board(Global.Random.Next(MinRowCount,MaxRowCount),Global.Random.Next(MinColumnCount,MaxColumnCount));

            Board = new Board(8,8);

            Image = new Image("Textures/shutterstock_360399314");
        }


        public void Load()
        {
            Board.LoadContent();

            Image.LoadContent();

            //Image.SetRowAndColumnCount(Global.Random.Next(2, Board.ColumnCount - 1));
            //Image.SetRowAndColumnCount(5);
            Image.SetRowAndColumnCount(2);

            //spreadedimage preparing
            var spreadedImagePieces = General.PopulateListRandomlyFromAnother<Piece>(Board.Pieces.OfType<Piece>().Where(p => !p.IsEmpty).ToList(), Image.Pieces.Count);

            for (int i = 0; i < Image.Pieces.Count; i++)
            {
                var imagePiece = Image.Pieces[i];

                spreadedImagePieces[i].Id = imagePiece.Id;

                spreadedImagePieces[i].MakeImage(imagePiece.ImageNumber, imagePiece.Texture);
            }

            Nest = new Nest(Board,Image);



            ////spreading part
            //var spreadedImagePieces = General.PopulateListRandomlyFromAnother<Piece>(Board.Pieces.OfType<Piece>().ToList(), Image.RowCount * Image.ColumnCount);

            //for (int i = 0; i < Image.Pieces.Count; i++)
            //{
            //    spreadedImagePieces[i].SetBackgroundTexture(Image.Pieces[i].Texture);
            //}
            
            
        }

        public void Update()
        {
            Nest.Update();

            Board.Update();

            //Image.Update();

            if (InputManager.IsKeyPress(Microsoft.Xna.Framework.Input.Keys.R))
                Load();
        }

        public void Draw()
        {
            Nest.Draw();

            Board.Draw();
            //Image.Draw();
        }

        public void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
