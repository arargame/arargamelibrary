using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArarGameLibrary.ScreenManagement
{
    public class Container : Component
    {
        public List<Row> Rows
        {
            get
            {
                return GetChildAs<Row>().ToList();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime = null)
        {
            //foreach (var row in Rows)
            //{
            //    row.Update();
            //}

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch = null)
        {
            //foreach (var row in Rows)
            //{
            //    row.Draw(spriteBatch);
            //}

           base.Draw(spriteBatch);
        }

        public Container AddRow(Row row, int drawOrder, float heightRatio)
        {
            AddChild(row);

            row.SetHeightRatio(heightRatio);

            return this;
        }

        public Container PrepareRows(bool isCentralized = false, string floatTo = null)
        {
            var maxHeight = Size.Y;
            var takenHeight = 0f;

            if (Rows.Sum(r => r.HeightRatio) > 100)
            {
                var averageHeightPerRow = 100 / Rows.Count;

                Rows.ForEach(r=>r.SetHeightRatio(averageHeightPerRow));
            }

            //for (int i = 0; i < Rows.Count; i++)
            //{
            //    var r = Rows[i];

            //    r.SetPosition(new Vector2(r.Parent.Position.X, r.Parent.Position.Y + takenHeight));
            //    r.SetSize(new Vector2(r.Parent.Size.X, maxHeight * r.HeightRatio / 100));

            //    if (r.Frame != null)
            //        r.SetFrame(r.Frame.LineColor);

            //    takenHeight += r.Size.Y;
            //}

            foreach (var row in Rows)
            {
                row.SetPosition(new Vector2(row.Parent.Position.X, row.Parent.Position.Y + takenHeight));
                row.SetSize(new Vector2(row.Parent.Size.X, maxHeight * row.HeightRatio / 100));

                if (row.Frame != null)
                    row.SetFrame(row.Frame.LineColor);

                takenHeight += row.Size.Y;

                row.SetLayerDepth(LayerDepth + 0.01f);

                row.PrepareColumns(isCentralized, floatTo);
            }

            //Rows.ForEach(r => r.PrepareColumns(isCentralized, floatTo));

            return this;
        }
    }
}
