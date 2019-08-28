using ArarGameLibrary.Effect;
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
        const float RightSideRatio = 12.5f;

        const int MaxPieceWidth = 87;
        const int MaxPieceHeight = 60;

        const int MinRowCount = 3;
        const int MinColumnCount = 3;

        Image Image { get; set; }

        Board Board { get; set; }

        Nest Nest { get; set; }

        List<Piece> BoardPiecesOnNest { get; set; }

        public GameLevel()
        {
            Initialize();
        }

        public override void Initialize()
        {
            var navBarWidth = Global.ViewportWidth * RightSideRatio / 100;
            var boardWidth = Global.ViewportWidth - navBarWidth;

            var maxRowCount = (int)Global.ViewportHeight / MaxPieceHeight;
            var maxColumnCount = (int)boardWidth / MaxPieceWidth;

            var boardColumnCount = MathHelper.Clamp(maxColumnCount, MinColumnCount, maxColumnCount);
            var boardRowCount = MathHelper.Clamp(maxRowCount, MinRowCount, maxRowCount);

            Board = new Board(boardRowCount,boardColumnCount, new Vector2(boardWidth, Global.ViewportHeight));

            Image = new Image("Textures/shutterstock_360399314");

            BoardPiecesOnNest = new List<Piece>();

            base.Initialize();
        }


        public override void LoadContent(Texture2D texture = null)
        {
            Image.LoadContent();

            Image.SetRowAndColumnCount(2);

            Board.LoadContent();

            Board.SpreadImagePiecesOnTheBoard(Image);

            Nest = new Nest(Board, Image);

            Nest.Pieces.ForEach(p => p.SetDrawMethodType(1));

            Nest.Pieces.ForEach(p => p.SetColor(Color.Gray));

            //Board.Pieces.Where(p=>!p.IsEmpty).ToList().ForEach(p=>p.SetColor(new Color(p.Color,0.5f)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime = null)
        {
            Nest.Update();

            Board.Update();

            if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R))
                LoadContent();

            foreach (var boardPiece in Board.Pieces)
            {
                foreach (var nestPiece in Nest.Pieces)
                {
                    if (boardPiece.Number == nestPiece.Number)
                    {
                        if (!BoardPiecesOnNest.Any(np => np.Number == boardPiece.Number))
                            BoardPiecesOnNest.Add(boardPiece);
                    }
                }
            }


            var outOfNest = BoardPiecesOnNest.Select(bp => bp.Number).Except(Nest.Pieces.Select(np => np.Number)).FirstOrDefault();

            BoardPiecesOnNest.RemoveAll(np => np.Number == outOfNest);

            var outsidePiecesOfNest = Board.Pieces.Where(bp => !BoardPiecesOnNest.Any(np => np.Number == bp.Number)).ToList();


            foreach (var piece in Board.Pieces)
            {
                var simpleShadowEffect = piece.GetEvent<SimpleShadowEffect>();

                if (outsidePiecesOfNest.Any(ep => ep.Number == piece.Number))
                {
                    piece.SetColor(new Color(piece.Color, 1f));

                    if (simpleShadowEffect != null && !piece.IsEmpty)
                        simpleShadowEffect.Start();
                }
                else
                {
                    piece.SetColor(new Color(piece.Color, 0.75f));

                    if (simpleShadowEffect != null)
                        simpleShadowEffect.End();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            Nest.Draw();

            Board.Draw();

            var imagePieces = Board.Pieces.Where(p => p.IsImage).ToList();



            base.Draw();
        }


        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
