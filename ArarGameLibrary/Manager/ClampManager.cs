using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class ClampObject
    {
        public string PropertyName { get; set; }

        public float Min { get; set; }

        public float Max { get; set; }

        public ClampObject(string propertyName,float min,float max)
        {
            PropertyName = propertyName;

            Min = min;

            Max = max;
        }

        public ClampObject SetMin(float min)
        {
            Min = min;

            return this;
        }

        public ClampObject SetMax(float max)
        {
            Max = max;

            return this;
        }
    }

    public class ClampManager
    {
        private Sprite Sprite { get; set; }

        List<ClampObject> ClampObjects = new List<ClampObject>();

        public ClampManager(Sprite sprite)
        {
            Sprite = sprite;
        }

        public bool ContainsKey(string property)
        {
            return ClampObjects.Any(co=>co.PropertyName == property);
        }

        public ClampManager Add(params ClampObject[] clampObjects)
        {
            foreach (var clampObject in clampObjects)
            {
                if (!ClampObjects.Any(co => co.PropertyName == clampObject.PropertyName))
                {
                    ClampObjects.Add(clampObject);
                }
            }

            return this;
        }

        public ClampManager RefreshClampObject(string propertyName,float min,float max)
        {
            var co = ClampObjects.FirstOrDefault(c => c.PropertyName == propertyName);

            if (co == null)
                return this;
            else
                co.SetMin(min)
                    .SetMax(max);

            return this;
        }

        public void Update()
        {
            foreach (var co in ClampObjects)
            {
                switch (co.PropertyName)
                {
                    case "Position.X":
                        Sprite.SetPosition(new Vector2(MathHelper.Clamp(Sprite.Position.X, co.Min, co.Max), Sprite.Position.Y));
                        break;

                    case "Position.Y":
                        Sprite.SetPosition(new Vector2(Sprite.Position.X, MathHelper.Clamp(Sprite.Position.Y, co.Min, co.Max)));
                        break;

                    case "Size.X":
                        Sprite.SetSize(new Vector2(MathHelper.Clamp(Sprite.Size.X, co.Min, co.Max), Sprite.Size.Y));
                        break;

                    case "Size.Y":
                        Sprite.SetSize(new Vector2(Sprite.Size.X, MathHelper.Clamp(Sprite.Size.Y, co.Min, co.Max)));
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
