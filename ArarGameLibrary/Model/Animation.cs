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
    public class AnimationFrame : Sprite
    {
        public int Number { get; set; }

        public Animation Animation { get; set; }

        //public AnimationFrame Previous { get; set; }

        //public AnimationFrame Next { get; set; }

        public AnimationFrame(Animation animation,int number)
        {
            Animation = animation;

            Number = number;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(Texture2D texture = null)
        {
            throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime = null)
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            base.Draw();
        }
    }

    public class Animation : Sprite
    {
        public Sprite Sprite { get; set; }

        public LinkedList<AnimationFrame> Frames { get; set; }

        public int ActiveFrameNumber { get; set; }

        //public List<Piece> Pieces { get; set; }

        public int TotalFrameCount { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public AnimationFrame ActiveFrame { get; set; }

        public double Interval { get; set; }

        public double TimeUntilNextFrame { get; set; }

        public Animation(Texture2D texture, Vector2 position, Vector2 size, int rowCount, int columnCount, int totalFrameCount)
        {
            SetTexture(texture);

            SetPosition(position);

            SetSize(size);

            TotalFrameCount = totalFrameCount;

            RowCount = rowCount;

            ColumnCount = columnCount;

            Interval = (float)1/(rowCount*columnCount);
        }

        public override void Initialize()
        {
            base.Initialize();

            Frames = new LinkedList<AnimationFrame>();

            //Pieces = new List<Piece>();

            //FrameSize = new Vector2((float)Texture.Width / ColumnCount, (float)Texture.Height / RowCount);
        }

        public override void LoadContent(Texture2D texture = null)
        {
            var position = Vector2.Zero;

            for (int i = 0, frameNumber = 0; i < RowCount; i++)
            {
                for (int k = 0; k < ColumnCount; k++, frameNumber++)
                {
                    Image image = new Image(Texture);

                    image.SetRowAndColumnCount(RowCount,ColumnCount);

                    var piece = image.Pieces.FirstOrDefault(p => p.ImageNumber == frameNumber);

                    //piece.SetSize(FrameSize);

                    //piece.SetPosition(position + new Vector2(position.X + k * FrameSize.X, position.Y + i * FrameSize.Y));

                    var frame = new AnimationFrame(this, frameNumber);

                    frame.SetTexture(piece.Texture);

                    frame.SetSize(Size);

                    frame.SetPosition(position + new Vector2(position.X + k * Size.X, position.Y + i * Size.Y));

                    AddFrame(frame);
                }
            }

            Frames.ToList().ForEach(f=>f.SetPosition(Position));

            ActiveFrame = Frames.First();

            //var firstFrame = Frames.First();
            //var lastFrame = Frames.Last();

            //for (int i = 0; i < Frames.Count; i++)
            //{
            //    if (firstFrame.Number != Frames[i].Number)
            //    {
            //        Frames[i].Previous = Fra
            //    }
            //}
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime = null)
        {
            //foreach (var piece in Pieces)
            //{
            //    piece.Update();
            //}

            TimeUntilNextFrame -= Global.GameTime.ElapsedGameTime.TotalSeconds;

            if (TimeUntilNextFrame <= 0)
            {
                var frame = Frames.Find(ActiveFrame);

                if (frame.Next != null)
                    ActiveFrame = frame.Next.Value;
                else
                    ActiveFrame = Frames.First();

                TimeUntilNextFrame = Interval;
            }

            ActiveFrame.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            //foreach (var piece in Pieces)
            //{
            //    piece.Draw();
            //}

            ActiveFrame.Draw();
        }

        //public Animation AddPiece(Piece piece)
        //{
        //    Pieces.Add(piece);

        //    return this;
        //}

        public Animation AddFrame(AnimationFrame frame)
        {
            Frames.AddLast(frame);

            return this;
        }

        //public Animation SetPieceSize(Vector2 pieceSize)
        //{
        //    foreach (var piece in Pieces)
        //    {
        //        piece.SetSize(pieceSize);
        //    }

        //    return this;
        //}

        public Animation SetSprite(Sprite sprite)
        {
            Sprite = sprite;

            Position = sprite.Position;

            Size = sprite.Size;

            return this;
        }

        public Animation PrepareMovement(List<int> numbers)
        {
            var list = Frames.Where(f => numbers.Contains(f.Number));

            return this;
        }
    }
}
