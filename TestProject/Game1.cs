using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using ArarGameLibrary.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using TestProject.Screens;

namespace TestProject
{
    public class Game1 : Game
    {
        //52 133 111 255
        //Column column;

        ScrollBar scrollBar;

        Container cnt1001;

        byte opacity = 0;
        Texture2D texture;

        Container cnt101;
        Container cnt102;
   
        public Game1()
        {
            Global.Game(this);
        }

        protected override void Initialize()
        {
            Global.Initialize(this);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            cnt101 = new Container();
            cnt101.SetTexture();
            cnt101.SetSize(new Vector2(100,100));
            cnt101.TestInfo.Font.SetChangeTextEvent(() => cnt101.LayerDepth.ToString());
            cnt101.TestInfo.Show();


            cnt102 = new Container();
            cnt102.TestInfo.Font.SetChangeTextEvent(() => cnt102.LayerDepth.ToString());
            cnt102.SetTexture();
            cnt102.SetSize(new Vector2(100, 100));
            cnt102.SetPosition(new Vector2(250,250));
            cnt102.TestInfo.Show();

            cnt1001 = new Container();
            cnt1001.SetTexture();
            cnt1001.SetPosition(new Vector2(0,0));
            cnt1001.SetSize(new Vector2(Global.ViewportWidth,Global.ViewportHeight));
            cnt1001.TestInfo.Show("Hellow");

            for (int i = 0; i < 4; i++)
            {
                var r = new Row();
                r.SetTexture();
                r.TestInfo.Font.SetChangeTextEvent(()=>r.LayerDepth.ToString());
                r.TestInfo.Show();
                cnt1001.AddRow(r,25);
            }

            cnt1001.PrepareRows();

            var cl = new Column();
            cl.SetName("cl");
            cl.SetTexture();
            cl.SetSize(new Vector2(20,20));
            cl.SetPosition(new Vector2(250, 0));
            cl.TestInfo.Font.SetChangeTextEvent(()=>cl.LayerDepth.ToString());
            cl.TestInfo.Show();
            cnt1001.Rows.FirstOrDefault().AddColumn(cl,70);

            //column = new Column();
            //column.SetTexture();
            //column.SetSize(new Vector2(100, 100));
            ////column.SetSizeDifferenceRatioWithParent(new Vector2(80,80));

            //var container = new Container();
            //container.SetTexture();
            //container.SetSize(new Vector2(25, 25));
            //column.AddChild(container);

            //column.AlignChildAsCenter();

            //column.SetDragable();

            scrollBar = SetScrollBar();
            scrollBar.SetActive(false);

            var menuScreen = new MainMenu();

            var startingScreen = new TradeMarkScreen101();
            startingScreen.SetNextScreen(menuScreen);

            ScreenManager.Add(startingScreen);

            ScreenManager.SetActive(true);

            texture = Global.Content().Load<Texture2D>("smilemanLogoFull");
        }

        public ScrollBar SetScrollBar()
        {
            //column = new Column();

            Column[] columns = new Column[125];


            for (int i = 0; i < columns.Length; i++)
            {
                var container = new Container();

                var row = new Row();
                row.SetTexture();
                container.AddRow(row, 70);

                var row2 = new Row();
                row2.SetTexture();
                container.AddRow(row2, 30);

                var row2Column1 = new Column();
                row2Column1.SetTexture(Color.Black);
                row2.AddColumn(row2Column1, 30);
                row2Column1.AddImage(Triangle.PlayButton(Color.Yellow).Texture);

                var row2Column2 = new Column();
                row2Column2.SetTexture();
                row2Column2.SetFont("Level : " + i);
                row2.AddColumn(row2Column2, 70);

                container.PrepareRows();
                row2Column1.SetPadding(Offset.CreatePadding(OffsetValueType.Ratio, 10, 10, 10, 10));

                var column1 = new Column();
                column1.AddChild(container);

                columns[i] = column1;

                column1.SetFrame(Color.Yellow);
            }


            var scrollBar = new ScrollBar(rowsCountToShow: 3, rowPadding: Offset.CreatePadding(OffsetValueType.Ratio, 5, 5, 5, 5), columns: columns);

            scrollBar.SetActive(true);

            return scrollBar;
        }


        protected override void UnloadContent()
        {


        }


