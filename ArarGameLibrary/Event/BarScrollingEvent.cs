using ArarGameLibrary.Manager;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Event
{
    public class BarScrollingEvent : EventManager
    {
        private int PageNumber { get; set; }

        private ScrollBar ScrollBar { get; set; }

        public BarScrollingEvent(ScrollBar scrollBar)
            : base(scrollBar, isContinuous: true)
        {
            ScrollBar = Sprite as ScrollBar;

            SetTask(() => 
            {
                if (ScrollBar.Bar.IsDragging || (ScrollBar.Bar.IsDragable && InputManager.IsMouseScrolling))
                {
                    var counter = 0;

                    var maxOverlapAmongBarAndBlock = ScrollBar.ScrollContainer.Rows.Select(row =>
                        new
                        {
                            BlockNumber = counter++,
                            OverlappedAreaHeight = Rectangle.Intersect(ScrollBar.Bar.DestinationRectangle, row.DestinationRectangle).Height
                        })
                    .OrderByDescending(o => o.OverlappedAreaHeight)
                    .FirstOrDefault();


                    if (PageNumber != maxOverlapAmongBarAndBlock.BlockNumber)
                    {
                        PageNumber = maxOverlapAmongBarAndBlock.BlockNumber;

                        ScrollBar.LoadListContainer(PageNumber);
                    }
                }
                else
                {
                    var rows = ScrollBar.ListContainer.Rows;

                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i].SetVisible(true);
                    }
                }

            });
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
