using ArarGameLibrary.Effect;
using ArarGameLibrary.Event;
using ArarGameLibrary.Extension;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Rectangle CollisionRectangle { get; set; }
        public Color Color { get; set; }
        public ClampManager ClampManager { get; set; }

        public Rectangle DestinationRectangle
        {
            get
            {
                return new Rectangle((int)Math.Ceiling(Position.X), (int)Math.Ceiling(Position.Y), (int)Math.Ceiling(Size.X * Scale), (int)Math.Ceiling((Size.Y * Scale)));
            }
        }
        public int DrawMethodType { get; set; }
        public Vector2 DroppingRange { get; set; }

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

        public Vector2 Margin { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 Padding { get; set; }
        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }
        //public bool SimpleShadowVisibility { get; set; }
        public Vector2 Size { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Speed { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public TestInfo TestInfo { get; set; }
        public Texture2D Texture { get; set; }

        public delegate void ChangingSomething();
        public event ChangingSomething OnChangeRectangle;

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

            SetStartingSettings();

            //DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            //destinationRectangle = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)size.X, (int)size.Y);

            SourceRectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);

            Color = Color.White;

            //Effects.Add(new PulsateEffect(this));

            Events.Add(new DraggingEvent(this));

            Events.Add(new SimpleShadowEffect(this, new Vector2(-6, -6)));

            Events.Add(new PulsateEffect(this));

            ClampManager = new ClampManager(this);

            TestInfo = new TestInfo(this);

            SpriteBatch = Global.SpriteBatch;
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

                TestInfo.Update();

                IsHovering = InputManager.IsHovering(DestinationRectangle);

                if (IsClickable)
                {
                    IsSelecting = InputManager.Selected(DestinationRectangle);
                }
            }
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

                //foreach (var effect in Effects)
                //{
                //    effect.Draw();
                //}

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

        public void SetClickable(bool enable)
        {
            IsClickable = enable;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetDragable(bool enable = true)
        {
            IsDragable = enable;

            SetClickable(true);
        }

        public void SetDrawMethodType(int methodType)
        {
            DrawMethodType = methodType;
        }

        public void SetLayerDepth(float layerDepth)
        {
            LayerDepth = MathHelper.Clamp(layerDepth, 0f, 1f);
        }

        public void SetMargin(Vector2 margin)
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

        public void SetPadding(Vector2 padding)
        {
            Padding = padding;
        }

        public void SetPosition(Vector2 position)
        {
            //if (Position != Vector2.Zero && Position == position)
            //    return;

            Position = position;

            if (OnChangeRectangle != null)
                OnChangeRectangle();
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

        public void SetScale(float scale)
        {
            //if (Scale != 1f && Scale == scale)
            //    return;

            Scale = scale;

            if (OnChangeRectangle != null)
                OnChangeRectangle();
        }

        public void SetSize(Vector2 size)
        {
            //if (Size != Vector2.Zero && Size == size)
            //    return;

            Size = size;

            if (OnChangeRectangle != null)
                OnChangeRectangle();
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

        public void SetTexture(string name)
        {
            Texture = Global.Content.Load<Texture2D>(name);
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void SetVisible(bool enable)
        {
            IsVisible = enable;
        }

        #endregion


        #region StartingFunctions

        private void SetStartingSettings()
        {
            SetClickable(false);

            SetStartingLayerDepth();

            SetOrigin();

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



    }
}
