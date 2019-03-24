using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public class Image : Sprite
    {
        public int Row { get; set; }

        public int Column { get; set; }

        //public List<Piece> Pieces = new List<Piece>();

        public Image() { }

        public override void LoadContent()
        {
            SetTexture(Name);
        }

        public Image SetRowCount(int row)
        {
            Row = row;

            return this;
        }

        public Image SetColumnCount(int column)
        {
            Column = column;

            return this;
        }

    }
}
