using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class TimeManager
    {
        public static GameTime GameTime
        {
            get
            {
                return Global.GameTime;
            }
        }

        /// <summary>
        /// Returns the time elapsed since the last update(If the game is running at 60 frames per second the value is likely to be 1/60=0.0166 seconds)
        /// </summary>
        public static TimeSpan ElapsedGameTime
        {
            get
            {
                return GameTime.ElapsedGameTime;
            }
        }

        public static TimeSpan TotalGameTime
        {
            get
            {
                return GameTime.TotalGameTime;
            }
        }

        public static bool IsRunningSlowly
        {
            get
            {
                return GameTime.IsRunningSlowly;
            }
        }

        public static double FPS
        {
            get
            {
                return 1 / ElapsedGameTime.TotalSeconds;
            }
        }

        public static void Draw()
        {
            //Global.SpriteBatch.DrawString(,);
        }
    }
}
