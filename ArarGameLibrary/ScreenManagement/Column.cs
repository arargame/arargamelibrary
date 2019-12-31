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

        public Column AddImage(Texture2D texture,Vector2? padding = null)
        {
            Image = new Column();

            Image.LoadContent(texture);

            AddChild(Image);

            Image.SetPosition(Position);

            //if (padding != null)
             //   Image.SetSize(new Vector2(Size.X - padding.Value.X * 2, Size.Y - padding.Value.Y * 2));
            //else
            Image.SetSize(new Vector2(50,50));

            Image.SetFrame();

            Image.SetName("ColumnImage");

            return this;
        }

        //public override void SetStartingSize()
        //{
        //    SetSize(new Vector2(200, 200));
        //}


    }
}
