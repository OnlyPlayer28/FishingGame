using Core.Cameras;
using Core.SceneManagement;
using Fishing.Scripts.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Core;
using Core.Debug;
using System.Globalization;
using Fishing.Scripts;
using Core.Components;
using System.Collections.Generic;
using Fishing.Core;
using System.Linq;
using Core.InventoryManagement;
using Fishing.Scripts.Food;

namespace Fishing
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        internal static ContentManager contentManager;


        private MainMenuState mainMenuState;
        private FishingScene fishingState;
        internal static SceneManagement stateManager;

        internal static CameraManager cameraManager;
        internal static Vector2 resolution = new Vector2(128*5, 128*5);

        static SpriteFont spriteFont;

        public static float FPS { get; private set; }

        public static Player player { get; private set; }

        public static SpriteFont Font_10;

        public static Color gold = new Color(192, 139, 59);

        public static bool isFocused { get; private set; }


        public static List<IAddableToInventory> itemRegistry;

        public Game1()
        {
            itemRegistry = new List<IAddableToInventory>();

            _graphics = new GraphicsDeviceManager(this);
            //depthBuffer = new RenderTarget2D(_graphics, (int)resolution.X, (int)resolution.Y);
            _graphics.PreferredBackBufferWidth = (int)resolution.X; _graphics.PreferredBackBufferHeight =(int)resolution.Y;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
            CameraManager.AddCamera(new Camera(Vector2.Zero, 5, new Vector2(1600, 900), "mainCamera"));
            CameraManager.SetCurrentCamera("mainCamera");

            string textFile = File.ReadAllText(contentManager.RootDirectory + "/Art/test.txt");
            Console.WriteLine(textFile);
            Console.WriteLine(contentManager.RootDirectory);
            Helper.HexToRgb("#943989");

            /*itemRegistry.Add(new Fish(0, "bluefin tuna", new Sprite(Vector2.Zero, new Vector2(16, 11), Vector2.Zero, "Art/Food/fish"), 15, FishSpecies.Tuna));
            FileWriter.WriteJson(itemRegistry, "C:/Users/bruno/OneDrive/Počítač/fish.json");*/
            
        }

        protected override void Initialize()
        {

            LineTool.Initialize(GraphicsDevice);
            fishingState = new FishingScene("fishingScene");
            mainMenuState = new MainMenuState("mainMenuScene");
            player= new Player();
            // TODO: Add your initialization logic here
            stateManager = new SceneManagement(mainMenuState, fishingState).SetActive(true,"fishingScene");

            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            stateManager.LoadContent(contentManager);
            CameraManager.LoadContent(contentManager);

            spriteFont = Content.Load<SpriteFont>("Fonts/Font_Pixel");
            Font_10 = Content.Load<SpriteFont>("Fonts/Font_Pixel");

            test1.LoadContent(contentManager);
            test2.LoadContent(contentManager);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            isFocused = this.IsActive;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FPS = 1/(float)gameTime.ElapsedGameTime.TotalSeconds;

            

            if (InputManager.AreKeysBeingPressedDown(false, Keys.P)) 
            {

                CameraManager.GetCurrentCamera().SetShaking(true, .1f, 4); 
            }
            CameraManager.Update(gameTime);
            stateManager.Update(gameTime);
            // TODO: Add your update logic here
            InputManager.Update(gameTime);
            base.Update(gameTime);
        }
        public Sprite test1 = new Sprite(Vector2.Zero, new Vector2(25, 8), Vector2.Zero, "Art/Props/Boat", "boat", .6f).SetColor(Color.Red);
        public Sprite test2 = new Sprite(Vector2.Zero, new Vector2(25, 8), Vector2.Zero, "Art/Props/Boat", "boat", .5f).SetColor(Color.Green);
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp,transformMatrix: CameraManager.GetCurrentMatrix(),depthStencilState:DepthStencilState.Default);
            stateManager.Draw(_spriteBatch);
            LineTool.Draw(_spriteBatch);

            _spriteBatch.End();

            //UI rendering -> on canvas
            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: CameraManager.GetCurrentCamera().uiMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.Default);

            stateManager.DrawUI(_spriteBatch);
            _spriteBatch.End();
            //Text Rendering
            _spriteBatch.Begin(SpriteSortMode.BackToFront,transformMatrix:CameraManager.GetCurrentCamera().textMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.Default);
            stateManager.DrawText(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, "fps:" + FPS.ToString("F2", CultureInfo.InvariantCulture), new Vector2(2, 2), Color.Black);
            _spriteBatch.End();


           // GraphicsDevice.SetRenderTarget(depthBuffer);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
