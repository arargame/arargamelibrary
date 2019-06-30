using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Screens
{
    public class EpisodeCard : Component
    {
        public EpisodeCard()
        {
            var sizeX1 = Global.ViewportWidth * 90 / 100;

            var sizeX2 = Global.ViewportWidth * 10 / 100;

            var scale = new Vector2(150 * Global.Scale.X,150 * Global.Scale.Y / Global.ViewportHeight);

            SetSize(new Vector2(Global.ViewportWidth, Global.ViewportHeight));

            SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.SeaShell));

            var leftSideContainer = new Container();
            //leftSideContainer.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.SlateBlue));
            leftSideContainer.SetSize(new Vector2(sizeX1, Global.ViewportHeight));

            var row1 = new Row();
            row1.MakeFrameVisible(true);
          //  row1.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row1.SetFrame(Color.Black);

            var column1 = new Column();
            column1.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Red, 1, 1));

            var column2 = new Column();
            column2.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Green, 1, 1));

            var column3 = new Column();
            column3.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Blue, 1, 1));

            row1.AddColumn(column1, 25);
            row1.AddColumn(column2, 25);
            row1.AddColumn(column3, 25);


            leftSideContainer.AddRow(row1, 1, 33);


            var row2 = new Row();
            row2.MakeFrameVisible(true);
            row2.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row2.SetFrame(Color.Black);

            leftSideContainer.AddRow(row2, 2, 33);

            var row3 = new Row();
            row3.MakeFrameVisible(true);
            row3.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            row3.SetFrame(Color.Black);

            leftSideContainer.AddRow(row3, 1, 33);

            leftSideContainer.PrepareRows(true);
           

            var rightSideContainer = new Container();
            rightSideContainer.SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Tan));
            rightSideContainer.SetPosition(new Vector2(leftSideContainer.Position.X + leftSideContainer.Size.X, leftSideContainer.Position.Y));
            rightSideContainer.SetSize(new Vector2(sizeX2, Global.ViewportHeight));



            //container.SetSize(new Vector2(200, 150));
            ////container.SetColor(Color.MonoGameOrange);
            //container.SetTexture(TextureManager.CreateTexture2DByRandomColor(1,1));
            //container.SetMargin(new Vector2(10, 10));

            AddChild(leftSideContainer);
            AddChild(rightSideContainer);

            //rightSideContainer.SetVisible(false);


        }
    }


    public class EpisodesMenu : Screen
    {
        EpisodeCard Card { get; set; }

        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Episodes");
        }

        public override bool Load()
        {
            Card = new EpisodeCard();

            return true;
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update(gameTime);

            Card.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            Card.Draw(spriteBatch);
        }
    }
}
