using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArarGameLibrary.Effect;

namespace ArarGameLibrary.ScreenManagement
{
    public class ScrollBar : Container
    {
        public Container ListContainer
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

        public Column Bar 
        {
            get
            {
                var columns = GetChildAs<Column>(c => c.Name == "Bar");

                return columns.FirstOrDefault();
            }
        }

        public int RowsCountToShow { get; set; }

        public int MaxRowsCount { get; set; }

        public int ColumnsCountPerRow { get; set; }

        public float ScrollContainerWidthRatio { get; set; }

        public ScrollBar(int rowsCountToShow = 4, int columnsCountPerRow = 3, float scrollContainerWidthRatio = 2.5f, params Column[] columns)
        {
            OnChangeRectangle += ScrollBar_OnChangeRectangle;

            MaxRowsCount = columns.Length != 0 ? columns.Length : rowsCountToShow;

            RowsCountToShow = rowsCountToShow;

            ColumnsCountPerRow = columnsCountPerRow;

            ScrollContainerWidthRatio = scrollContainerWidthRatio;

            LoadListContainer();

            LoadScrollContainer();

            var rowRatio = (float)100 / RowsCountToShow;
            var columnRatio = (float)100 / ColumnsCountPerRow;

            for (int i = 0; i < RowsCountToShow; i++)
            {
                if (columns.Length == 0)
                    continue;

                var columnsToAdd = columns.Skip(ColumnsCountPerRow * i)
                                    .Take(ColumnsCountPerRow)
                                    .ToList();

                var row = new Row();
                columnsToAdd.ForEach(c => row.AddColumn(c, columnRatio));
                ListContainer.AddRow(row, i, rowRatio);
            }
        }


        public override void Initialize()
        {
            base.Initialize();

            SetPosition(Vector2.Zero);

            SetSize(new Vector2(Global.ViewportWidth, Global.ViewportHeight));

            Effects.Add(new BarScrollingEffect(this));
        }


        public override void LoadContent(Texture2D texture = null)
        {
            base.LoadContent(texture);
        }

        private void LoadListContainer()
        {
            var listContainer = new Container();
            listContainer.SetName("ListContainer");
            listContainer.SetFrame(Color.Blue);
            listContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            AddChild(listContainer);
        }

        private void LoadScrollContainer()
        {
            var scrollContainer = new Container();
            scrollContainer.SetName("ScrollContainer");
            scrollContainer.SetFrame(Color.Black);
            scrollContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            AddChild(scrollContainer);
            //62 62 66 255
            for (int i = 0; i < MaxRowsCount; i++)
            {
                var row = new Row();
                row.SetFrame(Color.Black, 2f);
                row.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(62,62,66,255)));
                row.MakeFrameVisible(true);

                var heightRatio = (float)1 / MaxRowsCount * 100;

                scrollContainer.AddRow(row, i, heightRatio);
            }

            var scrollContainerRows = scrollContainer.GetChildAs<Row>();

            var itemToScroll = new Column();
            itemToScroll.SetName("Bar");
            itemToScroll.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(104, 104, 104, 255)));
            scrollContainerRows.FirstOrDefault().AddColumn(itemToScroll, 100);
            //itemToScroll.SetLayerDepth(Rows.FirstOrDefault().LayerDepth + 0.0001f);
            itemToScroll.SetDragable(true);
        }

        private void ScrollBar_OnChangeRectangle()
        {
            var scrollContainerSizeX = (float)Size.X * ScrollContainerWidthRatio / 100;

            ListContainer.SetPosition(Position);
            ListContainer.SetSize(new Vector2(Size.X - scrollContainerSizeX, Size.Y));

            ScrollContainer.SetPosition(new Vector2(Position.X + Size.X - scrollContainerSizeX, Position.Y));
            ScrollContainer.SetSize(new Vector2(scrollContainerSizeX, Size.Y));

            ListContainer.PrepareRows(floatTo: "left");
            ScrollContainer.PrepareRows(isCentralized: true);

            if (Bar != null)
            {
                var coX = new ClampObject("Position.X", ScrollContainer.Position.X, ScrollContainer.Position.X);

                if (!Bar.ClampManager.ContainsKey("Position.X"))
                    Bar.ClampManager.Add(coX);
                else
                    Bar.ClampManager.RefreshClampObject(coX.PropertyName, coX.Min, coX.Max);

                var coY = new ClampObject("Position.Y", ScrollContainer.Position.Y, ScrollContainer.Position.Y + ScrollContainer.Size.Y - Bar.Size.Y);

                if (!Bar.ClampManager.ContainsKey("Position.Y"))
                    Bar.ClampManager.Add(coY);
                else
                    Bar.ClampManager.RefreshClampObject(coY.PropertyName, coY.Min, coY.Max);
            }
        }

        //under the construction 
        public override void Update(GameTime gameTime = null)
        {
            if (MaxRowsCount < RowsCountToShow)
                ScrollContainer.SetVisible(false);

            base.Update(gameTime);

            if (Bar.IsDragging)
            {
                var counter = 1;

                var maxOverlapped = ScrollContainer.Rows.Select(row => new
                {
                    Number = counter++,
                    OverlappedAreaHeight = Rectangle.Intersect(Bar.DestinationRectangle, row.DestinationRectangle).Height
                })
                .OrderByDescending(o => o.OverlappedAreaHeight)
                .FirstOrDefault();

                var barScrollingEffect = GetEffect<BarScrollingEffect>();

                if (barScrollingEffect.PageNumber != maxOverlapped.Number)
                    barScrollingEffect.Start();
                else
                    barScrollingEffect.End();

            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);
        }

        public ScrollBar SetListContainer2(params Column[] columns)
        {
            //var list = GetChildAs<Container>(c => c.Id == List.Id);

            //List.SetPosition(new Vector2(0, 0));
            //List.SetSize(new Vector2(Global.ViewportWidth, Global.ViewportHeight));
            //List.SetFrame(Color.Black);
            //List.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            //MaxRowsCount = columns.Length;

            //var rowRatio = (float)100 / RowsCountToShow;
            //var columnRatio = (float)100 / ColumnsCountPerRow;

            //for (int i = 0; i < RowsCountToShow; i++)
            //{
            //    if (columns == null)
            //        continue;

            //    var columnsToAdd = columns.Skip(ColumnsCountPerRow * i)
            //                        .Take(ColumnsCountPerRow)
            //                        .ToList();

            //    var row = new Row();
            //    columnsToAdd.ForEach(c => row.AddColumn(c, columnRatio));
            //    ListContainer.AddRow(row, i, rowRatio);
            //}

            return this;
        }
    }
}
