using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fishing.Scripts.Scenes
{
    internal class MainMenuState : IScene
    {
        public MainMenuState(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {

        }

        public override void LoadContent(ContentManager contentManager)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
