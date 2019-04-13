using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleMeWindowsProject.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Model
{
    public interface IPieceContainer
    {
        int RowCount { get; set; }

        int ColumnCount { get; set; }

        //Piece[,] Pieces { get; set; }
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
        public static int MaxWidth = 50;
        public static int MaxHeight = 50;

        public IPieceContainer Container { get; set; }

        public FontManager FontManager { get; set; }

        public int Number { get; set; }

        public int RowNumber { get; set; }

        public int ColumnNumber { get; set; }

        public int ImageNumber = -1;

        public Color BackgroundColor { get; set; }

        public Texture2D BackgroundTexture { get; set; }

        public Texture2D SelectedTexture { get; set; }

        public Vector2 StartingSize { get; set; }

        public Graph Frame { get; set; }

        public PieceState State { get; set; }

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

        public Piece() { }

        public Piece(Vector2 startingPosition,
            Vector2 startingSize,
            PieceState pieceState = PieceState.UnSelected,
            params PieceType[] pieceTypes)
        {
            SetPosition(startingPosition);
            SetSize(startingSize);
            State = pieceState;
            AddType(pieceTypes);
        }

        public override void Initialize()
        {
            base.Initialize();

            OnChangeRectangle += Piece_OnChangeRectangle;
        }

        private void Piece_OnChangeRectangle()
        {

            Frame = new Graph(true).PopulatePoints(new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
                                                    new Vector2(DestinationRectangle.Right, DestinationRectangle.Y),
                                                    new Vector2(DestinationRectangle.Right, DestinationRectangle.Bottom),
                                                    new Vector2(DestinationRectangle.Left, DestinationRectangle.Bottom))
                .PopulateLines(Color.Blue, 2f);

            Frame.LoadContent();
        }

        public override void LoadContent()
        {
            if (BackgroundTexture == null)
            {
                //var color = new Color(Global.Random.Next(0, 255), Global.Random.Next(0, 255), Global.Random.Next(0, 255));
                //var color = new Color(74, 74, 31, 127);
                var color = new Color(178,147,114);

                SetBackgroundColor(color);
                SetBackgroundTexture();
                SetBackgroundColor(Color.White * 0.8f);
            }

            SelectedTexture = TextureManager.CreateTexture2DBySingleColor(Color.IndianRed,(int)Size.X, (int)Size.Y);

            FontManager = new FontManager("Fonts/MenuFont", "", Position, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f, Vector2.Zero, () => SetText());

            SetRectangle();

            Frame = new Graph(true).PopulatePoints(new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
                                                    new Vector2(DestinationRectangle.Right, DestinationRectangle.Y),
                                                    new Vector2(DestinationRectangle.Right, DestinationRectangle.Bottom),
                                                    new Vector2(DestinationRectangle.Left, DestinationRectangle.Bottom))
                .PopulateLines(Color.Blue,2f);

            Frame.LoadContent();
        }

        public override void Update()
        {
            FontManager.CalculateCenterVector2(DestinationRectangle);

            FontManager.Update();

            base.Update();
        }

        public override void Draw()
        {
            //if (IsImage)
            //    return;

            if (BackgroundTexture != null)
                Global.SpriteBatch.Draw(BackgroundTexture, DestinationRectangle, BackgroundColor);

            if (Texture != null)
                base.Draw();

            if (!IsEmpty)
            {
                if (State == PieceState.Selected)
                    Global.SpriteBatch.Draw(SelectedTexture, DestinationRectangle, Color);
            }

            FontManager.Draw();

            Frame.Draw();
        }

        public Piece MakeEmpty()
        {
            //BackgroundColor = new Color((byte)0, (byte)0, (byte)0, (byte)(0.5 * 255));

            //SetBackgroundTexture();

            SetBackgroundColor(Color.White);
            SetBackgroundTexture(null);

            //SetMidLayerTexture(null);


            //SetColor(Color.White * 0.5f);
            SetColor(Color.White);
            SetTexture(TextureManager.CreateTexture2D("Textures/emptyPiece"));
            //SetTexture(TextureManager.CreateTexture2DBySingleColor(Color.Black, (int)Size.X, (int)Size.Y));

            ClearTypes();
            AddType(PieceType.Empty);

            Frame.ChangeAllLinesColor(Color.Red);

            return this;
        }

        public Piece MakeNest(Texture2D backgroundTexture, Color backgroundColor, Texture2D texture, Color color)
        {
            SetBackgroundColor(backgroundColor);
            SetBackgroundTexture(backgroundTexture);

            //SetImageNumber(imageNumber);
            SetColor(color);
            SetTexture(null);

            ClearTypes();
            AddType(PieceType.Nest);

            return this;
        }

        public Piece MakeImage(int imageNumber,Texture2D texture) 
        {
            SetTexture(texture);

            SetImageNumber(imageNumber);

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

        public Piece SetContainer(IPieceContainer pieceContainer)
        {
            Container = pieceContainer;

            return this;
        }

        public Piece SetBackgroundColor(Color color)
        {
            BackgroundColor = color;

            return this;
        }

        public Piece SetBackgroundTexture()
        {
            BackgroundTexture = TextureManager.CreateTexture2DBySingleColor(BackgroundColor, (int)Size.X, (int)Size.Y);

            return this;
        }

        public Piece SetBackgroundTexture(Texture2D texture)
        {
            BackgroundTexture = texture;

            return this;
        }

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


        public Piece SetNumber(int number)
        {
            Number = number;

            //FontManager.SetText(Number.ToString());

            return this;
        }


        public string SetText(string text = null)
        {
            return text ?? $"({RowNumber},{ColumnNumber})={Number}";

            //return text ?? $"{ImageNumber}";
        }

        public new Piece SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            return this;
        }

        public Piece Select()
        {
            if (!HasType(PieceType.Empty) && !HasType(PieceType.Nest))
            {
                State = PieceState.Selected;

                //SetColor(Color.Red);
            }

            return this;
        }

        public Piece UnSelect()
        {
            State = PieceState.UnSelected;

            //SetColor(Color.White);

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
            //return pieceList.FirstOrDefault(p => p.Number == (Number - Container.ColumnCount));
            get
            {
                return Container.Pieces.FirstOrDefault(p => p.Number == (Number - Container.ColumnCount));
            }
        }

        public Piece BottomNeighbor
        {
            //return pieceList.FirstOrDefault(p => p.Number == (Number + Container.ColumnCount));
            get
            {
                return Container.Pieces.FirstOrDefault(p => p.Number == (Number + Container.ColumnCount));
            }
        }

        public Piece LeftNeighbor
        {
            //return ColumnNumber!=0 ? pieceList.FirstOrDefault(p => p.Number == (Number - 1)) : null;
            get
            {
                return ColumnNumber != 0 ? Container.Pieces.FirstOrDefault(p => p.Number == (Number - 1)) : null;
            }
        }

        ///?????
        public Piece RightNeighbor
        {
            //return Container!=null && ColumnNumber!=Container.ColumnCount-1 ? pieceList.FirstOrDefault(p => p.Number == (Number + 1)) : null;

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

        public bool IsNeighborWith(List<Piece> pieceList,Piece piece)
        {
            if ((TopNeighbor != null && TopNeighbor.Id == piece.Id) ||
                (BottomNeighbor != null && BottomNeighbor.Id == piece.Id) ||
                (LeftNeighbor != null && LeftNeighbor.Id == piece.Id) ||
                (RightNeighbor != null && RightNeighbor.Id == piece.Id))
                return true;
            else 
                return false;
        }

        public static void Replace(Piece selectedPiece,Piece previouslySelectedPiece,List<Piece> Pieces)
        {
            //var emptyPiece = Pieces[selectedPiece.RowNumber, selectedPiece.ColumnNumber];
            //var tempPiece = emptyPiece;
            //var filledPiece = Pieces[previouslySelectedPiece.RowNumber, previouslySelectedPiece.ColumnNumber];
            //emptyPiece = filledPiece;
            //filledPiece = tempPiece;

            var emptyPiece = Pieces.FirstOrDefault(p => p.RowNumber == selectedPiece.RowNumber && p.ColumnNumber == selectedPiece.ColumnNumber);
            //Pieces[selectedPiece.RowNumber, selectedPiece.ColumnNumber].MemberwiseClone() as Piece;

            var filledPiece = Pieces.FirstOrDefault(p => p.RowNumber == previouslySelectedPiece.RowNumber && p.ColumnNumber == previouslySelectedPiece.ColumnNumber);
                //Pieces[previouslySelectedPiece.RowNumber, previouslySelectedPiece.ColumnNumber].MemberwiseClone() as Piece;

            
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

            emptyPiece.SetText();
            filledPiece.SetText();

            //if (filledPiece.IsNest)
            //{
            //    var tempTexture = emptyPiece.Texture;

            //    emptyPiece.SetTexture(filledPiece.Texture);

            //    filledPiece.SetTexture(tempTexture);
            //}

            //Pieces[emptyPiece.RowNumber, emptyPiece.ColumnNumber] = emptyPiece;
            //Pieces[filledPiece.RowNumber, filledPiece.ColumnNumber] = filledPiece;
        }

        public static Piece[,] To2DPieceArray(Vector2 pieceSize, int rowCount, int columnCount)
        {
            Piece[,] array = new Piece[rowCount, columnCount];

            for (int i = 0, number = 0; i < rowCount; i++)
            {
                for (int k = 0; k < columnCount; k++, number++)
                {
                    array[i, k] = new Piece(new Vector2(pieceSize.X * k, pieceSize.Y * i), pieceSize)
                                        .SetBackgroundColor(Color.White)
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
            var newPiece = new Piece(previous.Position, previous.Size,previous.State, previous.Types.ToArray())
                                .SetBackgroundColor(previous.BackgroundColor)
                                .SetBackgroundTexture(previous.BackgroundTexture)
                                .SetColor(previous.Color)
                                .SetTexture(previous.Texture)
                                .SetRowAndColumnNumber(previous.RowNumber,previous.ColumnNumber)
                                .SetNumber(previous.Number)
                                .SetImageNumber(previous.ImageNumber);

            newPiece.LoadContent();

            return newPiece;
        }

    }
}
