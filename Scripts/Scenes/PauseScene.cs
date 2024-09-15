using Core;
using Core.Debug;
using Core.SceneManagement;
using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Scenes
{
    internal class PauseScene : IScene
    {
        private Rect backdrop { get; set; }
        private Canvas canvas { get; set; }
        private Text pausedText { get; set; }
        private Button settingsButton { get; set; }
        private Button exitButton { get; set; }
        public PauseScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            canvas = new Canvas("canvas", true);
            pausedText = new Text(new Vector2((128 / 2) -14, (128 / 2)-23 ), "-=Paused=-", Color.White, Game1.Font_24, layer: 0,isActive:true);
            settingsButton = new Button(new Vector2((128 / 2) - 15, (128 / 2) - 15), new Vector2(30, 10), 0, isActive: true,onClickSound:"click").SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34"));
            canvas.AddTextElement(new Text(new Vector2((128 / 2) - 10, (128 / 2)-14 ), "Settings", Color.White, Game1.Font_24, layer: 0, isActive: true));
            canvas.AddClickableElement(settingsButton);
            canvas.AddTextElement(pausedText);
            backdrop = new Rect(new Vector2((128/2)-25, (128/2)-25), new Vector2(50, 50), new Color(0,0,0,150), true,.0000001f);
            components.Add(backdrop);

            LoadContent(Game1.contentManager);
        }
        public override IScene SetActive(bool active)
        {
            canvas.isActive = active;
            return base.SetActive(active);
        }
        public override void LoadContent(ContentManager contentManager)
        {
            canvas.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }
        public override void Update(GameTime gameTime)
        {
            canvas.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            System.Diagnostics.Debug.WriteLine("text");
            canvas.DrawText(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            canvas.Draw(spriteBatch);
        }
    }
}
