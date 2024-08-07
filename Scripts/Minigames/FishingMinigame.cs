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
using TopDownShooter.Core;
using Core;
using Fishing.Scripts.Scenes;

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
        private float baseSpeed { get; set; } = 20f;
        private float currentSpeed { get; set; } 
        private float timerDirectionChange { get; set; } = .5f;
        private int direction { get; set; } = 1;
        private int[] scoring { get; set; } = new int[] {1,2,3};
        public int score { get; private set; }

        public Vector2 position { get; private set; }
        public int fishID { get; private set; }

        public EventHandler<FishingMinigameEventArgs> OnFishCatchEvent { get; set; }

        private Line fishingCursor { get;set; }

        private Random random;

        public FishingMinigame( float difficulty, Vector2 position, int fishID)
        {
            currentSpeed = baseSpeed;
            random = new Random();
            this.difficulty = difficulty;
            this.position = position;

            fishingCursor = new Line(position+new Vector2(20,1), new Vector2(1, 3), Helper.HexToRgb("000000"), .1f);
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

            minigameArea.Draw(spriteBatch);
            fishingCursor.Draw(spriteBatch);
            for (int i = 0; i < score; i++)
            {
                progressionBar[i].Draw(spriteBatch);
            }
            backdropRect.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager contentManager)
        {
            minigameArea.LoadContent(contentManager);
        }
        public int CalculateScore()
        {
            if((fishingCursor.position.X >=position.X+12&& fishingCursor.position.X <= position.X + 17)|| (fishingCursor.position.X > position.X + 23 && fishingCursor.position.X <= position.X + 28)) { return scoring[0]; }
            if ((fishingCursor.position.X > position.X + 17 && fishingCursor.position.X <= position.X + 19) || (fishingCursor.position.X > position.X + 21 && fishingCursor.position.X <= position.X + 23)) { return scoring[1]; }
            if ((fishingCursor.position.X > position.X + 19 && fishingCursor.position.X <= position.X + 21)) { return scoring[2]; }
            return -1;
        }
        public void Update(GameTime gameTime)
        {
            difficulty = 1;
            timerDirectionChange -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timerDirectionChange <= 0)
            {
                direction *= -1;
                timerDirectionChange = (random.Next(15 - (int)(difficulty*10), 25 - (int)(difficulty * 10)))/10;
                currentSpeed = random.Next((int)baseSpeed,(int)( baseSpeed + 10));
            }
            if(fishingCursor.position.X <= minigameArea.position.X+1 || fishingCursor.position.X >= minigameArea.position.X + 38)
            {
                fishingCursor.position = new Vector2(Math.Clamp(fishingCursor.position.X, position.X + 1, position.X + 38), fishingCursor.position.Y);
                direction *= -1;
            }

            fishingCursor.position += new Vector2(currentSpeed * direction, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (InputManager.IsMouseButtonPressed(0))
            {
                score += CalculateScore();
                if(score < 0)
                {
                    score = 0;
                    ((FishingScene)Game1.stateManager.GetActiveGameState()).boat.fishingState = FishingState.WaitingForFish;
                }else  if(score >= progressionBar.Length)
                {
                    Console.WriteLine("Caught fish!!!!");
                    ((FishingScene)Game1.stateManager.GetActiveGameState()).boat.fishingState = FishingState.Ascending;
                }

            }
            minigameArea.Update(gameTime);
        }
    }
}
