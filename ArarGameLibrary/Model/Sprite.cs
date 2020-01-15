using ArarGameLibrary.Effect;
using ArarGameLibrary.Event;
using ArarGameLibrary.Extension;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ValueHistoryManagement;

namespace ArarGameLibrary.Model
{
    public interface IXna
    {
        void Initialize();

        void LoadContent(Texture2D texture = null);

        void UnloadContent();

        void Update(GameTime gameTime = null);

        void Draw(SpriteBatch spriteBatch = null);
    }

    public interface IClickableObject
    {
        bool IsClickable { get; set; }
        bool IsDragable { get; set; }
        bool IsDragging { get; set; }
        bool IsHovering { get; set; }
        bool IsSelecting { get; set; }
        void SetClickable(bool enable);
        void SetDragable(bool enable);
        Vector2 DroppingRange { get; set; }
    }


    public abstract class Sprite : BaseObject, IDrawableObject, IClickableObject
    {
        #region Properties

        public BlendState BlendState { get; set; }

        public Rectangle CollisionRectangle { get; set; }
        public Color Color { get; set; }
        public ClampManager ClampManager { get; set; }

        public DepthStencilState DepthStencilState { get; set; }
        public Rectangle DestinationRectangle
        {
            get
            {
                return new Rectangle((int)Math.Ceiling(Position.X), (int)Math.Ceiling(Position.Y), (int)Math.Ceiling(Size.X * Scale), (int)Math.Ceiling((Size.Y * Scale)));
            }
        }
        public int DrawMethodType { get; set; }
        public Vector2 DroppingRange { get; set; }

        public Microsoft.Xna.Framework.Graphics.Effect Effect { get; set; }

        public bool IsActive { get; set; }
        public bool IsAlive { get; set; }
        public bool IsClickable { get; set; }
        public bool IsDragable { get; set; }
        public bool IsDragging { get; set; }
        public bool IsPulsating { get; set; }
        public bool IsHovering { get; set; }
        public bool IsSelecting { get; set; }
        public bool IsVisible { get; set; }

        //From 0f to 1f where 1f is the top layer
        public float LayerDepth { get; set; }
        public const float LayerDepthPlus = 0.01f;

        public Offset Margin { get; set; }

        public Vector2 Origin { get; set; }

        public Offset Padding { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PositionChangingRatio
        {
            get
            {
                var histories = ValueHistoryManager.GetRecordsByPropertyName("Position", 2).ToList();

                if (histories.Count() == 2)
                {
                    var previousValue = (Vector2)histories[1].Value;

                    var currentValue = (Vector2)histories[0].Value;

                    return (previousValue != Vector2.Zero && currentValue != Vector2.Zero) || previousValue != Vector2.Zero ? currentValue / previousValue : new Vector2(1,1);
                }

                return new Vector2(1, 1);
            }
        }

        public RasterizerState RasterizerState { get; set; }
        public float Rotation { get; set; }

        public SamplerState SamplerState { get; set; }
        public float Scale { get; set; }
        //public bool SimpleShadowVisibility { get; set; }
        public Vector2 Size { get; set; }
        public Vector2? SizeChangingRatio
        {
            get
            {
                var histories = ValueHistoryManager.GetRecordsByPropertyName("Size", 2).ToList();

                if (histories.Count() == 2)
                {
                    var previousValue = (Vector2)histories[1].Value;

                    var currentValue = (Vector2)histories[0].Value;

                    var result = ((currentValue - previousValue) / previousValue) * 100;

                    if (float.IsInfinity(result.X) || float.IsNaN(result.X))
                        result.X = 0;

                    if (float.IsInfinity(result.Y) || float.IsNaN(result.Y))
                        result.Y = 0;

                    return result != Vector2.Zero ? result : (Vector2?)null;
                }

                return null;
            }
        }
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Speed { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteSortMode SpriteSortMode { get; set; }

        public TestInfo TestInfo { get; set; }
        public Texture2D Texture { get; set; }
        public Matrix? TransformMatrix { get; set; }

        public delegate void SomethingHasBeenChanged();
        public event SomethingHasBeenChanged OnChangeRectangle;

        //public virtual void Refresh(Action action = null)
        //{
        //    if (action != null)
        //        action.Invoke();
        //}

        //public List<EffectManager> Effects = new List<EffectManager>();
        public List<EventManager> Events = new List<EventManager>();

        #endregion

        #region Constructor 

        public Sprite()
        {
            Initialize();
        }

        #endregion

        #region Functions

        #region XNA Functions
        public virtual void Initialize()
        {
            SetName(MemberInfoName);

            OnChangeRectangle += SetRectangle;

            IsActive = IsAlive = IsVisible = true;

            ClampManager = new ClampManager(this)
                .Add(new ClampObject("Size.X", 0f, float.MaxValue))
                .Add(new ClampObject("Size.Y", 0f, float.MaxValue));

            SetStartingSettings();

            //DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            //destinationRectangle = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)size.X, (int)size.Y);

            SourceRectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);

            Color = Color.White;

            //Effects.Add(new PulsateEffect(this));

            Events.Add(new DraggingEvent(this));

            Events.Add(new SimpleShadowEffect(this, new Vector2(-6, -6)));

            Events.Add(new PulsateEffect(this));

            TestInfo = new TestInfo(this);

            SpriteBatch = Global.SpriteBatch;

            var equalityFunctionForVector2 = new Func<object, object, bool>((previousValue, currentValue) =>
            {
                var previousValueAsVector2 = (Vector2)previousValue;

                var currentValueAsVector2 = (Vector2)currentValue;

                return previousValueAsVector2 != currentValueAsVector2;
            });

            ValueHistoryManager.AddSetting(new ValueHistorySetting("Position", 2, equalityFunctionForVector2));

            ValueHistoryManager.AddSetting(new ValueHistorySetting("Size", 2, equalityFunctionForVector2));
        }

        public virtual void LoadContent(Texture2D texture = null)
        {
            SetTexture(texture);
        }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime = null)
        {
            if (IsActive)
            {
                SetRectangle();

                SetOrigin();

                //if (IsPulsating)
                //{
                //    Scale = General.Pulsate();
                //}

                //foreach (var effect in Effects)
                //{
                //    effect.Update();
                //}

                foreach (var e in Events)
                {
                    e.Update();
                }

                ClampManager.Update();

                IsHovering = InputManager.IsHovering(DestinationRectangle);

                if (IsClickable)
                {
                    IsSelecting = InputManager.Selected(DestinationRectangle);
                }

                TestInfo.Update();

                ValueHistoryManager.Update();
            }
        }

