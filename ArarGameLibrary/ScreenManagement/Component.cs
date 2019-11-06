﻿using ArarGameLibrary.Effect;
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
    //public interface IComponent :  IDrawableObject
    //{
    //    Guid Id { get; set; }

    //    bool IsDragable { get; set; }

    //    IScreen Screen { get; set; }

    //    void OnClick(Action action);

    //    IComponent Parent { get; set; }

    //    List<IComponent> Child { get; set; }

    //    Component SetParent(IComponent parent);

    //    Component AddChild(params IComponent[] child);

    //    List<T> GetChildAs<T>(Func<T, bool> predicate = null, bool fetchAllDescandents = true) where T : IComponent;

    //    List<T> GetParentAs<T>(Func<T, bool> predicate = null) where T : IComponent;

    //    IComponent SetMargin(Vector2 margin);

    //    IComponent SetPadding(Vector2 padding);

    //    void Align(Vector2 offset, Rectangle? parentRect = null);

    //    void SetPosition(Vector2 position);
    //}


    public abstract class Component : Sprite, IDrawableObject
    {
        public IScreen Screen { get; set; }

        public Component Parent { get; set; }

        public Action ClickAction;

        public List<Component> Child { get; set; }

        public Frame Frame { get; set; }

        public Vector2 DistanceToParent { get; set; }

        public Vector2 SizeDifferenceWithParent { get; set; }

        public Font Font { get; private set; }

        public bool IsFixedToParentPosition { get; private set; }

        public bool IsFixedToParentSize { get; private set; }

        public int ChildrenOrderNumberOnXAxis { get; private set; }
        public int ChildrenOrderNumberOnYAxis { get; private set; }

        public override void IncreaseLayerDepth(float? additionalDepth = null, float? baseDepth = null)
        {
            baseDepth = baseDepth ?? Parent.LayerDepth;

            base.IncreaseLayerDepth(additionalDepth, baseDepth);

            if (Frame != null)
            {
                Frame.IncreaseLayerDepth(baseDepth: LayerDepth);
            }

            if (Font != null)
            {
                Font.IncreaseLayerDepth(baseDepth: LayerDepth);
            }

            foreach (var children in Child)
            {
                (children as Sprite).IncreaseLayerDepth(baseDepth: LayerDepth);
            }
        }

        public void OnClick(Action action)
        {
            ClickAction = action;
        }

        public override void Initialize()
        {
            Child = new List<Component>();

            SetDrawMethodType(5);

            FixToParentPosition();

            FixToParentSize();

            SetClickable(true);

            OnChangeRectangle += Component_OnChangeRectangle;

            base.Initialize();
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

                if (Font != null)
                    Font.Update();

                foreach (var children in Child)
                {
                    children.Update(gameTime);
                }

                CalculateChildOrderNumbers();

                if (IsFixedToParentPosition)
                {
                    if (Parent != null)
                    {
                        var newDistanceToParent = Parent.Position - Position;

                        if (newDistanceToParent != DistanceToParent)
                            SetPosition(Parent.Position - DistanceToParent);

                        //if ((Parent as Sprite).IsPulsating)
                        //{
                        //    GetEvent<PulsateEffect>().SetWhenToInvoke((Parent as Sprite).GetEvent<PulsateEffect>().WhenToInvoke);
                        //}
                    }
                }

                if (IsFixedToParentSize)
                {
                    if (Parent != null)
                    {
                        var newSizeDifferenceWithParent = CalculateSizeDifferenceWithParent();

                        if (newSizeDifferenceWithParent != SizeDifferenceWithParent)
                        {
                            var sizeXRatio = Parent.Size.X * SizeDifferenceWithParent.X;
                            var sizeYRatio = Parent.Size.Y * SizeDifferenceWithParent.Y;

                            SetSize(new Vector2(sizeXRatio, sizeYRatio));

                            var parentChild = Parent.Child;

                            if (parentChild.Any(c => (c.ChildrenOrderNumberOnYAxis < ChildrenOrderNumberOnYAxis))) 
                            {
                                var anotherChildren = parentChild.FirstOrDefault(c=>c.ChildrenOrderNumberOnYAxis<ChildrenOrderNumberOnYAxis);

                                //SetPosition(,);
                            }
                        }
                    }
                }


                //SetDistanceToParent();

                if (Parent!=null && Parent.IsDragable)
                    SetDragable(false);
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            if (IsActive && IsVisible)
            {
                if (Frame != null)
                    Frame.Draw();

                if (Font != null)
                    Font.Draw();

                foreach (var children in Child)
                {
                    children.Draw(spriteBatch);
                }
            }
        }

        public override void Align(Vector2 offset, Rectangle? parentRect = null)
        {
            parentRect = parentRect ?? (Parent != null ? Parent.DestinationRectangle : (Rectangle?)null);

            base.Align(offset, parentRect);

            SetDistanceToParent();
        }

        public new Component SetMargin(Vector2 margin)
        {
            Margin = margin;

            Align(Margin, Parent != null ? Parent.DestinationRectangle : (Rectangle?)null);

            if (Parent != null)
            {
                SetSize(new Vector2(Parent.Size.X - margin.X * 2, Parent.Size.Y - margin.X * 2));
            }

            return this;
        }

        public new Component SetPadding(Vector2 padding)
        {
            Padding = padding;

            foreach (Component children in Child)
            {
                children.Align(Padding, DestinationRectangle);

                children.SetSize(new Vector2(Size.X - padding.X * 2, Size.Y - padding.X * 2));
            }

            return this;
        }



        public Component SetParent(Component parent)
        {
            Parent = parent;

            IncreaseLayerDepth();

            if (IsFixedToParentPosition)
            {
                SetPosition(parent.Position);
            }

            if(IsFixedToParentSize)
                SetSizeDifferenceWithParent();

            return this;
        }

        public Component AddChild(params Component[] child)
        {
            Child.AddRange(child);

            child.ToList().ForEach(c => c.SetParent(this));

            return this;
        }



        void Component_OnChangeRectangle()
        {
            if (Frame != null)
            {
                if (Frame.DestinationRectangle != DestinationRectangle)
                {
                    Frame = Frame.Create(DestinationRectangle, Frame.LinesColor, Frame.LinesThickness);

                    Frame.LoadContent();
                }
            }

            if (Font != null)
            {
                if (Padding == Vector2.Zero)
                    Font.CalculateCenterVector2(DestinationRectangle);
                else
                    Font.SetPosition(new Vector2(Position.X + Padding.X, Position.Y + Padding.Y));

                Font.SetScale(Scale);
            }

            SetDistanceToParent();

            //SetSizeDifferenceWithParent();

            foreach (Component children in Child)
            {
                if (children.IsFixedToParentPosition)
                {
                    children.SetPosition(Position - children.DistanceToParent);
                }

                if (children.IsFixedToParentSize)
                {
                    var sizeX = Size.X * children.SizeDifferenceWithParent.X;
                    var sizeY = Size.Y * children.SizeDifferenceWithParent.Y;

                    var sizeDifferenceY = Size.Y - sizeY;

                    children.SetSize(new Vector2(sizeX, sizeY));

                    if (Child.Any(c => (c.ChildrenOrderNumberOnYAxis < children.ChildrenOrderNumberOnYAxis)))
                    {
                        var anotherChildren = Child.FirstOrDefault(c => c.ChildrenOrderNumberOnYAxis < children.ChildrenOrderNumberOnYAxis);

                        anotherChildren.SetPosition(new Vector2(anotherChildren.Position.X, anotherChildren.Position.Y + sizeDifferenceY));
                    }
                }
            }
        }

        public void SetDistanceToParent()
        {
            if (Parent != null)
                DistanceToParent = Parent.Position - Position;

            //foreach (Component children in Child)
            //{
            //    if(children.IsFixedToParentPosition)
            //        children.SetPosition(children.Parent.Position - children.DistanceToParent);

            //    children.SetDistanceToParent();
            //}
        }

        public void SetSizeDifferenceWithParent()
        {
            SizeDifferenceWithParent = CalculateSizeDifferenceWithParent();

            //foreach (Component children in Child)
            //{
            //    children.SetSizeDifferenceWithParent();
            //}
        }

        private Vector2 CalculateSizeDifferenceWithParent()
        {
            if (Parent != null)
            {
                var sizeXRatio = Parent.Size.X != 0 && Size.X != 0 ? Size.X / Parent.Size.X : 1;

                var sizeYRatio = Parent.Size.Y != 0 && Size.Y != 0 ? Size.Y / Parent.Size.Y : 1;

                return new Vector2(sizeXRatio, sizeYRatio);
            }

            return SizeDifferenceWithParent;
        }

        public Component FixToParentPosition(bool enable = true)
        {
            IsFixedToParentPosition = enable;

            return this;
        }

        public Component FixToParentSize(bool enable = true)
        {
            IsFixedToParentSize = enable;

            return this;
        }

        public override void SetVisible(bool enable)
        {
            IsVisible = enable;

            foreach (var children in Child)
            {
                children.SetVisible(enable);
            }
        }

        public Component MakeFrameVisible(bool enable = true)
        {
            if (Frame != null)
            {
                if (enable)
                    Frame.SetVisible(enable);
                else
                    SetFrame();
            }

            return this;
        }

        public Component SetFrame(Color? lineColor = null, float thickness = 1f, bool makeFrameVisible = true)
        {
            //if (DestinationRectangle.IsEmpty)
            //    throw new Exception("Prepare 'Position' and 'Size' properties before you set 'Frame'");

            lineColor = lineColor ?? Color.Black;

            Frame = Frame.Create(DestinationRectangle, lineColor.Value, thickness);

            Frame.LoadContent();

            MakeFrameVisible(makeFrameVisible);

            return this;
        }

        public Component SetFont(string text, Color? textColor, Vector2? textPadding = null)
        {
            textColor = textColor ?? Color.White;

            textPadding = textPadding ?? Vector2.Zero;

            Font = new Font(text: text, color: textColor);

            Font.IncreaseLayerDepth(baseDepth: LayerDepth);

            SetPadding(textPadding ?? Vector2.Zero);

            var newSize = new Vector2(Font.TextMeasure.X + 2 * Padding.X, Font.TextMeasure.Y + 2 * Padding.Y);

            newSize = new Vector2(MathHelper.Clamp(newSize.X, Size.X, newSize.X), MathHelper.Clamp(newSize.Y, Size.Y, newSize.Y));

            SetSize(newSize);

            return this;
        }

        public Component SetScreen(IScreen screen)
        {
            Screen = screen;

            return this;
        }

        public List<T> GetChildAs<T>(Func<T, bool> predicate = null, bool fetchAllDescandents = true) where T : Component
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

            if (!fetchAllDescandents)
                list.RemoveAll(l => l.Parent.Id != this.Id);

            return list.ToList();
        }

        public List<T> GetParentAs<T>(Func<T, bool> predicate = null) where T : Component
        {
            var list = new List<T>();

            while (Parent != null)
            {
                if (Parent is T)
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

        private void SetChildrenOrderNumberOnXAxis(int childrenOrderNumberOnXAxis)
        {
            ChildrenOrderNumberOnXAxis = childrenOrderNumberOnXAxis;
        }

        private void SetChildrenOrderNumberOnYAxis(int childrenOrderNumberOnYAxis)
        {
            ChildrenOrderNumberOnYAxis = childrenOrderNumberOnYAxis;
        }

        private void CalculateChildOrderNumbers()
        {
            var counter = 1;
            foreach (Component children in Child.OrderByDescending(c => c.Position.X))
            {
                children.SetChildrenOrderNumberOnXAxis(counter++);
            }

            counter = 1;
            foreach (Component children in Child.OrderByDescending(c => c.Position.Y))
            {
                children.SetChildrenOrderNumberOnYAxis(counter++);
            }
        }

    }
}
