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

        public Font Font { get; set; }

        public TestInfo(IDrawableObject drawableObject)
        {
            DrawableObject = drawableObject;

            //After the font class started to derive from the sprite class each font object we create calls a testinfo object which uses a font.So it leads to infinitive loop(stackoverflowexception)
            if (DrawableObject.GetType().Name != "Font")
            {
                Font = new Font(text: DrawableObject.GetType().Name);
                Font.SetLayerDepth(DrawableObject.LayerDepth + 0.1f);
            }
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

        public TestInfo Show(bool enable = true)
        {
            IsVisible = enable;

            return this;
        }
    }
}
