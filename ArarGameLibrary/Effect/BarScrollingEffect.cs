using ArarGameLibrary.Model;
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
            SetTask(() => 
            {
            
            });

            SetEndTask(() => 
            {
            
            });
        }

        public BarScrollingEffect SetPageNumber(int pageNumber)
        {
            PageNumber = pageNumber;

            return this;
        }
    }
}
