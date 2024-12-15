using Core;
using Core.Components;
using Core.Debug;
using Core.SceneManagement;
using Core.UI;
using Core.UI.Elements;
using Fishing.Scripts.UI.PauseScreenMenus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Scenes
{
    public enum PauseMenuPages
    {
        Restaurant,
        Menu,
        Settings,
        Exit
    }
    internal class PauseScene : IScene
    {
        private Rect backdrop { get; set; }
        private Rect outline { get; set; }
        private Canvas canvas { get; set; }
        private Text currentPageText { get; set; }
        private Button restaurantButton { get; set; }
        private Button menuButton { get; set; }
        private Button settingsButton { get; set; }
        private Button exitButton { get; set; }
        private Line buttonsDevider { get; set; }

        private IMenu[] pages { get; set; } = new IMenu[4];
        private int currentPage = 0;
        public PauseScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            canvas = new Canvas("canvas", true);
            currentPageText = new Text(new Vector2((128 / 2) -14, 2), $"-={PauseMenuPages.Restaurant}=-", Color.White, Game1.Font_24, layer: Layer.UI,isActive:true);
            restaurantButton = new Button(new Vector2(11, 11), new Vector2(11, 11), Layer.UI - .001f, PauseMenuPages.Restaurant.ToString(), true, "click").SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34"));
            menuButton = new Button(new Vector2(24, 11), new Vector2(11, 11), Layer.UI - .001f, PauseMenuPages.Menu.ToString(), true, "click").SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34"));
            settingsButton = new Button(new Vector2(37, 11), new Vector2(11, 11), Layer.UI - .001f, PauseMenuPages.Settings.ToString(), true, "click").SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34"));
            exitButton = new Button(new Vector2(50, 11), new Vector2(11, 11), Layer.UI - .001f, PauseMenuPages.Exit.ToString(), true, "click").SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34"));

            canvas.AddClickableElement(restaurantButton);
            canvas.AddClickableElement(menuButton);
            canvas.AddClickableElement(settingsButton);
            canvas.AddClickableElement(exitButton);
            canvas.AddTextElement(currentPageText);
            backdrop = new Rect(new Vector2(3,3), new Vector2(122, 122), Helper.HexToRgb("#542424"), true, Layer.UI).SetFillColor(Helper.HexToRgb("#6e3b34"));
            outline = new Rect(new Vector2(9,9), new Vector2(111, 111), Helper.HexToRgb("#a06f51"),  layer: Layer.UI-.00000001f);
            buttonsDevider = new Line(new Vector2(9, 23), new Vector2(111, 1), Helper.HexToRgb("#a06f51"), layer: Layer.UI - .00000001f);

            components.Add(backdrop);
            components.Add(outline);
            components.Add(buttonsDevider);

            LoadContent(Game1.contentManager);


            restaurantButton.OnButtonClickEvent += OnButtonClick;
            menuButton.OnButtonClickEvent += OnButtonClick;
            settingsButton.OnButtonClickEvent += OnButtonClick;
            exitButton.OnButtonClickEvent += OnButtonClick;

            canvas.AddUIELement(new Image(new Sprite(new Vector2(13,13), new Vector2(6, 7), new Vector2(13, 30), "Art/UI/UI", layer: Layer.UI - .01f),true));
            canvas.AddUIELement(new Image(new Sprite(new Vector2(26, 13), new Vector2(6, 7), new Vector2(7, 30), "Art/UI/UI", layer: Layer.UI - .01f),true));
            canvas.AddUIELement(new Image(new Sprite(new Vector2(39, 13), new Vector2(7, 7), new Vector2(19, 30), "Art/UI/UI", layer: Layer.UI - .01f),true));
            canvas.AddUIELement(new Image(new Sprite(new Vector2(52, 13), new Vector2(8, 7), new Vector2(26, 30), "Art/UI/UI", layer: Layer.UI - .01f), true));

            pages[0] = new RestaurantSettingsMenu(new Vector2(10,24),new Vector2(100),Layer.UI-.0001f,canvas);
            pages[1] = new RestaurantMenuSettings(new Vector2(10, 24), new Vector2(100), Layer.UI - .0001f, canvas);
            pages[2] = new SettingsMenu(new Vector2(10, 24), new Vector2(100), Layer.UI - .0001f, canvas);
            pages[3] = new ExitMenu(new Vector2(10, 24), new Vector2(100), Layer.UI - .0001f, canvas);

            pages[0].SetActive(true);


        }
        public override IScene SetActive(bool active)
        {
            canvas.isActive = active;
            if (active) { Game1.disableHUDGloballyEvent?.Invoke(this, EventArgs.Empty); }
            else { Game1.enableHUDGloballyEvent?.Invoke(this, EventArgs.Empty); }
            return base.SetActive(active);
        }
        public override void LoadContent(ContentManager contentManager)
        {
            canvas.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }
        public void OnButtonClick(Object o,ButtonEventArgs e)
        {
            currentPage = (int)Enum.Parse(typeof(PauseMenuPages),e.buttonRef.name);
            for (int i = 0; i < pages.Length; i++)
            {
                if(i == currentPage) { pages[i].SetActive(true); }
                else {  pages[i].SetActive(false); }
            }
            currentPageText.text = $"-={e.buttonRef.name}=-";
        }
        public void OnMouseClick(Object o,MouseInputEventArgs e)
        {

        }
        public override void Update(GameTime gameTime)
        {
            canvas.Update(gameTime);
            pages[currentPage].Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            canvas.DrawText(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            
            canvas.Draw(spriteBatch);
            pages[currentPage].Draw(spriteBatch);
        }
    }
}
