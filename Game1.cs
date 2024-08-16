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
using Microsoft.Xna.Framework.Audio;

namespace Fishing
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        internal static ContentManager contentManager;


        private MainMenuState mainMenuState;
        private FishingScene fishingState;
        private RestaurantScene restaurantScene;
        internal static SceneManagement stateManager;

        internal static CameraManager cameraManager;
        internal static Vector2 resolution = new Vector2(128*5, 128*5);

        static SpriteFont spriteFont;

        public static float FPS { get; private set; }

        public static Player player { get; private set; }

        public static SpriteFont Font_36;
        public static SpriteFont Font_24;
        public static SpriteFont Font_12;
        public static SpriteFont Font_HUD;

        public static Color gold = new Color(192, 139, 59);

        public static bool isFocused { get; private set; }


        public static List<IAddableToInventory> itemRegistry;

        private SoundEffect testEffect { get; set; }
        /// <summary>
        /// Returns a new instance of an inventory item.
        /// </summary>
        /// <returns></returns>
        public static IAddableToInventory GetItem(int ID)
        {
            return (IAddableToInventory)itemRegistry.Where(p=>p.ID == ID).FirstOrDefault().Clone();
        }
        public Game1()
        {
            itemRegistry = new List<IAddableToInventory>();

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)resolution.X; _graphics.PreferredBackBufferHeight =(int)resolution.Y;

            _graphics.IsFullScreen = true;
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
            CameraManager.AddCamera(new Camera(Vector2.Zero, 5, new Vector2(1600, 900), "mainCamera"));
            CameraManager.SetCurrentCamera("mainCamera");

            Console.WriteLine("root directory: "+contentManager.RootDirectory);
 
            itemRegistry = FileWriter.ReadJson < List<IAddableToInventory>>(Content.RootDirectory+"/Data/Food/fish.json");

        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Activated += OnFocus;
            this.Deactivated += OnFocusLoss;
        }
        public void OnFocus(Object o,EventArgs e)
        {
            isFocused = true;
        }
        public void OnFocusLoss(Object o,EventArgs e)
        {
            isFocused = false;
            //testEffect.Play();
        }
        protected override void LoadContent()
        {


            Font_36 = Content.Load<SpriteFont>("Fonts/Tuna");
            Font_24 = Content.Load<SpriteFont>("Fonts/Tuna_24");
            Font_12 = Content.Load<SpriteFont>("Fonts/Tuna_12");
            Font_HUD = Content.Load<SpriteFont>("Fonts/Tuna_HUD");

            testEffect = Content.Load<SoundEffect>("Audio/error");

            // TODO: use this.Content to load your game content here

            LineTool.Initialize(GraphicsDevice);
            fishingState = new FishingScene("fishingScene");
            mainMenuState = new MainMenuState("mainMenuScene");
            restaurantScene = new RestaurantScene("restaurantScene");
            player = new Player();
            stateManager = new SceneManagement(mainMenuState, fishingState,restaurantScene).SetActive(true, "fishingScene");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            stateManager.LoadContent(contentManager);
            CameraManager.LoadContent(contentManager);
        }

        

        protected override void Update(GameTime gameTime)
        {
            //Unfortunately the isActive tracks the debug console instead of the game window :(,so when I develop an in-game console I can properly re-implement this feature.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FPS = 1/(float)gameTime.ElapsedGameTime.TotalSeconds;


            if (InputManager.AreKeysBeingPressedDown(false, Keys.P)) 
            {
                CameraManager.GetCurrentCamera().SetShaking(true, .1f, 4); 
            }
            CameraManager.Update(gameTime);
            stateManager.Update(gameTime);
            InputManager.Update(gameTime,isFocused);
            inventoryText = "inventory:"+InputManager.inputState;
            foreach (var item in player.inventory.inventory)
            {
                inventoryText += $"\n {item.Item2.name}: {item.Item1}";
            } 
            base.Update(gameTime);
        }
        public static string inventoryText = "";
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //========== World rendering ==========
            _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp,transformMatrix: CameraManager.GetCurrentMatrix(),depthStencilState:DepthStencilState.DepthRead);
            stateManager.Draw(_spriteBatch);
            LineTool.Draw(_spriteBatch);
            _spriteBatch.End();
            //========== On canvas UI rendering ==========
            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: CameraManager.GetCurrentCamera().uiMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.DepthRead);
            stateManager.DrawUI(_spriteBatch);
            _spriteBatch.End();
            //========== Text rendering ==========
            _spriteBatch.Begin(SpriteSortMode.BackToFront,transformMatrix:CameraManager.GetCurrentCamera().textMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.DepthRead);
            stateManager.DrawText(_spriteBatch);
            _spriteBatch.DrawString(Font_24, inventoryText, new Vector2(2, 2), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
