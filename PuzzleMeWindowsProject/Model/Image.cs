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
        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public List<Piece> Pieces { get; set; }
        
        public Image() { }

        public override void LoadContent()
        {
            SetTexture(Name);
        }

        public override void Initialize()
        {
            Pieces = new List<Piece>();

            base.Initialize();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }

        public Image SetRowAndColumnCount(int rowCount,int columnCount)
        {
            RowCount = rowCount;

            ColumnCount = columnCount;

            //Texture

            return this;
        }

    }
}
