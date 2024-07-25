using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Components;
using Core.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fishing.Scripts.GameStates
{
    internal class FishingState : GameState
    {

        public  Sprite backgroundSprite { get; set; }
        public  FishingBoat boat { get; set; }
        public FishingState(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            backgroundSprite = new Sprite(Vector2.Zero, new Vector2(128), Vector2.Zero, "Art/Backdrops/Ocean","oceanBackground");
            boat = new FishingBoat(new Vector2(-2, 45),new Vector2(50,45));
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
    }
}
