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
        //class ImageColumn : Component { }

        public float WidthRatio { get; private set; }

        public Row Row
        {
            get
            {
                return Parent as Row;
            }
        }

        Column Image { get; set; }

        public Column()
        {

        }

        public Column SetWidthRatio(float widthRatio)
        {
            WidthRatio = MathHelper.Clamp(widthRatio, 0, 100);

            return this;
        }

        //vektorel cizim yapılabilr
        public Column AddImage(Texture2D texture)
        {
            Image = new Column();

            Image.LoadContent(texture);

            AddChild(Image);

            Image.SetPosition(Position + new Vector2(10));
            //Image.SetSize(new Vector2(Size.X - Size.X * 0.2f, Size.Y - Size.Y * 0.2f));
            Image.SetSize(new Vector2(Size.X - 20, Size.Y - 20));
            //Image.SetFrame();

            return this;
        }
    }
}
