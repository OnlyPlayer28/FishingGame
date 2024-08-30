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
using Core.Audio;

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

        public static SoundEffect testEffect { get; set; }
        public static string debugText = "";

        private Texture2D scrollingWaves;
        private double waveScrollX;

        internal DayNightSystem dayNightSystem;
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
            isFocused = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
            CameraManager.AddCamera(new Camera(Vector2.Zero, 5, new Vector2(1600, 900), "mainCamera"));
            CameraManager.SetCurrentCamera("mainCamera");

            Console.WriteLine("root directory: "+contentManager.RootDirectory);
            
            itemRegistry = FileWriter.ReadJson < List<IAddableToInventory>>(Content.RootDirectory+"/Data/Food/fish.json");
            itemRegistry.AddRange(FileWriter.ReadJson<List<IAddableToInventory>>(contentManager.RootDirectory + "/Data/Food/consumables.json"));

            itemRegistry.OrderBy(p => p.ID);
            dayNightSystem = new DayNightSystem(10);
            DayNightSystem.SetTime(6, 0);
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
            testEffect.Play();
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            AudioManager.Dispose();
            base.OnExiting(sender, args);
        }
        protected override void LoadContent()
        {
            AudioManager.LoadSongs(Content, "Audio/Songs/", "ocean_waves");

            Font_36 = Content.Load<SpriteFont>("Fonts/Tuna");
            Font_24 = Content.Load<SpriteFont>("Fonts/Tuna_24");
            Font_12 = Content.Load<SpriteFont>("Fonts/Tuna_12");
            Font_HUD = Content.Load<SpriteFont>("Fonts/Tuna_HUD");

            testEffect = Content.Load<SoundEffect>("Audio/error");

            // TODO: use this.Content to load your game content here
            player = new Player();
            LineTool.Initialize(GraphicsDevice);
            fishingState = new FishingScene("fishingScene");
            mainMenuState = new MainMenuState("mainMenuScene");
            restaurantScene = new RestaurantScene("restaurantScene");

            stateManager = new SceneManagement(mainMenuState, fishingState,restaurantScene).SetActive(true, "fishingScene");

            scrollingWaves = Content.Load<Texture2D>("Art/Backdrops/waves");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            stateManager.LoadContent(contentManager);
            CameraManager.LoadContent(contentManager);
            

            player.inventory.AddItem(GetItem(4), 5);
            player.inventory.AddItem(GetItem(5), 5);
            player.inventory.AddItem(GetItem(6), 5);
            player.inventory.AddItem(GetItem(7), 5);
            player.inventory.AddItem(GetItem(8), 5);
            player.inventory.AddItem(GetItem(9), 5);
            player.inventory.AddItem(GetItem(10), 5);
            player.inventory.AddItem(GetItem(11), 5);


        }

        
       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            dayNightSystem.Update(gameTime);
            FPS = 1/(float)gameTime.ElapsedGameTime.TotalSeconds;

            AudioManager.Update(gameTime);
            if (InputManager.AreKeysBeingPressedDown(false, Keys.P)) 
            {
                player.inventory.AddItem(GetItem(2), 1);
            }
            if (InputManager.AreKeysBeingPressedDown(false, Keys.L))
            {
                player.inventory.AddItem(GetItem(2),-1);
            }
            CameraManager.Update(gameTime);
            stateManager.Update(gameTime);
            InputManager.Update(gameTime,isFocused);
            /*debugText = DayNightSystem.GetCurrentDate().ToString();*/
            waveScrollX += 11 * gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //========== World rendering ==========
            _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointWrap,transformMatrix: CameraManager.GetCurrentMatrix(),depthStencilState:DepthStencilState.DepthRead);
            stateManager.Draw(_spriteBatch);
            LineTool.Draw(_spriteBatch);
            if (stateManager.GetActiveGameState().name == "fishingScene")
                _spriteBatch.Draw(scrollingWaves, Vector2.Zero, new Rectangle(-(int)waveScrollX, 0, 128, 128), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, .51f);
            _spriteBatch.End();
            //========== On canvas UI rendering ==========
            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: CameraManager.GetCurrentCamera().uiMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.DepthRead);
            stateManager.DrawUI(_spriteBatch);
            _spriteBatch.End();
            //========== Text rendering ==========
            _spriteBatch.Begin(SpriteSortMode.BackToFront,transformMatrix:CameraManager.GetCurrentCamera().textMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.DepthRead);
            stateManager.DrawText(_spriteBatch);
            _spriteBatch.DrawString(Font_24, debugText, new Vector2(2, 2), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
