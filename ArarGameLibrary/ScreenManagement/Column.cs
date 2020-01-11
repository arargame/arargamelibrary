using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public class Column : Component
    {
        public float WidthRatio { get; private set; }

        public Row Row
        {
            get
            {
                return Parent as Row;
            }
        }

        public Column()
        {

        }

        public Column SetWidthRatio(float widthRatio)
        {
            WidthRatio = MathHelper.Clamp(widthRatio, 0, 100);

            return this;
        }

        public Column AddImage(Texture2D texture)
        {
            var imageColumn = new Column();

            imageColumn.LoadContent(texture);

            imageColumn.SetPosition(Position);

            imageColumn.SetSizeRatioToParent(new Vector2(100, 100));

            AddChild(imageColumn);

            return this;
        }
    }
}
