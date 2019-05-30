using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public interface ITheme
    {
        //IScreen Screen { get; set; }

        ThemeMode Mode { get; set; }

        Color GetColor();
    }

    public enum ThemeMode
    {
        White,
        Dark
    }

    public class Theme : ITheme
    {
        //public IScreen Screen { get; set; }

        public ThemeMode Mode { get; set; }

        public Theme(ThemeMode themeMode)
        {
            //Screen = screen;
            Mode = themeMode;
        }

        public Color GetColor()
        {
            return GetDefaultColorByMode(Mode);
        }

        public static Color GetDefaultColorByMode(ThemeMode mode)
        {
            var color = Color.White;

            switch (mode)
            {
                case ThemeMode.White:

                    color = Color.White;

                    break;

                case ThemeMode.Dark:

                    color = Color.Black;

                    break;

                default:
                    break;
            }

            return color;
        }
    } 
}
