using ArarGameLibrary.Effect;
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
    public abstract class Component : Sprite, IDrawableObject
    {
        public IScreen Screen { get; set; }

        public Component Parent { get; set; }

        public int ChildrenOrderNumberOnXAxis { get; private set; }
        public int ChildrenOrderNumberOnYAxis { get; private set; }
        public Action ClickAction;

        public Frame Frame { get; set; }

        public Vector2 DistanceToParent { get; set; }

        public Vector2 SizeDifferenceRatioWithParent { get; set; }
        public Vector2 PositionRatio { get; set; }

        public Font Font { get; private set; }

        public bool IsFixedToParentPosition { get; private set; }

        public bool IsFixedToParentSize { get; private set; }

        public List<Component> Child { get; set; }

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

        //SetClickAction
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


                if (Parent != null && Parent.IsDragable)
                    SetDragable(false);
            }
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw(spriteBatch);

            if (IsActive && IsVisible)
            {
                Frame?.Draw();

                Font?.Draw();

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

            //SetDistanceToParent();
        }

        public new Component SetMargin(Offset margin)
        {
            Margin = margin;

            // Component_OnChangeMargin();

            //MarginRatio = CalculateMarginRatio();

            if (Parent != null)
            {
                var newPosition = Vector2.Zero;
                var newSize = Vector2.Zero;

                if (Margin.OffsetValueType == OffsetValueType.Piksel)
                {
                    newPosition = Parent.Position + new Vector2(Margin.Left, Margin.Top);

                    newSize = Size - new Vector2(Margin.Left + Margin.Right, Margin.Top + Margin.Bottom);
                }
                else if (Margin.OffsetValueType == OffsetValueType.Ratio)
                {
                    var sizeX = Parent.Size.X != 0f ? Parent.Size.X : 1;
                    var sizeY = Parent.Size.Y != 0f ? Parent.Size.Y : 1;

                    newPosition = Parent.Position + new Vector2(sizeX * Margin.Left / 100, sizeY * Margin.Top / 100);

                    newSize = new Vector2(Size.X - (Size.X * Margin.Left / 100) - (Size.X * Margin.Right / 100), Size.Y - (Size.Y * Margin.Top / 100) - (Size.Y * Margin.Bottom / 100));

                    //newSize = new Vector2(Margin.Right != 0f ? (Size.X - (Size.X * Margin.Right / 100)) : Size.X, Size.Y);
                }

                if (newPosition != Vector2.Zero)
                    SetPosition(newPosition);

                if (newSize != Vector2.Zero)
                {
                    var sizeXRatio = Size.X != 0 && newSize.X != 0 ? (newSize.X / Size.X) * 100 : 0;

                    var sizeYRatio = Size.Y != 0 && newSize.Y != 0 ? (newSize.Y / Size.Y) * 100 : 0;

                    SetSizeDifferenceRatioWithParent(new Vector2(sizeXRatio, sizeYRatio));

                    SetSize(newSize);
                }
            }

            return this;
        }

        public new Component SetPadding(Offset padding)
        {
            Padding = padding;

            //PaddingRatio = CalculatePaddingRatio();

            // Component_OnChangePadding();

            foreach (var children in Child)
            {
                var childrenNewPosition = Vector2.Zero;
                var childrenNewSize = Vector2.Zero;

                if (Padding.OffsetValueType == OffsetValueType.Piksel)
                {
                    childrenNewPosition = children.Position + new Vector2(Padding.Left, Padding.Top);

                    childrenNewSize = Size - new Vector2(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
                }
                else if (Padding.OffsetValueType == OffsetValueType.Ratio)
                {
                    childrenNewPosition = children.Position + new Vector2(children.Size.X * Padding.Left / 100, children.Size.Y * Padding.Top / 100);

                    childrenNewSize = children.Size - new Vector2((children.Size.X * Padding.Left / 100) + (children.Size.X * Padding.Right / 100), (children.Size.Y * Padding.Top / 100) + (children.Size.Y * Padding.Bottom / 100));
                }

                if (childrenNewPosition != Vector2.Zero)
                    children.SetPosition(childrenNewPosition);

                if (childrenNewSize != Vector2.Zero)
                {
                    var sizeXRatio = Size.X != 0 && childrenNewSize.X != 0 ? (childrenNewSize.X / Size.X) * 100 : 0;

                    var sizeYRatio = Size.Y != 0 && childrenNewSize.Y != 0 ? (childrenNewSize.Y / Size.Y) * 100 : 0;

                    children.SetSizeDifferenceRatioWithParent(new Vector2(sizeXRatio, sizeYRatio));

                    children.SetSize(childrenNewSize);
                }
            }

            return this;
        }

        public Component Prepare()
        {
            CalculateChildOrderNumbers();

            //if (Parent == null)
            //    Align(Margin);

            foreach (Component children in Child)
            {
                //children.SetPosition(children.Position + Padding + children.Margin);

                var childrenPosition = Vector2.Zero;
                var childrenSize = Vector2.Zero;

                if (Padding.OffsetValueType == OffsetValueType.Piksel)
                {
                    childrenPosition = children.Position + new Vector2(Padding.Left, Padding.Top) ;
                    
                    childrenSize = Size - new Vector2(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
                }
                else if (Padding.OffsetValueType == OffsetValueType.Ratio)
                {
                    var sizeX = Size.X != 0f ? Size.X : 1;
                    var sizeY = Size.Y != 0f ? Size.Y : 1;

                    childrenPosition = children.Position + new Vector2( sizeX * Padding.Left / 100, sizeY * Padding.Top / 100) ;
                    
                    childrenSize = Size - new Vector2((Size.X * Padding.Left / 100) + (Size.X * Padding.Right / 100), (Size.Y * Padding.Top / 100) + (Size.Y * Padding.Bottom / 100));
                }

                if(childrenPosition!=Vector2.Zero)
                    children.SetPosition(childrenPosition);

                if (childrenSize != Vector2.Zero)
                {
                    children.SetSize(childrenSize);
                }
            }

            foreach (Component children in Child)
            {
                children.Prepare();
            }

            return this;
        }

        public Component SetParent(Component parent)
        {
            Parent = parent;

            IncreaseLayerDepth();

            if (IsFixedToParentPosition)
            {
               //SetPosition(parent.Position + parent.Padding + Margin);
            }

            //if (IsFixedToParentSize)
            //    SetSizeDifferenceWithParent();

            if (Position == Vector2.Zero)
                SetPosition(Parent.Position);

            SetDistanceToParent();
            SetSizeDifferenceRatioWithParent();

            return this;
        }


        public Component AddChild(params Component[] child)
        {
            Child.AddRange(child);

            if (Size == Vector2.Zero)
                SetSize(new Vector2(100, 100));

            child.ToList().ForEach(c => c.SetParent(this));

            Component_OnChangeRectangle();

            return this;
        }

        //void Component_OnChangeMargin()
        //{
        //    Align(Margin, Parent?.DestinationRectangle);

        //    if (Parent != null)
        //    {
        //        SetSize(new Vector2(Parent.Size.X - Margin.X * 2, Parent.Size.Y - Margin.X * 2));
        //    }

        //    SetSizeDifferenceRatioWithParent();
        //}

        //void Component_OnChangePadding()
        //{
        //    foreach (Component children in Child)
        //    {
        //        children.Align(Padding, DestinationRectangle);

        //        children.SetSize(new Vector2(Size.X - Padding.X * 2, Size.Y - Padding.X * 2));
        //    }
        //}


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
                if (Padding.IsZero)
                    Font.CalculateNewPosition(DestinationRectangle, null, true);
                else
                    Font.CalculateNewPosition(DestinationRectangle, Padding);

                Font.SetScale(Scale);
            }

            SetDistanceToParent();

            SetSizeDifferenceRatioWithParent();

            foreach (Component children in Child)
            {
                if (children.IsFixedToParentPosition)
                {
                    var newDistanceToParent = Position - children.Position;

                    if (newDistanceToParent != children.DistanceToParent)
                    {
                        children.SetPosition(Position - children.DistanceToParent);
                    }
                }

                if (children.IsFixedToParentSize)
                {
                    var newSizeDifferenceWithParent = children.CalculateSizeDifferenceWithParent();

                    children.SetSizeDifferenceRatioWithParent();

                    if (newSizeDifferenceWithParent != children.SizeDifferenceRatioWithParent)
                    {
                        var newChildrenSize = (Size * children.SizeDifferenceRatioWithParent) / 100;

                        children.SetSize(newChildrenSize);

                        Vector2 distanceToParent = children.DistanceToParent;

                        var distanceChangeToParent = Vector2.Zero;

                        if (SizeChangingRatio != null)
                        {
                            if (SizeChangingRatio.Value != Vector2.Zero)
                                distanceChangeToParent = (SizeChangingRatio.Value * children.DistanceToParent) / 100;
                        }

                        var newPosition = Position - (distanceToParent + distanceChangeToParent);

                        if (newPosition != children.Position)
                        {
                            //children.DistanceToParent = Position - newPosition;
                            children.SetPosition(newPosition);
                        }
                    }
                }

                //if (children.IsFixedToParentSize)
                //{
                //    var newSizeDifferenceWithParent = children.CalculateSizeDifferenceWithParent();

                //    children.SetSizeDifferenceRatioWithParent();

                //    if (newSizeDifferenceWithParent != children.SizeDifferenceRatioWithParent)
                //    {
                //        var sizeX = Size.X * (children.SizeDifferenceRatioWithParent.X != 0 ? (children.SizeDifferenceRatioWithParent.X / 100) : 1);
                //        var sizeY = Size.Y * (children.SizeDifferenceRatioWithParent.Y != 0 ? (children.SizeDifferenceRatioWithParent.Y / 100) : 1);

                //        children.SetSize(new Vector2(sizeX, sizeY));

                //        Vector2 distanceToParent = children.DistanceToParent;

                //        if (SizeChangingRatio != null)
                //        {
                //            if (SizeChangingRatio.Value != Vector2.Zero)
                //                distanceToParent = SizeChangingRatio.Value * children.DistanceToParent;  
                //        }

                //        var newPosition = Position - distanceToParent;

                //        if (newPosition != children.Position)
                //            children.SetPosition(newPosition);
                //    }
                //}
            }
        }

        public void SetDistanceToParent()
        {
            if (Parent != null)
            {
                DistanceToParent = Parent.Position - Position;
            }
        }

        public void SetSizeDifferenceRatioWithParent(Vector2? sizeDifferenceWithParent = null)
        {
            if (sizeDifferenceWithParent != null)
                SizeDifferenceRatioWithParent = sizeDifferenceWithParent.Value;
            else
            {
                var newSizeDifferenceWithParent = CalculateSizeDifferenceWithParent();

                if (SizeDifferenceRatioWithParent == Vector2.Zero && newSizeDifferenceWithParent != Vector2.Zero)
                    SizeDifferenceRatioWithParent = newSizeDifferenceWithParent;
            }
        }

        private Vector2 CalculateSizeDifferenceWithParent()
        {
            if (Parent != null)
            {
                var sizeXRatio = Parent.Size.X != 0 && Size.X != 0 ? (Size.X / Parent.Size.X) * 100 : 0;

                var sizeYRatio = Parent.Size.Y != 0 && Size.Y != 0 ? (Size.Y / Parent.Size.Y) * 100 : 0;

                return new Vector2(sizeXRatio, sizeYRatio);
            }

            return SizeDifferenceRatioWithParent;
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

        public Component SetFont(string text, Color? textColor = null, Offset? textPadding = null)
        {
            textColor = textColor ?? Color.White;

            textPadding = textPadding ?? Offset.CreatePadding(OffsetValueType.Piksel, 0, 0, 0, 0);

            Font = new Font(text: text, color: textColor);

            Font.IncreaseLayerDepth(baseDepth: LayerDepth);

            SetPadding(textPadding.Value);

            var newSize = new Vector2(Font.TextMeasure.X + Padding.Left + Padding.Right, Font.TextMeasure.Y + Padding.Top + Padding.Bottom);

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

        //private Vector2 CalculatePaddingRatio()
        //{
        //    float paddingX = Size.X / Padding.X;

        //    float paddingY = Size.Y / Padding.Y;

        //    paddingX = float.IsNaN(paddingX) || float.IsInfinity(paddingX) ? 0 : paddingX;

        //    paddingY = float.IsNaN(paddingY) || float.IsInfinity(paddingY) ? 0 : paddingY;

        //    return new Vector2(paddingX, paddingY);
        //}

        //private Vector2 CalculateMarginRatio()
        //{
        //    if (Parent != null)
        //    {
        //        float marginX = Parent.Size.X / Margin.X;

        //        float marginY = Parent.Size.Y / Margin.Y;

        //        marginX = float.IsNaN(marginX) || float.IsInfinity(marginX) ? 0 : marginX;

        //        marginY = float.IsNaN(marginY) || float.IsInfinity(marginY) ? 0 : marginY;

        //        return new Vector2(marginX, marginY);
        //    }

        //    return MarginRatio;
        //}

        private void CalculateChildOrderNumbers()
        {
            var counter = 1;

            var xGroups = Child.OrderBy(c => c.Position.X)
                                .Select(c => c.Position.X)
                                .Distinct()
                                .Select(px => new
                                {
                                    PositionX = px,
                                    Number = counter++
                                })
                                .ToList();

            counter = 1;

            var yGroups = Child.OrderBy(c => c.Position.Y)
                                .Select(c => c.Position.Y)
                                .Distinct()
                                .Select(py => new
                                {
                                    PositionY = py,
                                    Number = counter++
                                })
                                .ToList();

            foreach (Component children in Child)
            {
                if (xGroups.Any(xg => children.Position.X == xg.PositionX))
                {
                    children.SetChildrenOrderNumberOnXAxis(xGroups.FirstOrDefault(xg => children.Position.X == xg.PositionX).Number);
                }

                if (yGroups.Any(yg => children.Position.Y == yg.PositionY))
                {
                    children.SetChildrenOrderNumberOnYAxis(yGroups.FirstOrDefault(yg => children.Position.Y == yg.PositionY).Number);
                }
            }
        }

    }
}
