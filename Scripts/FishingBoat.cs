using Core;
using Core.Components;
using Core.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownShooter.Core;

namespace Fishing.Scripts
{
    public enum FishingState
    {
        BoatIsMoving,
        Default,
        Descending,
        WaitingForFish,
        FishingMinigame,
        Ascending
    }
    internal class FishingBoat:IComponent,ITaggable
    {
        public  Sprite boat { get; set; }
        public string name { get; set; } = "fishingBoat";

        public Vector2 positionToMoveTo { get; private set; }

        public float moveSpeed { get; private set; } = 13.5f;

        public Line bobber { get; set; }
        private Vector2 bobberRestingPosition { get; set; } = Vector2.Zero;

        private float maxDepth { get; set; } = 120;
        public FishingState fishingState { get; set; } = FishingState.BoatIsMoving;

        private float bobberDescendingSpeed { get; set; } = 15f;
        private float bobberAscendingSpeed { get; set; } = 25f;

        public FishingBoat(Vector2 spawnPosition,Vector2 positionToMoveTo)
        {
            boat = new Sprite(spawnPosition, new Vector2(25, 8), Vector2.Zero, "Art/Props/Boat", "boat");
            this.positionToMoveTo = positionToMoveTo;
            bobber = new Line(Vector2.One,Vector2.One,Color.Red);
        }

        public void LoadContent(ContentManager contentManager)
        {
            boat.LoadContent(Game1.contentManager);
        }

        public void Update(GameTime gameTime)
        {
            
            boat.position = Helper.MoveToward(boat.position,positionToMoveTo,moveSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds);
            if(boat.position== positionToMoveTo&&fishingState==FishingState.BoatIsMoving){ fishingState = FishingState.Default; }

            boat.Update(gameTime);
            bobberRestingPosition = boat.position + new Vector2(boat.size.X + 3, -boat.size.Y);

            if(InputManager.IsMouseButtonPressed(0))
            {
                switch (fishingState)
                {
                    case FishingState.Default:
                        fishingState = FishingState.Descending;
                        break;
                    case FishingState.Descending:
                        fishingState = FishingState.WaitingForFish;
                        break;
                    case FishingState.WaitingForFish:
                        fishingState = FishingState.Ascending;
                        break;
                    case FishingState.FishingMinigame:
                        break;
                    case FishingState.Ascending:
                        fishingState = FishingState.WaitingForFish;
                        break;
                    default:
                        break;
                }
            }

            switch (fishingState)
            {
                case FishingState.Default:
                    if(bobber.position!= bobberRestingPosition) { bobber.position = bobberRestingPosition; }
                    break;
                case FishingState.Descending:
                    bobber.position += new Vector2(0, bobberDescendingSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case FishingState.WaitingForFish:
                case FishingState.FishingMinigame:
                    break;
                case FishingState.Ascending:
                    bobber.position -= new Vector2(0, bobberAscendingSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                default:
                    break;
            }
            if((bobber.position.Y< bobberRestingPosition.Y||bobber.position.Y > maxDepth)&&fishingState!=FishingState.BoatIsMoving) 
            { 
                bobber.position = new Vector2(bobberRestingPosition.X, Math.Clamp(bobber.position.Y, bobberRestingPosition.Y, maxDepth)); 
                if(bobber.position.Y == maxDepth) { fishingState = FishingState.WaitingForFish; }
                else { fishingState = FishingState.Default; }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            boat.Draw(spriteBatch);
            if (fishingState != FishingState.BoatIsMoving)
            {
                bobber.Draw(spriteBatch);
            }
        }
    }
}
