using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public class Level : IXna
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Level Next { get; set; }

        public Level()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
 
        }


        public virtual void LoadContent(Texture2D texture = null)
        {

        }

        public virtual void Update(GameTime gameTime = null)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch = null)
        {

        }

        public virtual void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
