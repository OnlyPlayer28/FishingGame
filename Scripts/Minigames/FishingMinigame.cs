using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Debug;

namespace Fishing.Scripts.Minigames
{
    public class FishingMinigameEventArgs : EventArgs
    {
        public int fishID;
    }

    public class FishingMinigame : IComponent
    {
        public Sprite minigameArea { get; set; }
        public Line[] progressionBar { get; set; }= new Line[5];
        public Rect backdropRect { get; set; }

        private float difficulty { get; set; }
        private float currentSpeed { get; set; }
        private float timerDirectionChange { get; set; }
        private int[] scoring { get; set; } = new int[] {1,2,3};
        public int score { get; private set; }

        public Vector2 position { get; private set; }
        public int fishID { get; private set; }

        public EventHandler<FishingMinigameEventArgs> OnFishCatchEvent { get; set; }

        public FishingMinigame( float difficulty, Vector2 position, int fishID)
        {
            this.difficulty = difficulty;
            this.position = position;
            this.minigameArea = new Sprite(this.position, new Vector2(40, 9), Vector2.Zero, "Art/UI/UI", layer: .11f);
            for (int i = 0; i < 5; i++)
            {
                progressionBar[i] = new Line(position + new Vector2(13, 7) + new Vector2((i * 2)+i*1, 0), new Vector2(2, 1), Game1.gold,.1f);
            }
            this.backdropRect = new Rect(position - new Vector2(10, 10), new Vector2(60, 40), new Color(0, 0, 0,125), true,.12f);
            backdropRect.layer = 0.01f;
            LoadContent(Game1.contentManager);
            this.fishID = fishID;


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Console.WriteLine(progressionBar[0].layer);
            minigameArea.Draw(spriteBatch);

            for (int i = 0; i < 5; i++)
            {
                progressionBar[i].Draw(spriteBatch);
            }
            backdropRect.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager contentManager)
        {
            minigameArea.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            minigameArea.Update(gameTime);
        }
    }
}
