using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PuzzleMeWindowsProject.Model
{
    public class EmptyPiece : Piece
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            SetColor(Color.White);
            SetTexture(TextureManager.CreateTexture2D("Textures/emptyPiece"));
            Frame.ChangeAllLinesColor(Color.Red);
        }
    }
}
