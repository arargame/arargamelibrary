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
using ArarGameLibrary.Event;

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

            Events.Add(new MouseScrollEvent(sprite: this,
                whenScrollStateIsUp: () => 
                {
                    var mouseScrollValue = (Bar.Size.Y / 4);

                    Bar.SetPosition(new Vector2(Bar.Position.X, MathHelper.Clamp(Bar.Position.Y - mouseScrollValue, ScrollContainer.Position.Y, ScrollContainer.Position.Y + ScrollContainer.Size.Y - Bar.Size.Y)));
                },
                whenScrollStateIsDown: () => 
                {
                    var mouseScrollValue = (Bar.Size.Y / 4);

                    Bar.SetPosition(new Vector2(Bar.Position.X, MathHelper.Clamp(Bar.Position.Y + mouseScrollValue, ScrollContainer.Position.Y, ScrollContainer.Position.Y + ScrollContainer.Size.Y - Bar.Size.Y)));
                },
                whenScrollStateIsIdle: null));
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
                    //row.SetFrame(Color.White);
                    //row.SetTexture(TextureManager.CreateTexture2DByRandomColor());
                    columnsToAdd.ForEach(c => row.AddColumn(c, columnRatio));
                    columnsToAdd.ForEach(c => c.SetTexture(TextureManager.CreateTexture2DByRandomColor()));
                    ListContainer.AddRow(row,rowRatio);
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


            var scrollContainerSizeX = (float)Size.X * ScrollContainerWidthRatio / 100;
            ListContainer.SetPosition(Position);
            ListContainer.SetSize(new Vector2(Size.X - scrollContainerSizeX, Size.Y));

            RefreshRectangle();
        }

        private void LoadScrollContainer()
        {
            var scrollContainerSizeX = (float)Size.X * ScrollContainerWidthRatio / 100;

            var scrollContainer = new Container();
            scrollContainer.SetName("ScrollContainer");
            scrollContainer.SetFrame(Color.Black);
            scrollContainer.SetTexture(TextureManager.CreateTexture2DByRandomColor());
            scrollContainer.SetPosition(new Vector2(Position.X + Size.X - scrollContainerSizeX, Position.Y));
            scrollContainer.SetSize(new Vector2(scrollContainerSizeX, Size.Y));

            AddChild(scrollContainer);

            for (int i = 0; i < MaxRowPageCount; i++)
            {
                var row = new Row();
                //row.SetFrame(Color.Black, 2f);
                row.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(62,62,66,255)));

                var heightRatio = (float)1 / MaxRowPageCount * 100;

                scrollContainer.AddRow(row,heightRatio);
            }

            var bar = new Column();
            bar.SetName("Bar");
            bar.SetTexture(TextureManager.CreateTexture2DBySingleColor(new Color(104, 104, 104, 255)));
            bar.SetDragable(true);

            var scrollContainerRows = scrollContainer.GetChildAs<Row>();
            scrollContainerRows.FirstOrDefault().AddColumn(bar, 70);

            scrollContainer.PrepareRows(isCentralized: true);

            RefreshRectangle();
        }

        private void ScrollBar_OnChangeRectangle()
        {
            if (ListContainer!=null)
            {
                ListContainer.PrepareRows(floatTo:"left");
            }

            if (ScrollContainer != null)
            {
                var barStartingPosition = ScrollContainer.Size.X * Bar.WidthRatio / 100;

                var coX = new ClampObject("Position.X", Bar.Position.X, Bar.Position.X);

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
            if (MaxRowsCount <= RowsCountToShow)
                Bar.SetVisible(false);
            else
                Bar.SetVisible(true);

            base.Update(gameTime);

            var mouseScrollValue = (Bar.Size.Y / 4);

            //if (InputManager.IsMouseScrollUp)
            //{
            //    Bar.SetPosition(new Vector2(Bar.Position.X, MathHelper.Clamp(Bar.Position.Y - mouseScrollValue, ScrollContainer.Position.Y, ScrollContainer.Position.Y + ScrollContainer.Size.Y - Bar.Size.Y)));
            //}
            //else if (InputManager.IsMouseScrollDown)
            //{
            //    Bar.SetPosition(new Vector2(Bar.Position.X, MathHelper.Clamp(Bar.Position.Y + mouseScrollValue, ScrollContainer.Position.Y, ScrollContainer.Position.Y + ScrollContainer.Size.Y - Bar.Size.Y)));
            //}

            var barScrollingEffect = GetEffect<BarScrollingEffect>();

            if (Bar.IsDragging || InputManager.IsMouseScrolling)
            {
                var counter = 0;
                //var counter2 = 0;

                //var xxx = ScrollContainer.Rows.Select(row => new
                //{
                //    BlockNumber = counter2++,
                //    OverlappedAreaHeight = Rectangle.Intersect(Bar.DestinationRectangle, row.DestinationRectangle).Height
                //}).ToList();

                var maxOverlapAmongBarAndBlock = ScrollContainer.Rows.Select(row => new
                {
                    BlockNumber = counter++,
                    OverlappedAreaHeight = Rectangle.Intersect(Bar.DestinationRectangle, row.DestinationRectangle).Height
                })
                .OrderByDescending(o => o.OverlappedAreaHeight)
                .FirstOrDefault();

               // LoadListContainer(maxOverlapAmongBarAndBlock.BlockNumber);

                if (barScrollingEffect.PageNumber != maxOverlapAmongBarAndBlock.BlockNumber)
                {
                    barScrollingEffect.SetPageNumber(maxOverlapAmongBarAndBlock.BlockNumber);
                    barScrollingEffect.Start();
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
