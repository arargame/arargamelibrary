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

        //public override void SetStartingSize()
        //{
        //    SetSize(new Vector2(200, 200));
        //}


        public Container AddRow(Row row,float heightRatio)
        {
            AddChild(row);

            //if (row.SizeDifferenceWithParent.Y == 1)
            //{
            //    row.SizeDifferenceWithParent = new Vector2(row.SizeDifferenceWithParent.X, heightRatio / 100);
            //}

            row.SetHeightRatio(heightRatio);

            row.SetSizeDifferenceRatioWithParent(new Vector2(100, heightRatio));

            return this;
        }

        public Container PrepareRows(bool isCentralized = false, string floatTo = null, float distanceAmongRows = 0f)
        {
            var maxHeight = Size.Y;
            var takenHeight = 0f + distanceAmongRows;

            var rowList = Rows.Where(r => r.IsActive).ToList();

            if (rowList.Sum(r => r.HeightRatio) + distanceAmongRows > 100)
            {
                var averageHeightPerRow = 100 / (distanceAmongRows == 0f ? rowList.Count : rowList.Count + 1);

                rowList.ForEach(r => r.SetHeightRatio(averageHeightPerRow));
            }


            foreach (var row in rowList)
            {
                var rowPosition = new Vector2(Position.X, Position.Y + takenHeight);

                row.SetPosition(rowPosition);

                row.SetSizeDifferenceRatioWithParent(new Vector2(100, row.HeightRatio));

                row.SetSize(new Vector2(Size.X, maxHeight * row.HeightRatio / 100));

                if (row.Frame != null)
                    row.SetFrame(row.Frame.LinesColor);

                takenHeight += row.Size.Y + distanceAmongRows;

                row.PrepareColumns(isCentralized, floatTo);
            }

            return this;
        }
    }
}
