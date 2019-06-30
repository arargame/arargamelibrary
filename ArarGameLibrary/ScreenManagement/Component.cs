using ArarGameLibrary.Extension;
using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public interface IComponent : IXna, IDrawableObject
    {
        IScreen Screen { get; set; }

        void OnClick(Action action);

        Component SetParent(IComponent parent);

        Component AddChild(params IComponent[] child);
    }

    public abstract class Component : Sprite , IComponent
    {
        public IScreen Screen { get; set; }

        public IComponent Parent { get; set; }

        public Action ClickAction;

        public Vector2 Margin { get; private set; }

        public Vector2 Padding { get; private set; }

        public List<IComponent> Child { get; set; }

        public Frame Frame { get; set; }

        public bool IsFrameVisible { get; private set; }

        //public bool IsStretching { get; private set; }


        public void OnClick(Action action)
        {
            ClickAction = action;
        }

        public override void Initialize()
        {
            Child = new List<IComponent>();

            SetDrawMethodType(5);

            base.Initialize();

            SetClickable(true);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (IsSelecting)
            {
                if (ClickAction != null)
                    ClickAction.Invoke();
            }

            if (IsFrameVisible && Frame!=null)
                Frame.Update();

            foreach (var children in Child)
            {
                children.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            if (IsFrameVisible && Frame!=null)
                Frame.Draw();

            foreach (var children in Child)
            {
                children.Draw(spriteBatch);
            }
        }

        public Component SetScreen(IScreen screen)
        {
            Screen = screen;

            return this;
        }

        public Component SetMargin(Vector2 margin)
        {
            Margin = margin;

            Align(Margin, Parent != null ? Parent.DestinationRectangle : (Rectangle?)null);

            return this;
        }

        public Component SetPadding(Vector2 padding)
        {
            Padding = padding;
            
            return this;
        }

        public Component MakeFrameVisible(bool enable)
        {
            IsFrameVisible = enable;

            return this;
        }

        public Component SetFrame(Color lineColor)
        {
            Frame = Frame.Create(DestinationRectangle, lineColor);

            Frame.LoadContent();

            return this;
        }

        //public Component Stretch(bool enable)
        //{
        //    IsStretching = enable;

        //    var size = Parent != null ? Parent.Size : Global.ViewportRect.Size();
        //    var position = Parent != null ? Parent.Position + new Vector2(10,10) : Global.ViewportRect.Position();
            
        //    SetPosition(position);
        //    SetSize(size);

        //    if (Parent != null)
        //        SetLayerDepth(Parent.LayerDepth + 0.01f);

        //    if(Frame!=null)
        //        SetFrame(Frame.LineColor);

        //    return this;
        //}

        public Component SetParent(IComponent parent)
        {
            Parent = parent;

            return this;
        }

        public Component AddChild(params IComponent[] child)
        {
            Child.AddRange(child);

            child.ToList().ForEach(c=>c.SetParent(this));

            return this;
        }

        public List<T> GetChildAs<T>() where T : IComponent
        {
            return Child.OfType<T>().ToList();
        }
    }
}
