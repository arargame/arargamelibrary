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

        public int MaxRowPageCount
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling((decimal)MaxRowsCount / RowsCountToShow));
            }
        }

        public Column[] Columns { get; set; }

        public ScrollBar(int rowsCountToShow = 4, int columnsCountPerRow = 3, float scrollContainerWidthRatio = 2.5f, params Column[] columns)
        {
            OnChangeRectangle += ScrollBar_OnChangeRectangle;

            MaxRowsCount = columns.Length != 0 ? Convert.ToInt32(Math.Ceiling((decimal)columns.Length / columnsCountPerRow)) : rowsCountToShow;

            RowsCountToShow = rowsCountToShow;

            ColumnsCountPerRow = columnsCountPerRow;

            ScrollContainerWidthRatio = scrollContainerWidthRatio;

            SetColumns(columns);

            LoadListContainer();

            LoadScrollContainer();
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

        public void LoadListContainer(int page = 0)
        {
            if (ListContainer == null)
            {
                var listContainer = new Container();
                listContainer.SetName("ListContainer");
                listContainer.SetFrame(Color.Blue);
                listContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());

                AddChild(listContainer);
            }

            //ListContainer.Rows.ForEach(r => r.SetActive(false));
            //ListContainer.Rows.Clear();

            var rowRatio = (float)100 / RowsCountToShow;
            var columnRatio = (float)100 / ColumnsCountPerRow;

            if (!ListContainer.Rows.Any())
            {
                for (int i = 0; i < MaxRowsCount; i++)
                {
                    if (Columns.Length == 0)
                        continue;

                    var columnsToAdd = Columns.Skip(ColumnsCountPerRow * i)
                                        .Take(ColumnsCountPerRow)
                                        .ToList();

                    var row = new Row();
                    row.SetFrame(Color.White);
                    row.SetTexture(TextureManager.CreateTexture2DByRandomColor());
                    columnsToAdd.ForEach(c => row.AddColumn(c, columnRatio));
                    columnsToAdd.ForEach(c => c.SetTexture(TextureManager.CreateTexture2DByRandomColor()));
                    ListContainer.AddRow(row, i, rowRatio);
                }
            }

            ListContainer.Rows.ForEach(r => r.SetActive(false));

            var collection = ListContainer.Rows.Skip(RowsCountToShow * page)
                                    .Take(RowsCountToShow)
                                    .ToList();

            var counter = 0;
            foreach (var item in collection)
            {
                item.SetActive(true);
                item.TestInfo.Show(true);
                item.TestInfo.Font.SetText(((RowsCountToShow * page) + counter++).ToString());
            }

            RefreshRectangle();

            //var index = 0;
            //var collection = rows.Skip(RowsCountToShow * index)
            //                        .Take(RowsCountToShow)
            //                        .Select(r => new
            //                        {
            //                            Page = index++,
            //                            Rows = r
            //                        }).ToList();
        }

        private void LoadScrollContainer()
        {
            var scrollContainer = new Container();
            scrollContainer.SetName("ScrollContainer");
            scrollContainer.SetFrame(Color.Black);
            scrollContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());

            AddChild(scrollContainer);
            //62 62 66 255
            for (int i = 0; i < MaxRowPageCount; i++)
            {
                var row = new Row();
                row.SetFrame(Color.Black, 2f);
                row.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(62,62,66,255)));
                row.MakeFrameVisible(true);

                var heightRatio = (float)1 / MaxRowPageCount * 100;

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

            if (ListContainer!=null)
            {
                ListContainer.SetPosition(Position);
                ListContainer.SetSize(new Vector2(Size.X - scrollContainerSizeX, Size.Y));
                ListContainer.PrepareRows(floatTo: "left");
            }

            if (ScrollContainer != null)
            {
                ScrollContainer.SetPosition(new Vector2(Position.X + Size.X - scrollContainerSizeX, Position.Y));
                ScrollContainer.SetSize(new Vector2(scrollContainerSizeX, Size.Y));
                ScrollContainer.PrepareRows(isCentralized: true);
            }


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

            var barScrollingEffect = GetEffect<BarScrollingEffect>();

            if (Bar.IsDragging)
            {
                var counter =0;
                var counter2 = 0;

                var xxx = ScrollContainer.Rows.Select(row => new
                {
                    BlockNumber = counter2++,
                    OverlappedAreaHeight = Rectangle.Intersect(Bar.DestinationRectangle, row.DestinationRectangle).Height
                }).ToList();

                var maxOverlapAmongBarAndBlock = ScrollContainer.Rows.Select(row => new
                {
                    BlockNumber = counter++,
                    OverlappedAreaHeight = Rectangle.Intersect(Bar.DestinationRectangle, row.DestinationRectangle).Height
                })
                .OrderByDescending(o => o.OverlappedAreaHeight)
                .FirstOrDefault();

                if (barScrollingEffect.PageNumber != maxOverlapAmongBarAndBlock.BlockNumber)
                {
                    barScrollingEffect.Start();
                    barScrollingEffect.SetPageNumber(maxOverlapAmongBarAndBlock.BlockNumber);
                }
            }
            else
            {
                barScrollingEffect.End();
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);
        }

        public ScrollBar SetColumns(Column[] columns)
        {
            Columns = columns;

            return this;
        }

    }
}