        protected override void Update(GameTime gameTime)
        {
            Global.Update(gameTime);

            //if (InputManager.IsLeftClicked)
            //    column.SetPosition(InputManager.CursorPosition);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (opacity < 255)
                opacity++;
            //column.Update();

            scrollBar.Update();


            cnt1001.Update();

            cnt101.Update();
            cnt102.Update();



            //if (InputManager.IsKeyDown(Keys.Up))
            //    column.SetSize(new Vector2(column.Size.X, column.Size.Y - 20));

            //if (InputManager.IsKeyDown(Keys.Down))
            //    column.SetSize(new Vector2(column.Size.X, column.Size.Y + 20));


            //if (InputManager.IsKeyDown(Keys.Left))
            //    column.SetSize(new Vector2(column.Size.X - 20, column.Size.Y));

            //if (InputManager.IsKeyDown(Keys.Right))
            //    column.SetSize(new Vector2(column.Size.X + 20, column.Size.Y));

            //if (InputManager.IsKeyDown(Keys.W))
            //    column.SetPosition(new Vector2(column.Position.X, column.Position.Y - 20));
            //if (InputManager.IsKeyDown(Keys.S))
            //    column.SetPosition(new Vector2(column.Position.X, column.Position.Y + 20));

            //if (InputManager.IsKeyDown(Keys.A))
            //    column.SetPosition(new Vector2(column.Position.X - 20, column.Position.Y));
            //if (InputManager.IsKeyDown(Keys.D))
            //    column.SetPosition(new Vector2(column.Position.X + 20, column.Position.Y));


            if (InputManager.IsKeyDown(Keys.Up))
            {
                Column item = cnt1001.GetChildAs<Column>(c=>c.Name=="cl").FirstOrDefault();

                item.SetPosition(new Vector2(item.Position.X, item.Position.Y - 10));

                cnt101.SetPosition(new Vector2( cnt101.Position.X, cnt101.Position.Y-10));
            }

            if (InputManager.IsKeyDown(Keys.Down))
            {
                Column item = cnt1001.GetChildAs<Column>(c => c.Name == "cl").FirstOrDefault();

                item.SetPosition(new Vector2(item.Position.X, item.Position.Y + 10));

                cnt101.SetPosition(new Vector2( cnt101.Position.X, cnt101.Position.Y + 10));
            }

            if (InputManager.IsKeyDown(Keys.Left))
            {
                cnt101.SetPosition(new Vector2(cnt101.Position.X -10, cnt101.Position.Y));
            }

            if (InputManager.IsKeyDown(Keys.Right))
            {
                cnt101.SetPosition(new Vector2(cnt101.Position.X + 10, cnt101.Position.Y));
            }


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Global.Theme.GetColor());

            //BlendState leri falan bir yerden çağır,intro için nonpremultiplied gerekiyor
            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            //column.Draw();

            scrollBar.Draw();
            //cnt1001.Draw();




            //cnt101.SetLayerDepth(cnt102.LayerDepth + 0.1f);

            //Global.SpriteBatch.Draw(cnt102.Texture, cnt102.DestinationRectangle, cnt102.SourceRectangle, cnt102.Color, cnt102.Rotation, cnt102.Origin, cnt102.SpriteEffects, cnt102.LayerDepth);

            //Global.SpriteBatch.Draw(cnt101.Texture, cnt101.DestinationRectangle, cnt101.SourceRectangle, cnt101.Color, cnt101.Rotation, cnt101.Origin, cnt101.SpriteEffects, cnt101.LayerDepth);

            

            //Global.SpriteBatch.End();


            //Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            ////Global.SpriteBatch.Draw(cnt102.Texture, cnt102.DestinationRectangle, cnt102.SourceRectangle, cnt102.Color, cnt102.Rotation, cnt102.Origin, cnt102.SpriteEffects, cnt102.LayerDepth);
            Global.SpriteBatch.End();

            ScreenManager.Draw();



            //Font.Draw("", new Vector2(0, 250), Color.White,
            //    () =>
            //    {
            //        return TimeManager.FPS.ToString();
            //    });
            //ScreenManager.Draw();

            //Color.Lerp(Color.Black, Color.Transparent, 0.9f)


            base.Draw(gameTime);

        }
    }
}
