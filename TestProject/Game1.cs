using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestProject
{
    public class Game1 : Game
    {
        //52 133 111 255

        Image image;

        Texture2D damageTexture;

        DrawableObject sprite;

        Piece piece;

        TestInfo ti;

        public Game1()
        {
            Content.RootDirectory = "Content";

            Global.Graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Global.Content = Content;
            Global.GameWindow = Window;
            Global.GraphicsDevice = GraphicsDevice;
            Global.Random = new Random();
            Global.SpriteBatch = new SpriteBatch(GraphicsDevice);

            InputManager.IsMouseVisible = IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite = new DrawableObject(Content.Load<Texture2D>("grsl1"),
                                            new Vector2(200, 50),
                                            new Vector2(120, 120));

            piece = new Piece(100,100)
                .SetPosition(new Vector2(250,250))
                .SetSize(new Vector2(150,150))
                .SetBackgroundTextureByRandomColor();


            piece.LoadContent();
            ti = new TestInfo(piece).AddParameters("DestinationRectangle");

            damageTexture = TextureManager.CreateDamageTexture(sprite.Texture);

            image = new Image("wrokcubeBackground");
            image.LoadContent();
            image.SetPosition(Global.ViewportCenter-new Vector2(50,50));
            image.SetSize(new Vector2(200,200));
            image.SetRowAndColumnCount(1,1);
            image.SetPieceSize(new Vector2(150,150));
            image.SetPiecePosition(Vector2.Zero);

            image.Pieces.ForEach(p => p.TestInfo.Show(true).AddParameters("DestinationRectangle"));

            image.Pieces.ForEach(p=>p.SetDrawMethodType(5));

            //image.Pieces.ForEach(p => p.SetLayerDepth(2));

        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            Global.GameTime = gameTime;
            InputManager.Update();
            InputManager.IsActive = true;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //sprite.Update();

            //piece.Update();

            if (InputManager.Selected(piece.DestinationRectangle))
            {
                //if (piece.State == PieceState.UnSelected)
                //{
                //    piece.OnSelecting();
                //}
                //else
                //{
                //    piece.OnDeselecting();
                //}
            }

            foreach (var item in image.Pieces)
            {
                if (InputManager.Selected(item.DestinationRectangle))
                {
                    if (item.State == PieceState.UnSelected)
                    {
                        item.OnSelecting();
                    }
                    else
                    {
                        item.OnDeselecting();
                    }
                }
            }

            ti.Update();

            image.Update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //sprite.Draw();

            //piece.Draw();

            //ti.Draw();

            //Global.SpriteBatch.Draw(damageTexture, new Vector2(0, 0), Color.White);

            image.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
