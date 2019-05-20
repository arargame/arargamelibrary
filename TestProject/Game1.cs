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
            sprite = new DrawableObject(Content.Load<Texture2D>("58669757_1118224611712565_8241604631101177856_o"),
                                            new Vector2(20, 20),
                                            new Vector2(120, 120));

            piece = new Piece(100,100)
                .SetPosition(new Vector2(250,250))
                .SetSize(new Vector2(150,150))
                .SetBackgroundTextureByRandomColor();


            piece.LoadContent();
            ti = new TestInfo(piece).AddParameters("DestinationRectangle");
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


            sprite.Update();

            piece.Update();

            if (InputManager.Selected(piece.DestinationRectangle))
            {
                if (piece.State == PieceState.UnSelected)
                {
                    piece.OnSelecting();
                }
                else
                {
                    piece.OnDeselecting();
                }
                
            }

            ti.Update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            //sprite.Draw();

            piece.Draw();

            ti.Draw();

            Global.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
