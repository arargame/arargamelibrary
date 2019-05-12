using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class DrawableObject : Sprite
    {
        public DrawableObject(Texture2D texture, Vector2 position, Vector2 size)
        {
            SetTexture(texture);

            SetPosition(position);

            SetSize(size);
        }
    }
}
