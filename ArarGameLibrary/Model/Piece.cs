using ArarGameLibrary.Effect;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public interface IPieceContainer
    {
        int RowCount { get; set; }

        int ColumnCount { get; set; }

        List<Piece> Pieces { get; set; }
    }

    public enum PieceType
    {
        Normal,
        Image,
        Nest,
        Empty
    }

    public enum PieceState
    {
        Selected,
        UnSelected
    }

    public class Piece : Sprite
    {
        //public static int MaxWidth = 50;
        //public static int MaxHeight = 50;

        public IPieceContainer Container { get; set; }

        public Font Font { get; set; }

        public int Number { get; set; }

        public int RowNumber { get; set; }

        public int ColumnNumber { get; set; }

        public int ImageNumber = -1;

        //public Color BackgroundColor = Color.White;

        //public Texture2D BackgroundTexture { get; set; }

        public Texture2D SelectedTexture { get; set; }

        public Vector2 StartingSize { get; set; }

        //public Graph Frame { get; set; }

        public Vector2 TemporaryPosition { get; set; }

        public PieceState State = PieceState.UnSelected;

        public List<PieceType> Types = new List<PieceType>();

        public bool IsEmpty
        {
            get
            {
                return HasType(PieceType.Empty);
            }
        }

        public bool IsImage
        {
            get
            {
                return HasType(PieceType.Image);
            }
        }

        public bool IsNest
        {
            get
            {
                return HasType(PieceType.Nest);
            }
        }

        public bool IsNormal
        {
            get
            {
                return HasType(PieceType.Normal);
            }
        }

        public Piece(int maxWidth, int maxHeight)
        {
            ClampManager.Add(new ClampObject("Size.X", 0, maxWidth), new ClampObject("Size.Y", 0, maxHeight));
        }

        public Piece() { }

        public override void Initialize()
        {
            base.Initialize();

            OnChangeRectangle += Piece_OnChangeRectangle;

            //var simpleShadowEffect = GetEffect<SimpleShadowEffect>();

            //if (simpleShadowEffect != null)
            //    simpleShadowEffect.SetOffset(new Vector2(-6, -6));

            GetEvent<SimpleShadowEffect>().SetWhenToInvoke(() =>
            {
                return State == PieceState.Selected;
            });


            GetEvent<PulsateEffect>().SetWhenToInvoke(() => 
            {
                return State == PieceState.Selected;
            });

            SetDrawMethodType(5);
        }

        private void Piece_OnChangeRectangle()
        {

            //Frame = new Graph(true).PopulatePoints(new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
            //                                        new Vector2(DestinationRectangle.Right, DestinationRectangle.Y),
            //                                        new Vector2(DestinationRectangle.Right, DestinationRectangle.Bottom),
            //                                        new Vector2(DestinationRectangle.Left, DestinationRectangle.Bottom))
            //    .PopulateLines(Color.Blue, 2f);

            //Frame.LoadContent();
        }

        public override void LoadContent(Texture2D texture = null)
        {
            //SelectedTexture = TextureManager.CreateTexture2DBySingleColor(Color.IndianRed,(int)Size.X, (int)Size.Y);

            //SetTexture(texture ?? TextureManager.CreateTexture2DByRandomColor(1,1));

            Font = new Font(fontFile: "Fonts/MenuFont", position: Position, fontColor: Color.Yellow, changeTextEvent: () => SetText());

            SetRectangle();

            //Frame = new Graph(true).PopulatePoints(new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
            //                                        new Vector2(DestinationRectangle.Right, DestinationRectangle.Y),
            //                                        new Vector2(DestinationRectangle.Right, DestinationRectangle.Bottom),
            //                                        new Vector2(DestinationRectangle.Left, DestinationRectangle.Bottom))
            //    .PopulateLines(Color.Blue,2f);

            //Frame.LoadContent();
        }

        public override void Update(GameTime gameTime = null)
        {
            var ee = GetEvent<SimpleShadowEffect>();

            if (State == PieceState.Selected)
            {
                SetLayerDepth(0.75f);

                //ShowSimpleShadow(true);

                //Pulsate(true);
            }
            else
            {
                SetStartingLayerDepth();

                //ShowSimpleShadow(false);

                //Pulsate(false);
            }

            if (Font != null)
            {
                Font.CalculateNewPosition(DestinationRectangle, null, true);

                Font.Update();
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            //if (IsImage)
            //    return;

            //if (BackgroundTexture != null && !IsNest)
            //{

            //    Global.SpriteBatch.Draw(BackgroundTexture, DestinationRectangle, SourceRectangle, BackgroundColor, Rotation, Origin, SpriteEffects, LayerDepth - 0.1f);
            //    //Global.SpriteBatch.Draw(BackgroundTexture, DestinationRectangle, BackgroundColor);
            //}

            base.Draw();

            if (Font != null)
            {
                Font.Draw();
            }

            //Frame.Draw();
        }

        public Piece MakeEmpty()
        {
            //SetBackgroundColor(Color.White);
            //SetBackgroundTexture(null);

            SetColor(Color.White);
            // SetTexture(TextureManager.CreateTexture2D("Textures/emptyPiece"));
            SetTexture(null);

            ClearTypes();
            AddType(PieceType.Empty);

            var simpleShadowEffect = GetEvent<SimpleShadowEffect>();

            if (simpleShadowEffect != null)
                simpleShadowEffect.End();

            return this;
        }

        public Piece MakeNest(Texture2D texture, Color color)
        {
          //  SetBackgroundTexture(null);

            SetTexture(texture);
            SetColor(color);

            ClearTypes();
            AddType(PieceType.Nest);

            return this;
        }

        public Piece MakeImage(int imageNumber,Texture2D texture) 
        {
            SetImageNumber(imageNumber);

            SetTexture(texture);
            SetColor(Color.White);

            ClearTypes();
            AddType(PieceType.Image);

            return this;
        }

        public Piece AddType(params PieceType[] pieceTypes)
        {
            foreach (var type in pieceTypes)
            {
                if (!Types.Any(t => t == type))
                    Types.Add(type);
            }

            return this;
        }

        public Piece RemoveType(PieceType pieceType)
        {
            Types.Remove(pieceType);

            return this;
        }

        public Piece ClearTypes()
        {
            Types.Clear();

            return this;
        }

        public bool HasType(PieceType type)
        {
            return Types.Any(t => t == type);
        }

        public Piece Select()
        {
            if (!HasType(PieceType.Empty) && !HasType(PieceType.Nest))
            {
                if (State == PieceState.UnSelected)
                {
                    State = PieceState.Selected;

                    SetPosition(new Vector2(Position.X, Position.Y) + new Vector2(5, 5));
                }
            }

            return this;
        }

        public Piece Unselect()
        {
            if(!HasType(PieceType.Empty) && !HasType(PieceType.Nest))
            {
                if(State == PieceState.Selected)
                {
                    State = PieceState.UnSelected;

                    SetPosition(new Vector2(Position.X, Position.Y) + new Vector2(-5, -5));
                }
            }

            return this;
        }

        public Piece SetContainer(IPieceContainer pieceContainer)
        {
            Container = pieceContainer;

            return this;
        }

        //public Piece SetBackgroundTextureByRandomColor()
        //{
        //    BackgroundTexture = TextureManager.CreateTexture2DByRandomColor((int)Size.X, (int)Size.Y);

        //    return this;
        //}

        //public Piece SetBackgroundColor(Color color)
        //{
        //    BackgroundColor = color;

        //    return this;
        //}

        //public Piece SetBackgroundTexture()
        //{
        //    BackgroundTexture = TextureManager.CreateTexture2DBySingleColor(BackgroundColor, (int)Size.X, (int)Size.Y);

        //    return this;
        //}

        //public Piece SetBackgroundTexture(Texture2D texture)
        //{
        //    BackgroundTexture = texture;

        //    return this;
        //}

        public new Piece SetColor(Color color)
        {
            base.SetColor(color);

            return this;
        }

        public Piece SetImageNumber(int imageNumber)
        {
            ImageNumber = imageNumber;

            return this;
        }

        public new Piece SetLayerDepth(float layerDepth)
        {
            base.SetLayerDepth(layerDepth);

            return this;
        }

        public Piece SetNumber(int number)
        {
            Number = number;

            return this;
        }

        public new Piece SetPosition(Vector2 position)
        {
            base.SetPosition(position);

            return this;
        }

        public new Piece SetSize(Vector2 size)
        {
            base.SetSize(size);

            return this;
        }

        public Piece SetState(PieceState state)
        {
            State = state;

            return this;
        }


        public string SetText(string text = null)
        {
            if (IsNest)
                return string.Empty;

            //return Number + " - " + string.Join(",", Types.Select(t=>t.ToString())); 

            return text ?? string.Format("({0},{1})={2}",RowNumber,ColumnNumber,Number);//$"({RowNumber},{ColumnNumber})={Number}";

            //return text ?? $"{ImageNumber}";
        }

        public new Piece SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            return this;
        }



        public Piece SetRowAndColumnNumber(int rowNumber,int columnNumber)
        {
            RowNumber = rowNumber;

            ColumnNumber = columnNumber;

            return this;
        }

        public Piece TopNeighbor
        {
            get
            {
                return Container.Pieces.FirstOrDefault(p => p.Number == (Number - Container.ColumnCount));
            }
        }

        public Piece BottomNeighbor
        {
            get
            {
                return Container.Pieces.FirstOrDefault(p => p.Number == (Number + Container.ColumnCount));
            }
        }

        public Piece LeftNeighbor
        {
            get
            {
                return ColumnNumber != 0 ? Container.Pieces.FirstOrDefault(p => p.Number == (Number - 1)) : null;
            }
        }

        public Piece RightNeighbor
        {
            get
            {
                return Container != null && ColumnNumber != Container.ColumnCount - 1 ? Container.Pieces.FirstOrDefault(p => p.Number == Number + 1) : null;
            }
        }

        public Piece Previous
        {
            get
            {
                return Container != null && Number > 0 ? Container.Pieces.FirstOrDefault(p => p.Number == Number - 1) : null;
            }
        }

        public Piece Next
        {
            get
            {
                return Container != null && Number < Container.Pieces.Max(p => p.Number) ? Container.Pieces.FirstOrDefault(p => p.Number == Number + 1) : null;
            }
        }

        public bool IsNeighborWith(Piece piece)
        {
            if ((TopNeighbor != null && TopNeighbor.Id == piece.Id) ||
                (BottomNeighbor != null && BottomNeighbor.Id == piece.Id) ||
                (LeftNeighbor != null && LeftNeighbor.Id == piece.Id) ||
                (RightNeighbor != null && RightNeighbor.Id == piece.Id))
                return true;
            else
                return false;
        }

        public static void Replace(Piece selectedPiece, Piece previouslySelectedPiece, List<Piece> Pieces)
        {
            var emptyPiece = Pieces.FirstOrDefault(p => p.RowNumber == selectedPiece.RowNumber && p.ColumnNumber == selectedPiece.ColumnNumber);
            var filledPiece = Pieces.FirstOrDefault(p => p.RowNumber == previouslySelectedPiece.RowNumber && p.ColumnNumber == previouslySelectedPiece.ColumnNumber);

            var tempPositon = emptyPiece.Position;
            emptyPiece.Position = filledPiece.Position;
            filledPiece.Position = tempPositon;

            var tempRowNumber = emptyPiece.RowNumber;
            var tempColumnNumber = emptyPiece.ColumnNumber;
            emptyPiece.SetRowAndColumnNumber(filledPiece.RowNumber, filledPiece.ColumnNumber);
            filledPiece.SetRowAndColumnNumber(tempRowNumber, tempColumnNumber);

            var tempNumber = emptyPiece.Number;
            emptyPiece.SetNumber(filledPiece.Number);
            filledPiece.SetNumber(tempNumber);
        }

        public static Piece[,] To2DPieceArray(Vector2 pieceSize, int rowCount, int columnCount, Vector2? startingPoint = null)
        {
            Piece[,] array = new Piece[rowCount, columnCount];

            for (int i = 0, number = 0; i < rowCount; i++)
            {
                for (int k = 0; k < columnCount; k++, number++)
                {
                    var piecePosition = new Vector2(pieceSize.X * k, pieceSize.Y * i);

                    piecePosition = startingPoint != null ? piecePosition + startingPoint.Value : piecePosition;

                    array[i, k] = new Piece()
                                        .SetPosition(piecePosition)
                                        .SetSize(pieceSize)
                                        .SetTexture(TextureManager.CreateTexture2DByRandomColor(1,1))
                                        .SetColor(Color.White)
                                        .SetRowAndColumnNumber(i, k)
                                        .SetNumber(number)
                                        .AddType(PieceType.Normal);
                }
            }

            return array;
        }

        public static Piece CreateACopy(Piece previous)
        {
            var newPiece = new Piece()
                                .SetColor(previous.Color)
                                .SetImageNumber(previous.ImageNumber)
                                .SetNumber(previous.Number)
                                .SetPosition(previous.Position)
                                .SetRowAndColumnNumber(previous.RowNumber, previous.ColumnNumber)
                                .SetSize(previous.Size)
                                .SetState(previous.State)
                                .SetTexture(previous.Texture)
                                .AddType(previous.Types.ToArray());
                                

            newPiece.LoadContent();

            return newPiece;
        }

    }
}
