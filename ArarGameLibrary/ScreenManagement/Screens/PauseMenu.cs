using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement.Screens
{
    public class PauseMenu : Menu
    {
        Texture2D TransparentBackground;

        public override void Initialize()
        {
            Global.ChangeGameWindowTitle("Pause");
        }

        public override bool Load()
        {
            var collection = new Dictionary<string, Action>();

            collection.Add("Back", () =>
            {
                DisableThenAddNew(PreviousScreen.Activate() ?? new MainMenu());
            });

            Components.AddRange(Button.Sort(collection));

            TransparentBackground = TextureManager.CreateTexture2DBySingleColor(new Color((byte)0, (byte)0, (byte)0, (byte)(0.5 * 255)), 1, 1);

            return true && base.Load();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            Global.SpriteBatch.Draw(TransparentBackground,new Rectangle(0,0,Global.ViewportWidth,Global.ViewportHeight),Color.White);

            base.Draw();
        }
    }
}
