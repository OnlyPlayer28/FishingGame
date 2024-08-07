using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Components;
using Core.SceneManagement;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooter.Core;

namespace Fishing.Scripts.Scenes
{
    internal class FishingScene : IScene
    {

        public  Sprite backgroundSprite { get; set; }
        public  FishingBoat boat { get; set; }

        public Canvas uiCanvas { get; set; }

        public FishingScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            backgroundSprite = new Sprite(Vector2.Zero, new Vector2(128), Vector2.Zero, "Art/Backdrops/Ocean","oceanBackground",layer:1);
            boat = new FishingBoat(new Vector2(0, 43),new Vector2(50,43));
            uiCanvas = new Canvas("fishingSceneCanvas", true);
            components.Add(backgroundSprite);
            components.Add(boat);

            uiCanvas.AddTextElement(new Text(new Vector2(50,50),$"{Helper.HexToInt("FF")}",Color.Red,Game1.Font_10));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isActive||isDrawing)
            {
                components.ForEach(p => p.Draw(spriteBatch));
            }
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            if(isActive||isDrawing)
            {
                uiCanvas.Draw(spriteBatch);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            components.ForEach(p=>p.LoadContent(contentManager));
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                components.ForEach(p => p.Update(gameTime));
            }
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            if (isActive || isDrawing)
            {
                uiCanvas.DrawText(spriteBatch);
            }
        }
    }
}
