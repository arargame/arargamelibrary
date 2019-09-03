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
        int DrawMethodType { get; set; }

        bool IsActive { get; set; }
        bool IsAlive { get; set; }
        bool IsVisible { get; set; }

        float LayerDepth { get; set; }

        Vector2 Origin { get; set; }

        Vector2 Padding { get; set; }
        Vector2 Position { get; set; }

        float Rotation { get; set; }

        void SetDrawMethodType(int methodType);
        void SetSpriteEffects(SpriteEffects effects);
        void SetVisible(bool enable);
        float Scale { get; set; }
        Vector2 Size { get; set; }
        Rectangle SourceRectangle { get; set; }
        Vector2 Speed { get; set; }
        SpriteEffects SpriteEffects { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        Texture2D Texture { get; set; }
    }

    //public class DrawableObject : BaseObject, IDrawableObject
    //{
    //    public Rectangle CollisionRectangle { get; set; }
    //    public Color Color { get; set; }

    //    public Rectangle DestinationRectangle { get; set; }
    //    public int DrawMethodType { get; set; }

    //    public bool IsActive { get; set; }
    //    public bool IsAlive { get; set; }
    //    public bool IsVisible { get; set; }

    //    public float LayerDepth { get; set; }

    //    public Vector2 Origin { get; set; }

    //    public Vector2 Padding { get; set; }
    //    public Vector2 Position { get; set; }

    //    public float Rotation { get; set; }

    //    public float Scale { get; set; }
    //    public Vector2 Size { get; set; }
    //    public Rectangle SourceRectangle { get; set; }
    //    public Vector2 Speed { get; set; }
    //    public SpriteEffects SpriteEffects { get; private set; }
    //    public SpriteBatch SpriteBatch { get; set; }

    //    public Texture2D Texture { get; set; }


    //    public virtual void Draw(SpriteBatch spriteBatch = null)
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public virtual void Initialize()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public virtual void LoadContent(Texture2D texture = null)
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public virtual void UnloadContent()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public virtual void Update(GameTime gameTime = null)
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public virtual void SetColor(Color color)
    //    {
    //        Color = color;
    //    }

    //    public void SetLayerDepth(float layerDepth)
    //    {
    //        LayerDepth = MathHelper.Clamp(layerDepth, 0f, 1f);
    //    }

    //    public void SetDrawMethodType(int methodType)
    //    {
    //        DrawMethodType = methodType;
    //    }

    //    public virtual void SetPosition(Vector2 position)
    //    {
    //        Position = position;
    //    }

    //    public virtual void SetScale(float scale)
    //    {
    //        Scale = scale;
    //    }

    //    public void SetSpriteEffects(SpriteEffects effects)
    //    {
    //        SpriteEffects = effects;
    //    }

    //    public virtual void SetVisible(bool enable)
    //    {
    //        IsVisible = enable;
    //    }
    //}
}
