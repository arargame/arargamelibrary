using ArarGameLibrary.Effect;
using ArarGameLibrary.Event;
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
    //    btn1001 = new Button();
    //btn1001.SetPosition(new Vector2(250,250));
    //btn1001.LoadContent(TextureManager.CreateTexture2DByRandomColor());
    //btn1001.SetFont("Hello Pluton",Color.Salmon,new Vector2(10));
    //btn1001.SetFrame(Color.Yellow, 2f);
    //btn1001.SetDragable(true);

    public class Button : Component
    {
        //public Font Font { get; private set; }

        public Button(bool isPulsating = false)
        {
            if (isPulsating)
            {
                var pulsateEffect = GetEvent<PulsateEffect>();

                pulsateEffect.SetOriginalScale(Scale);

                if (pulsateEffect != null)
                    pulsateEffect.SetWhenToInvoke(() =>
                    {
                        return IsHovering;
                    });
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            OnChangeRectangle += Button_OnChangeRectangle;
        }

        public override void LoadContent(Texture2D texture = null)
        {
            base.LoadContent(texture);
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw();
        }


        private void Button_OnChangeRectangle()
        {

        }

    }
}
