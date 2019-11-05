using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArarGameLibrary.Effect;

namespace ArarGameLibrary.ScreenManagement
{
    public class Container : Component
    {
        public List<Row> Rows
        {
            get
            {
                return GetChildAs<Row>(fetchAllDescandents: false).ToList();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public Container AddRow(Row row,float heightRatio)
        {
            AddChild(row);

            row.SetHeightRatio(heightRatio);

            return this;
        }

        public Container PrepareRows(bool isCentralized = false, string floatTo = null)
        {
            var maxHeight = Size.Y;
            var takenHeight = 0f;

            var rowList = Rows.Where(r => r.IsActive).ToList();

            if (rowList.Sum(r => r.HeightRatio) > 100)
            {
                var averageHeightPerRow = 100 / rowList.Count;

                rowList.ForEach(r => r.SetHeightRatio(averageHeightPerRow));
            }


            foreach (var row in rowList)
            {
                row.SetMargin(Padding);

                row.SetPosition(new Vector2(Position.X + row.Margin.X, Position.Y + row.Margin.Y + takenHeight));

                row.SetDistanceToParent();

                row.SetSize(new Vector2(Size.X, maxHeight * row.HeightRatio / 100));

                row.SetSizeDifferenceWithParent();

                if (row.Frame != null)
                    row.SetFrame(row.Frame.LinesColor);

                takenHeight += row.Size.Y;

                row.IncreaseLayerDepth();

                row.PrepareColumns(isCentralized, floatTo);
            }

            return this;
        }
    }
}
