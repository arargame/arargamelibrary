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
        public Container List
        {
            get
            {
                return GetChildAs<Container>(c => c.Name == "ListContainer").FirstOrDefault();
            }
        }

        public Container ScrollContainer
        {
            get
            {
                return GetChildAs<Container>(c => c.Name == "ScrollContainer").FirstOrDefault();
            }
        }

        public Column ItemToScroll { get; set; }

        public int RowsCountToShow { get; set; }

        public int MaxRowsCount { get; set; }

        public float ScrollContainerWidthRatio { get; set; }

        public ScrollBar(int maxRowsCount, int rowsCountToShow = 3, Vector2? position = null, Vector2? size = null, float scrollContainerWidthRatio = 2.5f)
        {
            SetPosition(position ?? Vector2.Zero);

            SetSize(size ?? new Vector2(Global.ViewportWidth, Global.ViewportHeight));

            MaxRowsCount = maxRowsCount;

            RowsCountToShow = rowsCountToShow;

            ScrollContainerWidthRatio = scrollContainerWidthRatio;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(Texture2D texture = null)
        {
            base.LoadContent(texture);

            var listContainer = new Container();
            listContainer.SetName("ListContainer");
            listContainer.SetFrame(Color.Blue);
            listContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            AddChild(listContainer);

            var scrollContainer = new Container();
            scrollContainer.SetName("ScrollContainer");
            scrollContainer.SetFrame(Color.Black);
            scrollContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            AddChild(scrollContainer);
            //62 62 66 255
            //for (int i = 0; i < MaxRowsCount; i++)
            //{
            //    var row = new Row();
            //    row.SetFrame(Color.Black, 2f);
            //    row.MakeFrameVisible(true);

            //    var heightRatio = (float)1 / MaxRowsCount * 100;

            //    AddRow(row, i, heightRatio);
            //}

            //ItemToScroll = new Column();
            //ItemToScroll.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(104, 104, 104, 255)));
            //Rows.FirstOrDefault().AddColumn(ItemToScroll, 100);
            //ItemToScroll.SetLayerDepth(Rows.FirstOrDefault().LayerDepth + 0.0001f);
            //ItemToScroll.SetDragable(true);

            OnChangeRectangle += ScrollBar_OnChangeRectangle;
        }

        private void ScrollBar_OnChangeRectangle()
        {
            //var coX = new ClampObject("Position.X", Position.X, Position.X);

            //if (!ItemToScroll.ClampManager.ContainsKey("Position.X"))
            //    ItemToScroll.ClampManager.Add(coX);
            //else
            //    ItemToScroll.ClampManager.RefreshClampObject(coX.PropertyName, coX.Min, coX.Max);

            //var coY = new ClampObject("Position.Y", Position.Y, Position.Y + Size.Y - ItemToScroll.Size.Y);

            //if (!ItemToScroll.ClampManager.ContainsKey("Position.Y"))
            //    ItemToScroll.ClampManager.Add(coY);
            //else
            //    ItemToScroll.ClampManager.RefreshClampObject(coY.PropertyName, coY.Min, coY.Max);

            var scrollContainerSizeX = (float)Size.X * ScrollContainerWidthRatio / 100;

            List.SetPosition(Position);
            List.SetSize(new Vector2(Size.X - scrollContainerSizeX,Size.Y));

            ScrollContainer.SetPosition(new Vector2(Position.X + Size.X - scrollContainerSizeX, Position.Y));
            ScrollContainer.SetSize(new Vector2(scrollContainerSizeX, Size.Y));
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

        public ScrollBar PrepareList(int rowsCountToShow = 3, int columnsCountPerRow = 3, params Column[] columns)
        {
            //var list = GetChildAs<Container>(c => c.Id == List.Id);

            //List.SetPosition(new Vector2(0, 0));
            //List.SetSize(new Vector2(Global.ViewportWidth, Global.ViewportHeight));
            //List.SetFrame(Color.Black);
            //List.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            var rowRatio = (float)100 / rowsCountToShow;
            var columnRatio = (float)100 / columnsCountPerRow;

            for (int i = 0; i < rowsCountToShow; i++)
            {
                if (columns == null)
                    continue;

                var columnsToAdd = columns.Skip(columnsCountPerRow * i)
                                    .Take(columnsCountPerRow)
                                    .ToList();

                var row = new Row();
                columnsToAdd.ForEach(c => row.AddColumn(c, columnRatio));

                //row.SetFrame(Color.White);
                //row.SetTexture(TextureManager.CreateTexture2DByRandomColor());

                List.AddRow(row, i, rowRatio);

                var x = columnsToAdd.Count!= 0 ? columnsToAdd.FirstOrDefault().LayerDepth : 0f;
                var y = row.LayerDepth;
            }

            List.PrepareRows(false,"left");

            return this;
        }
    }
}
