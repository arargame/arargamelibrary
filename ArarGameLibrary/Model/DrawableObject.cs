using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public interface IDrawableObject : IXna
    {
        Rectangle CollisionRectangle { get; set; }
        Color Color { get; set; }

        Rectangle DestinationRectangle { get; }

        bool IsActive { get; set; }
        bool IsAlive { get; set; }
        bool IsVisible { get; set; }

        float LayerDepth { get; set; }

        Vector2 Origin { get; set; }

        Vector2 Position { get; set; }

        float Rotation { get; set; }

        float Scale { get; set; }
        Vector2 Size { get; set; }
        Rectangle SourceRectangle { get; set; }
        Vector2 Speed { get; set; }
        SpriteEffects SpriteEffects { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        Texture2D Texture { get; set; }

        void SetVisible(bool enable);
    }

    public class DrawableObject : BaseObject, IDrawableObject
    {
        public Rectangle CollisionRectangle { get; set; }
        public Color Color { get; set; }

        public Rectangle DestinationRectangle { get; set; }

        public bool IsActive { get; set; }
        public bool IsAlive { get; set; }
        public bool IsVisible { get; set; }
        public float LayerDepth { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Vector2 Size { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Speed { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Texture2D Texture { get; set; }


        public virtual void Draw(SpriteBatch spriteBatch = null)
        {
            //throw new NotImplementedException();
        }

        public virtual void Initialize()
        {
            //throw new NotImplementedException();
        }

        public virtual void LoadContent(Texture2D texture = null)
        {
            //throw new NotImplementedException();
        }

        public virtual void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        public virtual void Update(GameTime gameTime = null)
        {
            //throw new NotImplementedException();
        }

        public virtual void SetVisible(bool enable)
        {
            IsVisible = enable;
        }
    }
}
