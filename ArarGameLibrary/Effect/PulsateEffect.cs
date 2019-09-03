using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
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

        public float CollapseSpeed { get; private set; }

        public float PulsateSpeed { get; private set; }

        public PulsateEffect(Sprite sprite, Func<bool> whenToInvoke = null, float lastScale = 1f, float collapseSpeed = 0.05f, float pulsateSpeed = 6f)
            : base(sprite, true)
        {
            SetWhenToInvoke(whenToInvoke);

            SetOriginalScale(lastScale);

            SetCollapseSpeed(collapseSpeed);

            SetPulsateSpeed(pulsateSpeed);

            SetTask(() =>
            {
                if (WhenToInvoke == null)
                    return;

                if (WhenToInvoke())
                {
                    Sprite.Scale = General.Pulsate(OriginalScale);
                }
                else
                {
                    if (Sprite.Scale.ToString("0.0") != OriginalScale.ToString("0.0"))
                    {
                        Sprite.Scale = OriginalScale > Sprite.Scale ? Sprite.Scale + CollapseSpeed : Sprite.Scale - CollapseSpeed;

                        return;
                    }
                    else
                    {
                        Sprite.Scale = OriginalScale;
                    }
                }
            });
        }

        public void SetOriginalScale(float scale)
        {
            OriginalScale = scale;
        }

        public void SetCollapseSpeed(float speed)
        {
            CollapseSpeed = speed;
        }

        public void SetPulsateSpeed(float speed)
        {
            PulsateSpeed = speed;
        }
    }
}
