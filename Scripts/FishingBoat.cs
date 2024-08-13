using Core;
using Core.Components;
using Core.Debug;
using Core.InventoryManagement;
using Fishing.Scripts.Food;
using Fishing.Scripts.Minigames;
using Fishing.Scripts.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts
{
    public class FishHookEventArgs : EventArgs
    {
        public FishingMinigame fishingMinigame;
    }
    public enum FishingState
    {
        BoatIsMoving,
        Default,
        Descending,
        WaitingForFish,
        HookingFish,
        FishingMinigame,
        FishingResults,
        Ascending
    }
    internal class FishingBoat:IComponent,ITaggable
    {
        public  Sprite boat { get; set; }
        public Sprite fishHookedWarningSprite { get; set; }
        public string name { get; set; } = "fishingBoat";
        public Vector2 positionToMoveTo { get; private set; }
        public float moveSpeed { get; private set; } = 13.5f;
        public Line bobber { get; set; }
        private Vector2 bobberRestingPosition { get; set; } = Vector2.Zero;
        private float maxDepth { get; set; } = 120;
        public FishingState fishingState { get; set; } = FishingState.BoatIsMoving;
        private float bobberDescendingSpeed { get; set; } = 15f;
        private float bobberAscendingSpeed { get; set; } = 25f;
        private Vector2 minAndMaxWaitingTimeForFish { get; set; } = new Vector2(2, 6);
        private float waitingTime { get; set; } = 0f;
        public EventHandler<FishHookEventArgs> OnFishHookEvent { get; set; }

        private float currentTimeToCatchFish;

        public FishingMinigame minigame { get;private set; }

        private int waterLevel { get; set; } = 51;
        private bool stopBobberAfterReachingWaterlevel { get; set; } = false;

        private Dictionary<Fish,float> catchableFish { get; set; }

        public Random random;


        Line line;
        public FishingBoat(Vector2 spawnPosition,Vector2 positionToMoveTo)
        {
            catchableFish = new Dictionary<Fish,float>();
            random = new Random();
            InputManager.OnMouseClickEvent += OnMouseClick;
            boat = new Sprite(spawnPosition, new Vector2(25, 8), Vector2.Zero, "Art/Props/Boat", "boat",.5f).SetOrigin(new Vector2(14,-7));
            fishHookedWarningSprite = new Sprite(Vector2.Zero, new Vector2(3, 9), new Vector2(10, 9), "Art/UI/UI", layer:.49f);
            this.positionToMoveTo = positionToMoveTo;
            bobber = new Line(Vector2.One,Vector2.One,Color.Red,.5f);

            line = new Line(Vector2.Zero, boat.size, Color.Green);

            boat.position = this.positionToMoveTo;
            foreach (var item in Game1.itemRegistry)
            {
                if(item is Fish fish)
                {
                    catchableFish.Add(fish,catchableFish.Values.Sum()+fish.rarity);
                }
            }
            if (Game1.player != null)
            {
                OnFishHookEvent += Game1.player.OnFishHook;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            boat.LoadContent(Game1.contentManager);
            fishHookedWarningSprite.LoadContent(Game1.contentManager);
        }
        public void OnMouseClick(Object sender,MouseInputEventArgs e)
        {

            if (InputManager.inputState != GameInputState.Gameplay) { return; }
            switch (fishingState)
            {
                case FishingState.Default:
                    fishingState = FishingState.Descending;
                    break;
                case FishingState.Descending:
                    if (bobber.position.Y >= waterLevel)
                    {
                        ResetFishCatchTime();
                        fishingState = FishingState.WaitingForFish;
                    }
                    else
                    {

                        stopBobberAfterReachingWaterlevel = true;
                    }
                    break;
                case FishingState.WaitingForFish:
                    fishingState = FishingState.Ascending;
                    break;
                case FishingState.FishingMinigame:
                    break;
                case FishingState.Ascending:
                    fishingState = FishingState.WaitingForFish;
                    break;
                case FishingState.HookingFish:
                    minigame = new FishingMinigame(0, new Vector2(70, 40), 0);
                    OnFishHookEvent?.Invoke(this, new FishHookEventArgs { fishingMinigame = minigame });

                    fishingState = FishingState.FishingMinigame;

                    break;
                case FishingState.FishingResults:
                    //fishingState = FishingState.WaitingForFish;
                    break;
                default:
                    break;
            }
        }
        public void ResetFishCatchTime()
        {
            currentTimeToCatchFish = Game1.player.CalculateCurrentFishCatchTime(minAndMaxWaitingTimeForFish);
        }
        public int GenerateFishToCatch()
        {
            return Inventory.GenerateLoot(catchableFish.Keys.ToArray());
        }
        public void Update(GameTime gameTime)
        {

            boat.Update(gameTime);

            boat.position= Helper.MoveToward(boat.position,positionToMoveTo,moveSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds);
            if(boat.position== positionToMoveTo&&fishingState==FishingState.BoatIsMoving){ fishingState = FishingState.Default; }

            bobberRestingPosition = boat.position + new Vector2(boat.size.X +5, -(boat.size.Y/2));

            if (fishingState == FishingState.FishingMinigame)
            {
                minigame.Update(gameTime);
            }
            //implement depth checking

   

            switch (fishingState)
            {
                case FishingState.Default:
                    if(bobber.position!= bobberRestingPosition) { bobber.position = bobberRestingPosition; }
                    break;
                case FishingState.Descending:
                    bobber.position += new Vector2(0, bobberDescendingSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case FishingState.WaitingForFish:
                    waitingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(waitingTime>= currentTimeToCatchFish)
                    {
                        Console.WriteLine("a fish has appeared!");
                        waitingTime = Game1.player.fishHookReactionTime;
                        fishingState = FishingState.HookingFish;
                        fishHookedWarningSprite.position = bobber.position+new Vector2(5,-4);
                    }
                    break;
                case FishingState.FishingMinigame:
                    break;
                case FishingState.Ascending:
                    bobber.position -= new Vector2(0, bobberAscendingSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case FishingState.HookingFish:
                    waitingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(waitingTime < -0)
                    {
                        Console.WriteLine("failed to hook fish :(");
                        waitingTime = 0;
                        fishingState = FishingState.WaitingForFish;
                    }
                    break;
                default:
                    break;
            }

            if(stopBobberAfterReachingWaterlevel&&bobber.position.Y > waterLevel)
            {
                ResetFishCatchTime();
                fishingState = FishingState.WaitingForFish;
                stopBobberAfterReachingWaterlevel = false;
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
            /*line.position = boat.position;
            line.Draw(spriteBatch);*/
            boat.Draw(spriteBatch);
            if (fishingState != FishingState.BoatIsMoving)
            {
                bobber.Draw(spriteBatch);
            }
            if(fishingState == FishingState.FishingMinigame)
            {
                minigame.Draw(spriteBatch);
            }
            if (fishingState == FishingState.HookingFish)
            {
                fishHookedWarningSprite.Draw(spriteBatch);
            }
        }
    }
}
