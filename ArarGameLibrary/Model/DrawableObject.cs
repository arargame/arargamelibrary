using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArarGameLibrary.Model
{
    public enum OffsetType
    {
        Margin,
        Padding
    }

    public enum OffsetValueType
    {
        Piksel,
        Ratio
    }

    public struct Offset
    {
        public OffsetType OffsetType { get; set; }

        public OffsetValueType OffsetValueType { get; set; }

        public float Left { get; set; }

        public float Right { get; set; }

        public float Top { get; set; }

        public float Bottom { get; set; }

        public Offset(OffsetType offsetType, OffsetValueType offsetValueType, float left, float top, float right, float bottom)
        {
            OffsetType = offsetType;
            OffsetValueType = offsetValueType;
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static Offset CreatePadding(OffsetValueType offsetValueType, float left, float top, float right, float bottom)
        {
            return new Offset(OffsetType.Padding, offsetValueType, left, top, right, bottom);
        }

        public static Offset CreateMargin(OffsetValueType offsetValueType, float left, float top, float right, float bottom)
        {
            return new Offset(OffsetType.Margin, offsetValueType, left, top, right, bottom);
        }

        public bool IsZero
        {
            get
            {
                return Left == 0f && Right == 0f && Top == 0f && Bottom == 0f;
            }
        }

        public static Offset Zero(OffsetType offsetType = OffsetType.Padding, OffsetValueType offsetValueType = OffsetValueType.Ratio)
        {
            return new Offset(offsetType, offsetValueType, 0, 0, 0, 0);
        }
    }

    public interface IDrawableObject : IXna
    {
        BlendState BlendState { get; set; }

        Rectangle CollisionRectangle { get; set; }
        Color Color { get; set; }

        DepthStencilState DepthStencilState { get; set; }
        Rectangle DestinationRectangle { get; }
        int DrawMethodType { get; set; }

        Microsoft.Xna.Framework.Graphics.Effect Effect { get; set; }

        bool IsActive { get; set; }
        bool IsAlive { get; set; }
        bool IsVisible { get; set; }

        float LayerDepth { get; set; }

        Offset Margin { get; set; }

        Vector2 Origin { get; set; }

        Offset Padding { get; set; }
        //Vector2 Padding { get; set; }
        Vector2 Position { get; set; }

        RasterizerState RasterizerState { get; set; }
        float Rotation { get; set; }

        SamplerState SamplerState { get; set; }
        void SetDrawMethodType(int methodType);
        void SetSpriteEffects(SpriteEffects effects);
        void SetVisible(bool enable);
        float Scale { get; set; }
        Vector2 Size { get; set; }
        Rectangle SourceRectangle { get; set; }
        Vector2 Speed { get; set; }
        SpriteEffects SpriteEffects { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        SpriteSortMode SpriteSortMode { get; set; }

        Texture2D Texture { get; set; }
        Matrix? TransformMatrix { get; set; }
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
