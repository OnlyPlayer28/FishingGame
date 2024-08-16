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
using Core;
using Core.UI.Elements;
using Core.Debug;
using Fishing.Scripts.UI;

namespace Fishing.Scripts.Scenes
{
    internal class FishingScene : IScene
    {

        public  Sprite backgroundSprite { get; set; }
        public  FishingBoat boat { get; set; }

        public Canvas uiCanvas { get; set; }

        public FishingResultsScreen fishingResultsScreen { get; set; }

        public HUD hud { get; set; }

        public FishingScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            backgroundSprite = new Sprite(Vector2.Zero, new Vector2(128), Vector2.Zero, "Art/Backdrops/Ocean","oceanBackground",layer:1);
            boat = new FishingBoat(new Vector2(0, 43),new Vector2(50,43));
            uiCanvas = new Canvas("fishingSceneCanvas", true);
            fishingResultsScreen = new FishingResultsScreen(new Vector2(31, 35), new Vector2(66, 60), .5f, 0, uiCanvas);
            hud = new HUD(Vector2.Zero, Vector2.Zero, .05f, uiCanvas).SetActive(true);
            uiCanvas.AddUIELement(fishingResultsScreen);
            uiCanvas.AddUIELement(hud);
            components.Add(backgroundSprite);
            components.Add(boat);
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
                uiCanvas.Update(gameTime);
                hud.Update(gameTime);
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
