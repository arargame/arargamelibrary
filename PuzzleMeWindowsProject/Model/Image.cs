using Microsoft.Xna.Framework;
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
        
        public Image(string name) 
        {
            SetName(name);
        }

        public override void Initialize()
        {
            Pieces = new List<Piece>();

            base.Initialize();
        }

        public override void LoadContent()
        {
            SetTexture(Name);
        }

        public override void Update()
        {
            if (Pieces.Count > 0)
                Pieces.ForEach(p => p.Update());
            else 
                base.Update();
        }

        public override void Draw()
        {
            if (Pieces.Count > 0)
                Pieces.ForEach(p => p.Draw());
            else
                base.Draw();
        }


        public Image SetRowAndColumnCount(int pieceCount)
        {
            return SetRowAndColumnCount(pieceCount, pieceCount);
        }

        public Image SetRowAndColumnCount(int rowCount,int columnCount)
        {
            RowCount = rowCount;

            ColumnCount = columnCount;

            Pieces = TextureManager.Crop(Texture, rowCount, columnCount);

            Pieces.ForEach(p=>p.LoadContent());

            return this;
        }

        public Image SetPieceSize(Vector2 pieceSize)
        {
            foreach (var piece in Pieces)
            {
                piece.SetSize(pieceSize);
            }

            return this;
        }

    }
}
