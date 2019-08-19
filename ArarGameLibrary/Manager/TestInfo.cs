using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class TestInfo
    {
        private HashSet<string> TestInfoParameters = new HashSet<string>();

        private IDrawableObject DrawableObject { get; set; }

        private Frame DestinationRectangleFrame { get; set; }

        private bool IsVisible { get; set; }

        public FontManager Font { get; set; }

        public TestInfo(IDrawableObject drawableObject)
        {
            DrawableObject = drawableObject;

            Font = FontManager.Create(DrawableObject.GetType().Name,Vector2.Zero,Color.White);
            Font.SetLayerDepth(DrawableObject.LayerDepth + 0.1f);
        }

        public TestInfo AddParameters(params string[] parameters)
        {
            foreach (var p in parameters)
            {
                TestInfoParameters.Add(p);
            }
            
            return this;
        }

        public void Update()
        {
            if (!IsVisible)
                return;

            foreach (var parameter in TestInfoParameters)
            {
                switch (parameter)
                {
                    case "DestinationRectangle":

                        DestinationRectangleFrame = Frame.Create(DrawableObject.DestinationRectangle,Color.Beige);
                        DestinationRectangleFrame.IsInPerformanceMode = true;
                        DestinationRectangleFrame.LoadContent();

                        break;

                    default:
                        break;
                }       
            }

            Font.SetPosition(DrawableObject.Position + new Vector2(10, 10));
            
            Font.Update();
        }

        public void Draw()
        {
            if (!IsVisible)
                return;

            foreach (var parameter in TestInfoParameters)
            {
                switch (parameter)
                {
                    case "DestinationRectangle":
                        DestinationRectangleFrame.Draw();
                        break;

                    default:
                        break;
                }       
            }

            Font.Draw();
        }

        public TestInfo Show(bool enable)
        {
            IsVisible = enable;

            return this;
        }
    }
}
