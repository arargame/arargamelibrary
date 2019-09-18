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
    public interface IComponent :  IDrawableObject
    {
        IScreen Screen { get; set; }

        void OnClick(Action action);

        Component SetParent(IComponent parent);

        Component AddChild(params IComponent[] child);

        List<T> GetChildAs<T>(Func<T, bool> predicate = null) where T : IComponent;

        List<T> GetParentAs<T>(Func<T, bool> predicate = null) where T : IComponent;

        IComponent SetMargin(Vector2 margin);

        IComponent SetPadding(Vector2 padding);

        void Align(Vector2 offset, Rectangle? parentRect = null);
    }


    public abstract class Component : Sprite , IComponent
    {
        public IScreen Screen { get; set; }

        public IComponent Parent { get; set; }

        public Action ClickAction;

        public List<IComponent> Child { get; set; }

        public Frame Frame { get; set; }

        public Vector2 DistanceToParent { get; set; }

        public IComponent SetDistanceToParent()
        {
            if (Parent != null)
            {
                DistanceToParent = Parent.Position - Position;
            }
            else
            {
                DistanceToParent = Vector2.Zero;
            }

            return this;
        }

        public override void IncreaseLayerDepth(float? additionalDepth = null, float? baseDepth = null)
        {
            baseDepth = baseDepth ?? Parent.LayerDepth;

            base.IncreaseLayerDepth(additionalDepth, baseDepth);
        }

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

            OnChangeRectangle += Component_OnChangeRectangle;
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();

            if (IsActive)
            {
                if (IsSelecting)
                {
                    if (ClickAction != null)
                        ClickAction.Invoke();
                }

                if (Frame != null)
                    Frame.Update();

                foreach (var children in Child)
                {
                    children.Update(gameTime);
                }

                if (Parent != null)
                    SetPosition(Parent.Position - DistanceToParent);
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            if (IsActive && IsVisible)
            {
                if (Frame != null)
                    Frame.Draw();

                foreach (var children in Child)
                {
                    children.Draw(spriteBatch);
                }
            }
        }

        public Component SetScreen(IScreen screen)
        {
            Screen = screen;

            return this;
        }

        public override void Align(Vector2 offset, Rectangle? parentRect = null)
        {
            parentRect = parentRect ?? (Parent != null ? Parent.DestinationRectangle : (Rectangle?)null);

            base.Align(offset, parentRect);
        }

        public new IComponent SetMargin(Vector2 margin)
        {
            Margin = margin;

            Align(Margin,Parent != null ? Parent.DestinationRectangle : (Rectangle?)null);

            return this;
        }

        public new IComponent SetPadding(Vector2 padding)
        {
            Padding = padding;

            foreach (var children in Child)
            {
                children.Align(Padding, DestinationRectangle);
            }

            return this;
        }

        public Component MakeFrameVisible(bool enable)
        {
            if (Frame != null)
                Frame.SetVisible(enable);

            return this;
        }

        public Component SetFrame(Color lineColor, float thickness = 1f,bool makeFrameVisible = true)
        {
            //if (DestinationRectangle.IsEmpty)
            //    throw new Exception("Prepare 'Position' and 'Size' properties before you set 'Frame'");

            Frame = Frame.Create(DestinationRectangle, lineColor, thickness);

            Frame.LoadContent();

            MakeFrameVisible(makeFrameVisible);

            return this;
        }

        public Component SetParent(IComponent parent)
        {
            Parent = parent;

            IncreaseLayerDepth();

            return this;
        }

        public override void SetVisible(bool enable)
        {
            base.SetVisible(enable);
        }

        public Component AddChild(params IComponent[] child)
        {
            Child.AddRange(child);

            child.ToList().ForEach(c => c.SetParent(this));

            return this;
        }

        public List<T> GetChildAs<T>(Func<T, bool> predicate = null) where T : IComponent
        {
            var list = new List<T>();

            if (predicate != null)
            {
                list.AddRange(Child.OfType<T>().Where(predicate));
            }
            else
            {
                list.AddRange(Child.OfType<T>());
            }

            foreach (var children in Child)
            {
                list.AddRange(children.GetChildAs<T>(predicate));
            }

            return list.ToList();
        }

        public List<T> GetParentAs<T>(Func<T, bool> predicate = null) where T : IComponent
        {
            var list = new List<T>();

            while(Parent!=null)
            {
                if(Parent is T)
                    list.Add((T)Parent);
                else 
                    list.AddRange(Parent.GetParentAs<T>(predicate));
            }

            if (predicate != null && list.Count > 0)
            {
                return list.Where(predicate).ToList();
            }

            return list.ToList();
        }

        void Component_OnChangeRectangle()
        {
            if (Frame != null)
                if (Frame.DestinationRectangle != DestinationRectangle)
                {
                    Frame = Frame.Create(DestinationRectangle, Frame.LinesColor, Frame.LinesThickness);

                    Frame.LoadContent();
                }
        }
    }
}
