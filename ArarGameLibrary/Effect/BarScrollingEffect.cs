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
