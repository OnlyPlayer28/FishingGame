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
using Core.Audio;

namespace Fishing.Scripts.Scenes
{
    internal class FishingScene : IScene
    {

        public  Sprite backgroundSprite { get; set; }
        public  FishingBoat boat { get; set; }

        public Canvas uiCanvas { get; set; }

        public FishingResultsScreen fishingResultsScreen { get; set; }

        public HUD hud { get; set; }

        private Sprite[] oceanFloorDecorations { get; set; }
        private Sprite[] clouds { get; set; }
        public Button goToRestaurantButton { get; set; }
        private float cloudMoveSpeed { get; set; } = 5f;

        private int minCloudYPos = 20;
        private int maxCloudYPos = 0;
        public FishingScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            uiCanvas = new Canvas("fishingSceneCanvas", true);
            hud = new HUD(Vector2.Zero, Vector2.Zero, .05f, uiCanvas).SetActive(true);
            backgroundSprite = new Sprite(Vector2.Zero, new Vector2(128), Vector2.Zero, "Art/Backdrops/Ocean","oceanBackground",layer:1);
            boat = new FishingBoat(new Vector2(0, 51),new Vector2(50,51));
            fishingResultsScreen = new FishingResultsScreen(new Vector2(31, 35), new Vector2(66, 60), .5f, 0, uiCanvas);
            fishingResultsScreen.name = "fishingResultsScreen";
           // uiCanvas.AddUIELement(fishingResultsScreen);
            uiCanvas.AddUIELement(hud);
            components.Add(backgroundSprite);
            components.Add(boat);
            goToRestaurantButton = new Button(new Sprite(new Vector2(116, 116), new Vector2(10, 10), new Vector2(18, 19), "Art/UI/UI", layer: .1f),true,"click")
                .SetOnButtonCLickAction(OnRestaurantButtonClick);
            uiCanvas.AddClickableElement(goToRestaurantButton);
            GenerateSeafloor();
            GenerateClouds(6);
        }
        public override IScene SetActive(bool active)
        {
            if (active) { AudioManager.PlaySong("ocean_waves", true); ; }
            return base.SetActive(active);
        }
        public void OnRestaurantButtonClick()
        {
            if(boat.fishingState!= FishingState.BoatIsMoving&& boat.fishingState !=FishingState.FishingMinigame&& boat.fishingState !=FishingState.FishingResults)
                Game1.stateManager.SetActive(true, "restaurantScene");
        }
        private void GenerateSeafloor(int maxSeaweed = 10,int maxRocks = 4)
        {
            Rectangle[] seaweeDimensions = new Rectangle[] {new Rectangle(3,14,8,17),new Rectangle(8,14,7,13),new Rectangle(15,14,7,7),new Rectangle(22,14,3,10),new Rectangle(25,14,5,14) };
            Rectangle[] rockDimensions = new Rectangle[] { new Rectangle(30, 14, 3, 2), new Rectangle(33, 14, 5, 3) };
            oceanFloorDecorations = new Sprite[maxSeaweed + maxRocks];
            int currentSeaweed = 0;
            int currentRocks = 0;
            Vector2 positionToSpawn = new Vector2(9, 125);
            for (int i = 0; i < oceanFloorDecorations.Length; i++)
            {
                float randomNum = (float)ReferenceHolder.random.NextDouble();
                
                if (randomNum <= .65f&&currentSeaweed<maxSeaweed)
                {
                    oceanFloorDecorations[i] = new Sprite(positionToSpawn, seaweeDimensions[ReferenceHolder.random.Next(0, seaweeDimensions.Length )], "Art/Props/enviroment", $"seaweed_{currentSeaweed}", .01f).SetOrigin(Origin.BottomCenter);
                    currentSeaweed++;
                }else if(randomNum > .65f&&currentRocks < maxRocks)
                {
                    oceanFloorDecorations[i] = new Sprite(positionToSpawn, rockDimensions[ReferenceHolder.random.Next(0, rockDimensions.Length )], "Art/Props/enviroment", $"rock_{currentRocks}", .01f).SetOrigin(Origin.BottomCenter);
                    currentRocks++;
                }
                else
                {
                    Game1.player.money++;
                    oceanFloorDecorations[i] = currentSeaweed < maxSeaweed ? new Sprite(positionToSpawn, seaweeDimensions[ReferenceHolder.random.Next(0, seaweeDimensions.Length )], "Art/Props/enviroment", $"seaweed_{currentSeaweed+=1}", .01f).SetOrigin(Origin.BottomCenter) : new Sprite(positionToSpawn, rockDimensions[ReferenceHolder.random.Next(0, rockDimensions.Length )], "Art/Props/enviroment", $"rock_{currentRocks+=1}", .01f).SetOrigin(Origin.BottomCenter);
                }

                positionToSpawn += new Vector2(oceanFloorDecorations[i].size.X + ReferenceHolder.random.Next(1, 5),0);
                positionToSpawn = new Vector2(positionToSpawn.X, 125 + ReferenceHolder.random.Next(0, 2));

                oceanFloorDecorations[i].LoadContent(Game1.contentManager);
            }


        }

        private void GenerateClouds (int cloudCount)
        {
            Rectangle[] cloudBounds = new Rectangle[] { new Rectangle(0, 0, 27, 14) ,new Rectangle(27,0,16,10),new Rectangle(43,0,15,10)};

            clouds = new Sprite[cloudCount];
            for (int i = 0; i < cloudCount; i++)
            {
                clouds[i] = new Sprite(new Vector2(-20+((i * 25)+ReferenceHolder.random.Next(-3,4)), ReferenceHolder.random.Next(maxCloudYPos,minCloudYPos)), cloudBounds[ReferenceHolder.random.Next(0, 3)], "Art/Props/enviroment", "cloud", .01f);
                clouds[i].LoadContent(Game1.contentManager);
                clouds[i].SetTransparency(.95f);
                components.Add(clouds[i]);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isActive||isDrawing)
            {
                foreach (var item in oceanFloorDecorations)
                {
                    item.Draw(spriteBatch);
                }
                components.ForEach(p => p.Draw(spriteBatch));
            }
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            if(isActive||isDrawing)
            {
                fishingResultsScreen.Draw(spriteBatch);
                uiCanvas.Draw(spriteBatch);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            components.ForEach(p=>p.LoadContent(contentManager));
            goToRestaurantButton.LoadContent(contentManager);

        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (InputManager.AreKeysBeingPressedDown(keys: Microsoft.Xna.Framework.Input.Keys.M))
                {
                    GenerateSeafloor();
                }
                fishingResultsScreen.Update(gameTime);
                uiCanvas.Update(gameTime);
                hud.Update(gameTime);
                components.ForEach(p => p.Update(gameTime));

                foreach (var item in clouds)
                {
                    item.position += new Vector2(cloudMoveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(item.position.X +item.size.X >128+item.size.X)
                    {
                        item.position = new Vector2(-item.size.X, ReferenceHolder.random.Next(maxCloudYPos, minCloudYPos));
                    }
                }
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
