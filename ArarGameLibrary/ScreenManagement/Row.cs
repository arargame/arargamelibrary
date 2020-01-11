using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public class Row : Component
    {
        public float HeightRatio { get; private set; }

        public List<Column> Columns
        {
            get
            {
                return GetChildAs<Column>(fetchAllDescandents: false).ToList();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime = null)
        {
            //foreach (var column in Columns)
            //{
            //    column.Update(gameTime);
            //}

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            //foreach (var column in Columns)
            //{
            //    column.Draw(spriteBatch);
            //}

            base.Draw(spriteBatch);
        }

        public override void SetStartingSize()
        {
            SetSize(new Vector2(100, 100));
        }


        public Row SetHeightRatio(float heightRatio)
        {
            HeightRatio = MathHelper.Clamp(heightRatio, 0, 100);

            return this;
        }

        public Row AddColumn(Column column,float widthRatio)
        {
            AddChild(column);

            column.SetWidthRatio(widthRatio);

            column.SetSizeRatioToParent(new Vector2(widthRatio, 100));

            return this;
        }

        public void PrepareColumns(bool isCentralized = false,string floatTo = null)
        {
            var containerWidth = Size.X;
            var takenWidth = 0f;

            if (Columns.Sum(c => c.WidthRatio) > 100)
            {
                var averageWidthPerColumn = (float)100 / Columns.Count;

                Columns.ForEach(r => r.SetWidthRatio(averageWidthPerColumn));
            }

            var totalEmptySpaceSize = containerWidth - Columns.Sum(c => c.WidthRatio) * containerWidth / 100;
            var paddingSize = 0f;

            foreach (var column in Columns)
            {
                var positionX = 0f;
                var positionY = column.Parent.Position.Y;
                var sizeX = containerWidth * column.WidthRatio / 100;
                var sizeY = column.Parent.Size.Y;

                switch (floatTo)
                {
                    case "left":

                        paddingSize = isCentralized ? totalEmptySpaceSize / Columns.Count : 0f;

                        positionX = column.Parent.Position.X + takenWidth;

                        takenWidth +=  sizeX + paddingSize;

                        break;

                    case "right":

                        paddingSize = isCentralized ? totalEmptySpaceSize / Columns.Count : totalEmptySpaceSize;

                        positionX = column.Parent.Position.X + paddingSize + takenWidth;

                        takenWidth += isCentralized ? sizeX + paddingSize : sizeX;

                        break;

                    default:

                        paddingSize = isCentralized ? totalEmptySpaceSize / (Columns.Count + 1) : totalEmptySpaceSize / 2;

                        positionX = column.Parent.Position.X + paddingSize + takenWidth;

                        takenWidth += isCentralized ? sizeX + paddingSize : sizeX;

                        break;
                }

                column.SetPosition(new Vector2(positionX,positionY));

                column.SetSize(new Vector2(sizeX,sizeY));
            }
        }
    }
}
