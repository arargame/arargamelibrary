using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArarGameLibrary.ScreenManagement
{
    public class ScrollBar : Container
    {
        public Column ItemToScroll { get; set; }

        public int RowsCountToShow { get; set; }

        public int MaxRowsCount { get; set; }

        public ScrollBar(int maxRowsCount,int rowsCountToShow = 3)
        {
            MaxRowsCount = maxRowsCount;

            RowsCountToShow = rowsCountToShow;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(Texture2D texture = null)
        {
            //62 62 66 255
            for (int i = 0; i < MaxRowsCount; i++)
            {
                var row = new Row();
                row.SetFrame(Color.Black, 2f);
                row.MakeFrameVisible(true);

                var heightRatio = (float)1 / MaxRowsCount * 100;

                AddRow(row, i, heightRatio);
            }

            ItemToScroll = new Column();
            ItemToScroll.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(104, 104, 104, 255)));
            Rows.FirstOrDefault().AddColumn(ItemToScroll, 100);
            ItemToScroll.SetLayerDepth(Rows.FirstOrDefault().LayerDepth+0.0001f);
            ItemToScroll.SetDragable(true);

            base.LoadContent(texture);
        }

        public override void Update(GameTime gameTime = null)
        {
            if (MaxRowsCount < RowsCountToShow)
                SetVisible(false);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);
        }
    }
}
