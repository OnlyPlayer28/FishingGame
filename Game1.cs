using Core.Cameras;
using Core.SceneManagement;
using Fishing.Scripts.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Core;
using Core.Debug;
using System.Globalization;

namespace Fishing
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        internal static ContentManager contentManager;


        private MainMenuState mainMenuState;
        private FishingState fishingState;
        internal static GameStateManager stateManager;

        internal static CameraManager cameraManager;
        internal static Vector2 resolution = new Vector2(128*5, 128*5);

        static SpriteFont spriteFont;

        public static float FPS { get; private set; }

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
            fishingState = new FishingState("fishingState");
            mainMenuState = new MainMenuState("mainMenuState");
            // TODO: Add your initialization logic here
            stateManager = new GameStateManager(mainMenuState, fishingState).SetActive(true,"fishingState");
            
            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            stateManager.LoadContent(contentManager);
            cameraManager.LoadContent(contentManager);

            spriteFont = Content.Load<SpriteFont>("Fonts/Font_Pixel");
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp,transformMatrix: cameraManager.GetCurrentMatrix());
            stateManager.Draw(_spriteBatch);
            LineTool.Draw(_spriteBatch);
            _spriteBatch.End();


            _spriteBatch.Begin(transformMatrix:cameraManager.GetCurrentCamera().transformMatrixNoScaling);
            _spriteBatch.DrawString(spriteFont, "fps:"+FPS.ToString("F2",CultureInfo.InvariantCulture), new Vector2(2, 2), Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
