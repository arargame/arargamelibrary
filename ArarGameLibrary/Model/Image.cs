using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{   
    public class Image : Sprite,IPieceContainer
    {
        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public List<Piece> Pieces { get; set; }

        public Image(Texture2D texture)
        {
            SetTexture(texture);
        }
        
        public Image(string name) 
        {
            SetName(name);
        }

        public override void Initialize()
        {
            Pieces = new List<Piece>();

            base.Initialize();
        }

        public override void LoadContent(Texture2D texture = null)
        {
            SetTexture(Name);
            SetSize(new Vector2(Texture.Width,Texture.Height));
        }

        public override void Update(GameTime gameTime = null)
        {
            if (Pieces.Count > 0)
                Pieces.ForEach(p => p.Update());
            else 
                base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
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

            foreach (var piece in Pieces)
            {
                piece.LoadContent();

                piece.SetContainer(this);
            }

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


        public Image SetPiecePosition(Vector2? startingPoint=null)
        {
            startingPoint = startingPoint ?? Position;

            foreach (var piece in Pieces)
            {
                piece.SetPosition(new Vector2(piece.Size.X * piece.ColumnNumber + startingPoint.Value.X, piece.Size.Y * piece.RowNumber + startingPoint.Value.Y));
            }

            return this;
        }

    }
}
