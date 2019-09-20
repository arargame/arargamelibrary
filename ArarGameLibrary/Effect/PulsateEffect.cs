using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Effect
{
    public class PulsateEffect : EventManager
    {
        public float OriginalScale { get; private set; }

        public float Speed { get; private set; }

        public float Range { get; private set; }

        public string ToUpperOrLower { get; private set; }

        public PulsateEffect(Sprite sprite, Func<bool> whenToInvoke = null, float lastScale = 1f, float pulsateSpeed = 6f, float range = 0.08f, string toUpperOrLower = "Upper")
            : base(sprite, true)
        {
            SetWhenToInvoke(whenToInvoke);

            SetOriginalScale(lastScale);

            SetPulsateSpeed(pulsateSpeed);

            SetPulsateRange(range);

            SetToUpperOrLower(toUpperOrLower);

            SetTask(() =>
            {
                if (WhenToInvoke == null)
                    return;

                if (WhenToInvoke())
                {
                    Sprite.SetScale(Pulsate(OriginalScale, Speed, Range, ToUpperOrLower));

                    Sprite.IsPulsating = true;
                }
                else
                {
                    if (Sprite.Scale.ToString("0.0") != OriginalScale.ToString("0.0"))
                    {
                        var difference = Math.Abs(Sprite.Scale - OriginalScale);

                        if (Sprite.Scale > OriginalScale)
                        {
                            Sprite.SetScale(Sprite.Scale - (difference > Range ? Range : difference));

                           // Sprite.Scale = MathHelper.Clamp(Sprite.Scale, OriginalScale, Sprite.Scale);
                        }
                        else
                        {
                            Sprite.SetScale(Sprite.Scale + (difference > Range ? Range : difference));

                           // Sprite.Scale = MathHelper.Clamp(Sprite.Scale, Sprite.Scale, OriginalScale);
                        }

                        //Sprite.Scale = OriginalScale > Sprite.Scale ? Sprite.Scale + (MathHelper.Clamp(Range,Range,OriginalScale - Range)) : Sprite.Scale - (Sprite.Scale - OriginalScale);

                        return;
                    }
                    else
                    {
                        Sprite.SetScale(OriginalScale);
                    }

                    Sprite.IsPulsating = false;
                }
            });
        }

        public void SetOriginalScale(float scale)
        {
            OriginalScale = scale;
        }

        public void SetPulsateRange(float range)
        {
            Range = range;
        }

        public void SetPulsateSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetToUpperOrLower(string toUpperOrLower)
        {
            ToUpperOrLower = toUpperOrLower;
        }

        public static float Pulsate(float startingScale = 1f, float speed = 6, float range = 0.05f, string toUpperOrLower = "Upper")
        {
            double time = Global.GameTime.TotalGameTime.TotalSeconds;

            float pulsate = 0f;

            switch (toUpperOrLower)
            {
                case "Upper":

                    pulsate = ((float)Math.Sin(time * speed) + startingScale);

                    break;

                case "Lower":

                    pulsate = ((float)Math.Sin(time * speed) - startingScale);

                    break;

                case "UpperOrLower":

                    pulsate = (float)Math.Sin(time * speed);

                    break;
            }

            pulsate = pulsate * range;

            return startingScale + pulsate;
        }
    }
}
