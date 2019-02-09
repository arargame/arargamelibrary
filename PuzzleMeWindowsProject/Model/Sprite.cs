using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public interface IDrawableObject
    {
        Texture2D Texture { get; set; }

        Vector2 Position { get; set; }

        Vector2 Size { get; set; }

        void Initialize();

        void LoadContent();

        void UnloadContent();

        void Update();

        void Draw();
    }

    public abstract class Sprite : IBaseObject,IDrawableObject
    {
        #region Properties

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

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
            Id = Guid.NewGuid();

            CreateDate = UpdateDate = DateTime.Now;

            Position = Size = Vector2.Zero;
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent() { }

        public virtual void Update() { }

        public virtual void Draw() { }

        public Sprite SetName(string name)
        {
            Name = name;

            return this;
        }

        public Sprite SetDescription(string description)
        {
            Description = description;

            return this;
        }

        public Sprite SetPosition(Vector2 position)
        {
            Position = position;

            return this;
        }

        public Sprite SetSize(Vector2 size)
        {
            Size = size;

            return this;
        }

        #endregion
    }
}
