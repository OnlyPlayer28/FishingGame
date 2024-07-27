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

        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)resolution.X; _graphics.PreferredBackBufferHeight =(int)resolution.Y;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
            cameraManager = new CameraManager().AddCamera(new Camera(Vector2.Zero, 5, new Vector2(1600, 900), "mainCamera")).SetCurrentCamera("mainCamera");

            string textFile = File.ReadAllText(contentManager.RootDirectory + "/Art/test.txt");
            Console.WriteLine(textFile);
            Console.WriteLine(contentManager.RootDirectory);

        }

        protected override void Initialize()
        {
            LineTool.Initialize(GraphicsDevice);
            fishingState = new FishingScene("fishingState");
            mainMenuState = new MainMenuState("mainMenuState");
            player= new Player();
            // TODO: Add your initialization logic here
            stateManager = new SceneManagement(mainMenuState, fishingState).SetActive(true,"fishingState");
            
            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            stateManager.LoadContent(contentManager);
            cameraManager.LoadContent(contentManager);

            spriteFont = Content.Load<SpriteFont>("Fonts/Verdana_10px");
            Font_10 = Content.Load<SpriteFont>("Fonts/Font_Pixel");

            test1.LoadContent(contentManager);
            test2.LoadContent(contentManager);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FPS = 1/(float)gameTime.ElapsedGameTime.TotalSeconds;


            if (InputManager.AreKeysBeingPressedDown(false, Keys.P)) 
            {

                cameraManager.GetCurrentCamera().SetShaking(true, .1f, 4); 
            }
            cameraManager.Update(gameTime);
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
            _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp,transformMatrix: cameraManager.GetCurrentMatrix(),depthStencilState:DepthStencilState.Default);
            stateManager.Draw(_spriteBatch);
            LineTool.Draw(_spriteBatch);
            _spriteBatch.End();


            //Text Rendering
            _spriteBatch.Begin(SpriteSortMode.BackToFront,transformMatrix:cameraManager.GetCurrentCamera().textMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.Default);
            stateManager.DrawText(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, "fps:" + FPS.ToString("F2", CultureInfo.InvariantCulture), new Vector2(2, 2), Color.Black);
            _spriteBatch.End();
            //UI rendering -> on canvas
            _spriteBatch.Begin(SpriteSortMode.BackToFront,transformMatrix: cameraManager.GetCurrentCamera().uiMatrix, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.Default);

            stateManager.DrawUI(_spriteBatch);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