        public void Draw(Action drawingFunction)
        {
            Global.SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, TransformMatrix);

            drawingFunction();

            Global.SpriteBatch.End();
        }

        public virtual void Draw(SpriteBatch spriteBatch = null)
        {
            SetSpriteBatch(spriteBatch);

            if (IsActive && IsVisible)
            {
                if (Texture != null)
                {
                    switch (DrawMethodType)
                    {
                        case 1:
                            SpriteBatch.Draw(Texture, DestinationRectangle, Color);
                            break;

                        case 2:
                            SpriteBatch.Draw(Texture, Position, Color);
                            break;

                        case 3:
                            SpriteBatch.Draw(Texture, DestinationRectangle, SourceRectangle, Color);
                            break;

                        case 4:
                            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color);
                            break;

                        case 5:
                            SpriteBatch.Draw(Texture, DestinationRectangle, SourceRectangle, Color, Rotation, Origin, SpriteEffects, LayerDepth);
                            break;

                        case 6:
                            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
                            break;

                        case 7:
                            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, new Vector2(Scale), SpriteEffects, LayerDepth);
                            break;

                        default:
                            SpriteBatch.Draw(Texture, DestinationRectangle, Color);
                            break;
                    }
                }

                foreach (var e in Events)
                {
                    e.Draw();
                }

