using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
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
            playButton = new Button(new Vector2(128/2,128/2)-new Vector2(10,15), new Vector2(20, 10), Layer.UI, "playButton",onClickSound:"click").SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34")).SetHighlightColor(Color.Gray);
            playButton.SetButtonText("Play", Color.White, Game1.Font_24);
            uiCanvas.AddClickableElement(playButton);
            playButton.OnButtonClickEvent += OnMouseClick;
            
        }
        public void OnMouseClick(Object o,ButtonEventArgs e)
        {
            if (!isActive) { return; }
            if (e.buttonRef.name == "playButton")
            {
                Game1.stateManager.SetActive(true, "fishingScene");
            }
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
