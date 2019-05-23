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

        public TestInfo(IDrawableObject drawableObject)
        {
            DrawableObject = drawableObject;
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

                        var drP1 = new Vector2(DrawableObject.DestinationRectangle.X, DrawableObject.DestinationRectangle.Y);
                        var drP2 = new Vector2(DrawableObject.DestinationRectangle.Right,drP1.Y);
                        var drP3 = new Vector2(DrawableObject.DestinationRectangle.Right,DrawableObject.DestinationRectangle.Bottom);
                        var drP4 = new Vector2(DrawableObject.DestinationRectangle.X, drP3.Y);

                        DestinationRectangleFrame = new Frame(drP1, drP2, drP3, drP4, Color.Beige);
                        DestinationRectangleFrame.IsInPerformanceMode = true;
                        DestinationRectangleFrame.LoadContent();

                        break;

                    default:
                        break;
                }       
            }
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
        }

        public TestInfo Show(bool enable)
        {
            IsVisible = enable;

            return this;
        }
    }
}