                TestInfo.Draw();
            }
        }

        #endregion

        
        public virtual void Align(Vector2 offset, Rectangle? parentRect = null)
        {
            if (parentRect == null)
                parentRect = Global.ViewportRect;

            SetPosition(new Vector2((int)(parentRect.Value.Position().X + offset.X), (int)(parentRect.Value.Position().Y + offset.Y)));
        }


        //public T GetEffect<T>() where T : EffectManager
        //{
        //    return EffectManager.Get<T>(Effects);
        //}

        public T GetEvent<T>() where T : EventManager
        {
            return EventManager.Get<T>(Events);
        }

        public virtual void IncreaseLayerDepth(float? additionalDepth = null, float? baseDepth = null)
        {
            SetLayerDepth((baseDepth ?? LayerDepth) + (additionalDepth ?? LayerDepthPlus));
        }

        //public void Pulsate(bool enable)
        //{
        //    //IsPulsating = enable;

        //    //var pulsateEffect = GetEffect<PulsateEffect>();

        //    //if (pulsateEffect == null)
        //    //    return;

        //    //if (IsPulsating)
        //    //    pulsateEffect.Start();
        //    //else
        //    //    pulsateEffect.End();
        //}

        public void RefreshRectangle()
        {
            OnChangeRectangle();
        }

        //public void ShowSimpleShadow(bool enable)
        //{
        //    //SimpleShadowVisibility = enable;

        //    //var simpleShadowEffect = GetEffect<SimpleShadowEffect>();

        //    //if (simpleShadowEffect == null)
        //    //    return;

        //    //if (SimpleShadowVisibility)
        //    //    simpleShadowEffect.Start();
        //    //else
        //    //    simpleShadowEffect.End();
        //}

        #region SetFunctions

        public void SetActive(bool enable)
        {
            IsActive = enable;
        }

        public void SetAlive(bool enable)
        {
            IsAlive = enable;
        }

        public void SetBlendState(BlendState blendState)
        {
            BlendState = blendState;
        }

        public void SetClickable(bool enable)
        {
            IsClickable = enable;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public void SetDepthStencilState(DepthStencilState depthStencilState)
        {
            DepthStencilState = depthStencilState;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetDragable(bool enable = true)
        {
            IsDragable = enable;

            if (enable)
                SetClickable(true);
        }

        public void SetDrawMethodType(int methodType)
        {
            DrawMethodType = methodType;
        }

        public void SetEffect(Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            Effect = effect;
        }

        public void SetLayerDepth(float layerDepth)
        {
            LayerDepth = MathHelper.Clamp(layerDepth, 0f, 1f);
        }

        public void SetMargin(Offset margin)
        {
            Margin = margin;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetOrigin(Vector2? origin = null)
        {
            if (origin != null)
                Origin = origin.Value;
            else
                Origin = Vector2.Zero;
        }

        public void SetPadding(Offset padding)
        {
            Padding = padding;
        }

        public void SetPosition(Vector2 position)
        {
            //if (Position != Vector2.Zero && Position == position)
            //    return;

            Position = position;

            if(ValueHistoryManager.HasChangedFor(new ValueHistoryRecord("Position", position)))
                OnChangeRectangle?.Invoke();
        }

        public void SetRasterizerState(RasterizerState rasterizerState)
        {
            RasterizerState = rasterizerState;
        }

        public void SetRectangle()
        {
            SourceRectangle = new Rectangle(0, 0, Texture != null ? Texture.Width : DestinationRectangle.Width, Texture != null ? Texture.Height : DestinationRectangle.Height);
            //CollisionRectangle = new Rectangle(Des);
            //SourceRectangle = new Rectangle(animation.FrameBounds.X, animation.FrameBounds.Y, (int)Size.X, (int)Size.Y);
            //            destinationRectangle = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)size.X, (int)size.Y);
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void SetSamplerState(SamplerState samplerState)
        {
            SamplerState = samplerState;
        }

        public void SetScale(float scale)
        {
            //if (Scale != 1f && Scale == scale)
            //    return;

            Scale = scale;

            OnChangeRectangle?.Invoke();
        }

        public void SetSize(Vector2 size)
        {
            //if (Size != Vector2.Zero && Size == size)
            //    return;
            Size = size;

            ClampManager.Update();

            if (ValueHistoryManager.HasChangedFor(new ValueHistoryRecord("Size", Size)))
                OnChangeRectangle?.Invoke();
        }

        public void SetSpeed(Vector2 speed)
        {
            Speed = speed;
        }

        public void SetSpriteBatch(GraphicsDevice graphicsDevice = null)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice ?? Global.GraphicsDevice);
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch = null)
        {
            if(spriteBatch!=null)
                SpriteBatch = spriteBatch;
        }

        public void SetSpriteEffects(SpriteEffects effects)
        {
            SpriteEffects = effects;
        }

        public void SetSpriteSortMode(SpriteSortMode spriteSortMode)
        {
            SpriteSortMode = spriteSortMode;
        }

        public void SetTexture(string assetName, string rootDirectory = "Content")
        {
            Texture = TextureManager.CreateTexture2D(assetName, rootDirectory);
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void SetTexture()
        {
            Texture = TextureManager.CreateTexture2DByRandomColor();
        }

        public void SetTexture(Color color, int width = 1, int height = 1)
        {
            Texture = TextureManager.CreateTexture2DBySingleColor(color, width, height);
        }

        public void SetTransformMatrix(Matrix transformMatrix)
        {
            TransformMatrix = transformMatrix;
        }

        public virtual void SetVisible(bool enable)
        {
            IsVisible = enable;
        }

        #endregion


        #region StartingFunctions

        private void SetStartingSettings()
        {
            SetBlendState(BlendState.AlphaBlend);

            SetClickable(false);

            SetStartingLayerDepth();

            SetOrigin();

            SetSpriteSortMode(SpriteSortMode.FrontToBack);
            SetStartingPosition();
            SetStartingRotation();
            SetStartingScale();
            SetStartingSize();
            SetStartingSpeed();
            SetStartingSpriteEffects();
        }

        public virtual void SetStartingLayerDepth()
        {
            SetLayerDepth(0.5f);
        }

        public virtual void SetStartingScale()
        {
            SetScale(1f);
        }

        public virtual void SetStartingSpriteEffects()
        {
            SpriteEffects = SpriteEffects.None;
        }

        public virtual void SetStartingPosition()
        {
            SetPosition(Vector2.Zero);
        }

        public virtual void SetStartingRotation()
        {
            SetRotation(0f);
        }

        public virtual void SetStartingSize()
        {
            SetSize(Vector2.Zero);
        }

        public virtual void SetStartingSpeed()
        {
            SetSpeed(Vector2.Zero);
        }

        #endregion


        #endregion


        public Sprite AsSprite()
        {
            return this as Sprite;
        }
    }
}
