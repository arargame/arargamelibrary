using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Effect
{
    public class PulsateEffect : EffectManager
    {
        public float LastScale { get; set; }

        public float CollapseSpeed { get; set; }

        public PulsateEffect(Sprite sprite, float lastScale = 1f,float collapseSpeed = 0.05f) : base(sprite)
        {
            SetTask(new Action(() =>
            {
                Sprite.Scale = General.Pulsate();
            }));

            SetEndTask(new Action(() =>
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


            }));

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
