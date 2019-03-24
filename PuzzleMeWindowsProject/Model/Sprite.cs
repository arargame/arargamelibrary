using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public interface IBaseObject
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        DateTime CreateDate { get; set; }

        DateTime UpdateDate { get; set; }
    }

    public abstract class BaseObject : IBaseObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public BaseObject()
        {
            Id = Guid.NewGuid();

            CreateDate = UpdateDate = DateTime.Now;
        }
    }

    public interface IXna
    {
        void Initialize();

        void LoadContent();

        void UnloadContent();

        void Update();

        void Draw();
    }

    public interface IDrawableObject : IXna
    {
        Texture2D Texture { get; set; }

        Vector2 Position { get; set; }

        Vector2 Size { get; set; }

        Vector2 Origin { get; set; }

        Vector2 Speed { get; set; }

        Rectangle DestinationRectangle { get; set; }

        Rectangle SourceRectangle { get; set; }

        Color Color { get; set; }

        bool IsAlive { get; set; }
    }

    

    public abstract class Sprite : BaseObject,IDrawableObject
    {
        #region Properties

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public Vector2 Speed { get; set; }

        public Vector2 Origin { get; set; }

        public Rectangle DestinationRectangle { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public Color Color { get; set; }

        public bool IsAlive { get; set; }

        #endregion

        #region Constructor 

        public Sprite()
        {
            Initialize();
        }

        #endregion

        #region Functions

        public virtual void Initialize()
        {
            IsAlive = true;

            SetStartingPosition();

            SetStartingSpeed();

            SetStartingSize();

            SetOrigin();

            DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            SourceRectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);

            Color = Color.White;
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update() 
        {
            if (IsAlive)
            {
                SetRectangle();

                SetOrigin();
            }
        }

        public virtual void Draw() 
        {
            if(IsAlive)
                Global.SpriteBatch.Draw(Texture,DestinationRectangle,Color);
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetTexture(string name)
        {
            Texture = Global.Content.Load<Texture2D>(name);
        }

        public virtual void SetStartingPosition()
        {
            SetPosition(Vector2.Zero);
        }

        public virtual void SetStartingSpeed()
        {
            SetSpeed(Vector2.Zero);
        }

        public virtual void SetStartingSize()
        {
            SetSize(Vector2.Zero);
        }

        private void SetRectangle()
        {
            DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            //SourceRectangle = new Rectangle(animation.FrameBounds.X, animation.FrameBounds.Y, (int)Size.X, (int)Size.Y);
        }

        private void SetOrigin()
        {
            Origin = Texture != null ? new Vector2(Texture.Width / 2, Texture.Height / 2) : Vector2.Zero;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void SetSize(Vector2 size)
        {
            Size = size;
        }

        public void SetSpeed(Vector2 speed)
        {
            Speed = speed;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        #endregion
    }
}
