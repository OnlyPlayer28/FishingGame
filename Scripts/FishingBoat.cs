using Core;
using Core.Audio;
using Core.Components;
using Core.Debug;
using Core.Effects;
using Core.InventoryManagement;
using Fishing.Scripts.Food;
using Fishing.Scripts.Minigames;
using Fishing.Scripts.Scenes;
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
        Ascending,
        Stopped
    }
    internal class FishingBoat:IComponent,INameable
    {
        public  Sprite boat { get; set; }
        public Sprite fishHookedWarningSprite { get; set; }
        public string name { get; set; } = "fishingBoat";
        public Vector2 positionToMoveTo { get; private set; }
        public float moveSpeed { get; private set; } = 13.5f;
        public Sprite bobber { get; set; }
        private Vector2 bobberRestingPosition { get; set; } = Vector2.Zero;
        private float maxDepth { get; set; } = 120;
        public FishingState fishingState { get; set; } = FishingState.BoatIsMoving;
        private float bobberDescendingSpeed { get; set; } = 15f;
        private float bobberAscendingSpeed { get; set; } = 25f;
        private Vector2 minAndMaxWaitingTimeForFish { get; set; } = new Vector2(2, 6);
        private float waitingTime { get; set; } = 0f;
        public EventHandler<FishHookEventArgs> OnFishHookEvent { get; set; }
        public EventHandler OnFishAppearEvent { get; set; }

        private float currentTimeToCatchFish;

        public FishingMinigame minigame { get;private set; }

        private int waterLevel { get; set; } = 51;
        private bool stopBobberAfterReachingWaterlevel { get; set; } = false;

        private List<Fish> catchableFish { get; set; }

        public Random random;

        private Sprite fisherman {  get; set; }  

        private Line fishingLine { get; set; }
        public FishingBoat(Vector2 spawnPosition,Vector2 positionToMoveTo)
        {
            catchableFish = new List<Fish>();
            foreach (var item in Game1.itemRegistry)
            {
                if(item is Fish)
                {
                    catchableFish.Add((Fish)item);
                }
            }
            random = new Random();
            InputManager.OnMouseClickEvent += OnMouseClick;
            OnFishAppearEvent += OnFishAppear;
            boat = new Sprite(spawnPosition, new Vector2(25, 8), Vector2.Zero, "Art/Props/Boat", "boat",Layer.Entity).SetOrigin(Origin.BottomCenter);
            fisherman = new Sprite(spawnPosition, new Vector2(19, 10), new Vector2(14, 8), "Art/Props/Boat", layer: Layer.Entity-.00001f);
            fishHookedWarningSprite = new Sprite(Vector2.Zero, new Vector2(3, 9), new Vector2(10, 9), "Art/UI/UI",layer: Layer.Entity);
            this.positionToMoveTo = positionToMoveTo;       //SET BOAT TO FINISH
            bobber = new Sprite(Vector2.One, new Vector2(3, 4), new Vector2(0, 8), "Art/Props/Boat", layer: Layer.Entity);

            fishingLine = new Line(bobberRestingPosition, Vector2.Zero, Helper.HexToRgb("#d1d9e5"),layer:Layer.Entity);

            if(Game1.devMode)
                boat.position = this.positionToMoveTo;
            if (Game1.player != null)
            {
                OnFishHookEvent += Game1.player.OnFishHook;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            boat.LoadContent(Game1.contentManager);
            fishHookedWarningSprite.LoadContent(Game1.contentManager);
            bobber.LoadContent(Game1.contentManager);
            fisherman.LoadContent(Game1.contentManager);
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
                    ResetFishCatchTime();
                    fishingState = FishingState.WaitingForFish;
                    break;
                case FishingState.HookingFish:
                    minigame = new FishingMinigame(0, new Vector2(70, 40),GenerateFishToCatch());
                    OnFishHookEvent?.Invoke(this, new FishHookEventArgs { fishingMinigame = minigame });

                    fishingState = FishingState.FishingMinigame;

                    break;
                case FishingState.Stopped:
                    fishingState = FishingState.Ascending;
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
            return Inventory.GenerateLoot(catchableFish.ToArray());
        }

        public void OnFishAppear(Object o,EventArgs e)
        {

            AudioManager.PlayCue("water_splash_2");
            waitingTime = Game1.player.fishHookReactionTime;
            fishHookedWarningSprite.position = bobber.position + new Vector2(5, -4);
            fishHookedWarningSprite.AddEffect(new Shake(.05f, Game1.player.fishHookReactionTime, fishHookedWarningSprite));
            fishingState = FishingState.HookingFish;
        }
        public void Update(GameTime gameTime)
        {

            boat.Update(gameTime);

            boat.position= Helper.MoveToward(boat.position,positionToMoveTo,moveSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds);
            fisherman.position = boat.getPosition() + new Vector2(12, -7);
            if(boat.position== positionToMoveTo&&fishingState==FishingState.BoatIsMoving){  fishingState = FishingState.Default; }

            bobberRestingPosition = boat.getPosition() + new Vector2(boat.size.X +5, -(boat.size.Y/2)+3);

            fishingLine.position = fishingLine.position == bobberRestingPosition ? fishingLine.position : bobberRestingPosition;
            fishingLine.size = new Vector2(1,bobber.position.Y- bobberRestingPosition.Y);

            if (fishingState == FishingState.FishingMinigame)
            {
                minigame.Update(gameTime);
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
                    waitingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(waitingTime>= currentTimeToCatchFish)
                    {
                        OnFishAppearEvent?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case FishingState.FishingMinigame:
                    break;
                case FishingState.Ascending:
                    bobber.position -= new Vector2(0, bobberAscendingSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

                case FishingState.HookingFish:
                    waitingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    fishHookedWarningSprite.Update(gameTime);
                    if (waitingTime < -0)
                    {
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
                if(bobber.position.Y == maxDepth) { fishingState = FishingState.Stopped; }
                else { fishingState = FishingState.Default; }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*line.position = boat.position;
            line.Draw(spriteBatch);*/
            fisherman.Draw(spriteBatch);
            boat.Draw(spriteBatch);
            if (fishingState != FishingState.BoatIsMoving)
            {
                fishingLine.Draw(spriteBatch);
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
