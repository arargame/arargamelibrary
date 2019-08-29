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
        public float LastScale { get; set; }

        public float CollapseSpeed { get; set; }

        public PulsateEffect(Sprite sprite,Func<bool> whenToInvoke = null, float lastScale = 1f, float collapseSpeed = 0.05f) 
            : base(sprite,true)
        {
            SetWhenToInvoke(whenToInvoke);

            SetTask(() =>
            {
                if (WhenToInvoke == null)
                    return;

                if (WhenToInvoke())
                {
                    Sprite.Scale = General.Pulsate();
                }
                else
                {
                    if (Sprite.Scale.ToString("0.0") != LastScale.ToString("0.0"))
                    {
                        Sprite.Scale = LastScale > Sprite.Scale ? Sprite.Scale + CollapseSpeed : Sprite.Scale - CollapseSpeed;

                        return;
                    }
                    else
                    {
                        Sprite.Scale = LastScale;
                    }
                }
            });

            SetLastScale(lastScale);
            SetCollapseSpeed(collapseSpeed);
        }

        public void SetLastScale(float scale)
        {
            LastScale = scale;
        }

        public void SetCollapseSpeed(float speed)
        {
            CollapseSpeed = speed;
        }
    }
}
