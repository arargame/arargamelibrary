using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Manager
{
    public class GameLevel : Level
    {
        Image Image { get; set; }

        Board Board { get; set; }

        Nest Nest { get; set; }

        public static int MinRowCount = 3;
        public static int MaxRowCount = Global.ViewportHeight / Piece.MaxHeight;

        public static int MinColumnCount = 3;
        public static int MaxColumnCount = Global.ViewportWidth / Piece.MaxWidth;

        public GameLevel()
        {
            Initialize();
        }

        public override void Initialize()
        {
            //Board = new Board(Global.Random.Next(MinRowCount,MaxRowCount),Global.Random.Next(MinColumnCount,MaxColumnCount));

            Board = new Board(8,8);

            Image = new Image("Textures/shutterstock_360399314");
        }


        public override void LoadContent(Texture2D texture = null)
        {
            Board.LoadContent();

            Image.LoadContent();

            Image.SetRowAndColumnCount(2);

            //spreadedimage preparing
            var spreadedImagePieces = General.PopulateListRandomlyFromAnother<Piece>(Board.Pieces.Where(p => !p.IsEmpty).ToList(), Image.Pieces.Count);

            for (int i = 0; i < Image.Pieces.Count; i++)
            {
                var imagePiece = Image.Pieces[i];

                spreadedImagePieces[i].Id = imagePiece.Id;

                spreadedImagePieces[i].MakeImage(imagePiece.ImageNumber, imagePiece.Texture);
            }

            Nest = new Nest(Board,Image);
        }

        public override void Update(GameTime gameTime = null)
        {
            //Nest.Update();

            Board.Update();

            if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R))
                LoadContent();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            //Nest.Draw();

            Board.Draw();
        }


        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
