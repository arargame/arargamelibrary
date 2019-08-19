using ArarGameLibrary.Model;
using ArarGameLibrary.ScreenManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Effect
{
    public class BarScrollingEffect : EffectManager
    {
        public int PageNumber { get; set; }

        public BarScrollingEffect(Sprite sprite)
            : base(sprite)
        {
            var scrollBar = sprite as ScrollBar;


            SetTask(() => 
            {
                scrollBar.LoadListContainer(PageNumber);

                var rows = scrollBar.ListContainer.Rows;

                var counter = 0;
                var collection = rows.Select(r => new
                {
                    Page = counter++,
                    Row = r
                }).ToList();


                //for (int i = 0; i < scrollBar.MaxRowPageCount; i++)
                //{
                //    rows.Skip(i * scrollBar.RowsCountToShow)
                //        .Take(scrollBar.RowsCountToShow)
                //        .Select(r => new
                //        {
                //            Key = r,
                //            Value = i
                //        })
                //        .ToDictionary(o => o.Key, o => o.Value)
                //        .ToList()
                //        .ForEach(d => collection.Add(d.Key, d.Value));
                //}

                //for (int i = 0; i < rows.Count; i++)
                //{
                //    if (PageNumber - 1 == i)
                //        rows[i].SetVisible(true);
                //    else
                //        rows[i].SetVisible(false);
                //}

                foreach (var item in collection)
                {
                    var newPosition = new Vector2(0, (PageNumber - 1 - item.Page) * 50);

                   // item.Row.SetPosition(new Vector2(100,100));
                }
            });

            SetEndTask(() => 
            {
                var rows = scrollBar.ListContainer.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    rows[i].SetVisible(true);
                }
            });
        }

        public BarScrollingEffect SetPageNumber(int pageNumber)
        {
            PageNumber = pageNumber;

            return this;
        }
    }
}
