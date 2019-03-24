using Microsoft.Xna.Framework;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Manager
{
    public class Level : IXna
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Level Next { get; set; }

        public List<Sprite> Sprites = new List<Sprite>();

        Board Board { get; set; }

        public Level()
        {
            Initialize();
        }

        public void Initialize()
        {
            Board = new Board(8,8);

            //Box box = new Box(new Vector2(100,100),new Vector2(100,100),Color.Bisque);

            //Sprites.Add(box);
        }

        public void Load()
        {
            Board.LoadContent();

            foreach (var sprite in Sprites)
            {
                sprite.LoadContent();
            }
        }

        public void Update()
        {
            Board.Update();

            foreach (var sprite in Sprites)
            {
                sprite.Update();
            }
        }

        public void Draw()
        {
            Board.Draw();

            foreach (var sprite in Sprites)
            {
                sprite.Draw();
            }
        }


        public void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }
    }
}
