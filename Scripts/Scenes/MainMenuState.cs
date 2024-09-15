using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.SceneManagement;
using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fishing.Scripts.Scenes
{
    internal class MainMenuState : IScene
    {
        private Canvas uiCanvas {  get; set; }
        private Button playButton { get; set; }
        public MainMenuState(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            uiCanvas = new Canvas("canvas", true);
            playButton = new Button(Vector2.Zero, new Vector2(20, 10), 0).SetSimpleSprite(Color.AliceBlue,Color.Aqua).SetHighlightColor(Color.Red);
            uiCanvas.AddClickableElement(playButton);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            uiCanvas.LoadContent(contentManager);
            playButton.LoadContent(contentManager);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            uiCanvas.DrawText(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            uiCanvas.Draw(spriteBatch);
        }


        public override void Update(GameTime gameTime)
        {
            uiCanvas.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
