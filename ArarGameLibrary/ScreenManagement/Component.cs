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

        public Vector2 SizeRatioToParent { get; set; }
        public Vector2 PositionRatio { get; set; }

        public Font Font { get; private set; }

        public bool IsFixedToParentPosition { get; private set; }

        public bool IsFixedToParentSize { get; private set; }

        public List<Component> Children { get; set; }

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

            foreach (var child in Children)
            {
                (child as Sprite).IncreaseLayerDepth(baseDepth: LayerDepth);
            }
        }

        public void SetClickAction(Action action)
        {
            ClickAction = action;
        }

        public override void Initialize()
        {
            Children = new List<Component>();

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

                foreach (var child in Children)
                {
                    child.Update(gameTime);
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

                foreach (var child in Children)
                {
                    child.Draw(spriteBatch);
                }
            }
        }

        public override void Align(Vector2 offset, Rectangle? parentRect = null)
        {
            parentRect = parentRect ?? (Parent != null ? Parent.DestinationRectangle : (Rectangle?)null);

            base.Align(offset, parentRect);
        }

        public new Component SetMargin(Offset margin)
        {
            Margin = margin;

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
                }

                if (newPosition != Vector2.Zero)
                    SetPosition(newPosition);

                if (newSize != Vector2.Zero)
                {
                    var sizeXRatio = Size.X != 0 && newSize.X != 0 ? (newSize.X / Size.X) * 100 : 0;

                    var sizeYRatio = Size.Y != 0 && newSize.Y != 0 ? (newSize.Y / Size.Y) * 100 : 0;

                    SetSizeRatioToParent(new Vector2(sizeXRatio, sizeYRatio));

                    SetSize(newSize);
                }
            }

            return this;
        }

        public new Component SetPadding(Offset padding)
        {
            Padding = padding;

            foreach (var child in Children)
            {
                var childrenNewPosition = Vector2.Zero;
                var childrenNewSize = Vector2.Zero;

                if (Padding.OffsetValueType == OffsetValueType.Piksel)
                {
                    childrenNewPosition = child.Position + new Vector2(Padding.Left, Padding.Top);

                    childrenNewSize = Size - new Vector2(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
                }
                else if (Padding.OffsetValueType == OffsetValueType.Ratio)
                {
                    childrenNewPosition = child.Position + new Vector2(child.Size.X * Padding.Left / 100, child.Size.Y * Padding.Top / 100);

                    childrenNewSize = child.Size - new Vector2((child.Size.X * Padding.Left / 100) + (child.Size.X * Padding.Right / 100), (child.Size.Y * Padding.Top / 100) + (child.Size.Y * Padding.Bottom / 100));
                }

                if (childrenNewPosition != Vector2.Zero)
                    child.SetPosition(childrenNewPosition);

                if (childrenNewSize != Vector2.Zero)
                {
                    var sizeXRatio = Size.X != 0 && childrenNewSize.X != 0 ? (childrenNewSize.X / Size.X) * 100 : 0;

                    var sizeYRatio = Size.Y != 0 && childrenNewSize.Y != 0 ? (childrenNewSize.Y / Size.Y) * 100 : 0;

                    child.SetSizeRatioToParent(new Vector2(sizeXRatio, sizeYRatio));

                    child.SetSize(childrenNewSize);
                }
            }

            return this;
        }

        public Component SetParent(Component parent)
        {
            Parent = parent;

            IncreaseLayerDepth();

            if (Position == Vector2.Zero)
                SetPosition(Parent.Position);

            SetDistanceToParent();
            SetSizeRatioToParent();

            return this;
        }


        public Component AddChild(params Component[] child)
        {
            Children.AddRange(child);

            if (Size == Vector2.Zero)
                SetSize(new Vector2(100, 100));

            child.ToList().ForEach(c => c.SetParent(this));

            Component_OnChangeRectangle();

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
                if (Padding.IsZero)
                    Font.CalculateNewPosition(DestinationRectangle, null, true);
                else
                    Font.CalculateNewPosition(DestinationRectangle, Padding);

                Font.SetScale(Scale);
            }

            SetDistanceToParent();

            SetSizeRatioToParent();

            foreach (Component child in Children)
            {
                if (child.IsFixedToParentPosition)
                {
                    var newDistanceToParent = Position - child.Position;

                    if (newDistanceToParent != child.DistanceToParent)
                    {
                        child.SetPosition(Position - child.DistanceToParent);
                    }
                }

                if (child.IsFixedToParentSize)
                {
                    var newSizeDifferenceWithParent = child.CalculateSizeRatioParent();

                    child.SetSizeRatioToParent();

                    if (newSizeDifferenceWithParent != child.SizeRatioToParent)
                    {
                        var newChildrenSize = (Size * child.SizeRatioToParent) / 100;

                        child.SetSize(newChildrenSize);

                        Vector2 distanceToParent = child.DistanceToParent;

                        var distanceChangeToParent = Vector2.Zero;

                        if (SizeChangingRatio != null)
                        {
                            if (SizeChangingRatio.Value != Vector2.Zero)
                                distanceChangeToParent = (SizeChangingRatio.Value * child.DistanceToParent) / 100;
                        }

                        var newPosition = Position - (distanceToParent + distanceChangeToParent);

                        if (newPosition != child.Position)
                        {
                            child.SetPosition(newPosition);
                        }
                    }
                }
            }
        }

        public void SetDistanceToParent()
        {
            if (Parent != null)
            {
                DistanceToParent = Parent.Position - Position;
            }
        }

        public void SetSizeRatioToParent(Vector2? sizeDifferenceWithParent = null)
        {
            if (sizeDifferenceWithParent != null)
                SizeRatioToParent = sizeDifferenceWithParent.Value;
            else
            {
                var newSizeDifferenceWithParent = CalculateSizeRatioParent();

                if (SizeRatioToParent == Vector2.Zero && newSizeDifferenceWithParent != Vector2.Zero)
                    SizeRatioToParent = newSizeDifferenceWithParent;
            }
        }

        private Vector2 CalculateSizeRatioParent()
        {
            if (Parent != null)
            {
                var sizeXRatio = Parent.Size.X != 0 && Size.X != 0 ? (Size.X / Parent.Size.X) * 100 : 0;

                var sizeYRatio = Parent.Size.Y != 0 && Size.Y != 0 ? (Size.Y / Parent.Size.Y) * 100 : 0;

                return new Vector2(sizeXRatio, sizeYRatio);
            }

            return SizeRatioToParent;
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

            foreach (var child in Children)
            {
                child.SetVisible(enable);
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

            Font = new Font(text: text, fontColor: textColor);

            Font.IncreaseLayerDepth(baseDepth: LayerDepth);

            //SetPadding(textPadding.Value);

            Component_OnChangeRectangle();

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
                list.AddRange(Children.OfType<T>().Where(predicate));
            }
            else
            {
                list.AddRange(Children.OfType<T>());
            }

            foreach (var child in Children)
            {
                list.AddRange(child.GetChildAs<T>(predicate));
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

            var xGroups = Children.OrderBy(c => c.Position.X)
                                .Select(c => c.Position.X)
                                .Distinct()
                                .Select(px => new
                                {
                                    PositionX = px,
                                    Number = counter++
                                })
                                .ToList();

            counter = 1;

            var yGroups = Children.OrderBy(c => c.Position.Y)
                                .Select(c => c.Position.Y)
                                .Distinct()
                                .Select(py => new
                                {
                                    PositionY = py,
                                    Number = counter++
                                })
                                .ToList();

            foreach (Component child in Children)
            {
                if (xGroups.Any(xg => child.Position.X == xg.PositionX))
                {
                    child.SetChildrenOrderNumberOnXAxis(xGroups.FirstOrDefault(xg => child.Position.X == xg.PositionX).Number);
                }

                if (yGroups.Any(yg => child.Position.Y == yg.PositionY))
                {
                    child.SetChildrenOrderNumberOnYAxis(yGroups.FirstOrDefault(yg => child.Position.Y == yg.PositionY).Number);
                }
            }
        }

        public Component AlignChildAsCenter(Vector2? sizeDifference = null)
        {
            foreach (var child in Children)
            {
                sizeDifference = sizeDifference ?? Size - child.Size;

                var childrenNewSize = Size - sizeDifference.Value;

                var sizeXRatio = Size.X != 0 && childrenNewSize.X != 0 ? (childrenNewSize.X / Size.X) * 100 : 0;

                var sizeYRatio = Size.Y != 0 && childrenNewSize.Y != 0 ? (childrenNewSize.Y / Size.Y) * 100 : 0;


                child.SetSizeRatioToParent(new Vector2(sizeXRatio,sizeYRatio));
                child.SetSize(childrenNewSize);

                child.SetPosition(new Vector2(DestinationRectangle.Center.X - child.Size.X / 2, DestinationRectangle.Center.Y - child.Size.Y / 2));
            }

            return this;
        }

        public Component FloatTo(string direction)
        {
            if (Parent != null)
            {
                switch (direction)
                {
                    case "right":

                        SetPosition(new Vector2(Parent.DestinationRectangle.Right - Size.X, Parent.Position.Y));

                        break;

                    case "left":

                        SetPosition(Parent.Position);

                        break;

                    default:
                        break;
                }
            }

            return this;
        }
    }
}
