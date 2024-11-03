using Core;
using Core.Audio;
using Core.Components;
using Core.SceneManagement;
using Core.UI;
using Core.UI.Elements;
using Fishing.Core;
using Fishing.Scripts.UI;
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
    internal class RestaurantScene : IScene
    {
        private Sprite backdrop {  get; set; }   
        private Sprite window { get; set; }
        private Sprite windowView { get; set;    }
        public Canvas uiCanvas { get; set; }
        public Button goToOceanButton { get; set; }
        public Button inventoryButton { get; set; }
        public Button cuttingBoardButton { get; set; }
        public InventoryScreen inventoryScreen { get; set; }
        public CuttingBoardScreen cuttingBoardScreen {  get; set; }
        private List<IMenu> openableMenus { get; set; }

        private string[] restaurantTracks { get; set; }

        public HUD hud { get; set; }

        static RestaurantScene()
        {

        }
        public RestaurantScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            uiCanvas = new Canvas("uiCanvas", isActive);
            hud  = new HUD(Vector2.Zero, Vector2.Zero, .05f, uiCanvas).SetActive(true);
            restaurantTracks = FileWriter.ReadJson<string[]>(Game1.contentManager.RootDirectory + "/Data/Audio/restaurantTracks.json");
            AudioManager.LoadSongs(Game1.contentManager, "Audio/Songs/", restaurantTracks);
            openableMenus = new List<IMenu>();
            inventoryScreen = new InventoryScreen(new Vector2(10, 20), new Vector2(90, 80), Layer.UI, uiCanvas,"inventoryScreen");
            cuttingBoardScreen = new CuttingBoardScreen(new Vector2(30, 25), new Vector2(68, 70), Layer.UI, uiCanvas,"cuttingBoardScreen");
            backdrop = new Sprite(Vector2.Zero, new Vector2(128, 128), Vector2.Zero, "Art/Backdrops/restaurant",layer:Layer.Backdrop);
            windowView = new Sprite(new Vector2(18, 13), new Vector2(92, 39), new Vector2(2, 2), "Art/Backdrops/Windows",layer:Layer.Backdrop);
            window = new Sprite(new Vector2(18, 13), new Vector2(92, 39), new Vector2(2, 2), "Art/Backdrops/window_overlay",layer:Layer.Backdrop-.00001f).SetColor(Helper.HexToRgb("#43546e")).SetTransparency(.25f);

            goToOceanButton = new Button(new Vector2(1, 2), new Vector2(10), layer:Layer.Overlay, onClickSound: "click")
                .SetButtonSprite(new Sprite(new Vector2(1, 2), new Vector2(10, 10), new Vector2(28, 19), "Art/UI/UI"),Game1.contentManager).SetOnButtonCLickAction(GoToOcean);

            inventoryButton = new Button(new Sprite(new Vector2(91, 89), new Vector2(30, 34), new Vector2(70, 0), "Art/UI/UI", layer: Layer.Overlay),onClickSound:"click")
                .SetOnButtonCLickAction(OpenInventory);
            cuttingBoardButton = new Button(new Vector2(87, 75), new Vector2(27, 10), 0, onClickSound: "click")
                .SetButtonSprite(new Sprite(new Vector2(87, 75), new Vector2(27, 10), new Vector2(100, 0), "Art/UI/UI", layer:Layer.Overlay), Game1.contentManager)
                .SetOnButtonCLickAction(OpenCuttingBoard);
            uiCanvas.isActive = true;
            uiCanvas.AddClickableElement(goToOceanButton);
            uiCanvas.AddClickableElement(inventoryButton);
            uiCanvas.AddClickableElement(cuttingBoardButton);

            uiCanvas.AddUIELement(hud);
            
            components.Add(backdrop);
            components.Add(window);
            components.Add(windowView);
            openableMenus.Add(inventoryScreen);
            openableMenus.Add(cuttingBoardScreen);
            LoadContent(Game1.contentManager);

            
        }
        public override void LoadContent(ContentManager contentManager)
        {

            inventoryScreen.LoadContent(contentManager);
            cuttingBoardScreen.LoadContent(contentManager);
            components.ForEach(p => p.LoadContent(contentManager));
            uiCanvas.LoadContent(contentManager);
        }
        public override IScene SetActive(bool active)
        {
            if (active) { AudioManager.PlaySong(restaurantTracks[ReferenceHolder.random.Next(0, restaurantTracks.Length)],true,.06f); }
            return base.SetActive(active);
        }
        private bool CanOpenMenu(string menuName)
        {
            return !openableMenus.Any(p => p.name != menuName && p.isActive);
        }
        public void GoToOcean()
        {
            Game1.stateManager.SetActive(true, "fishingScene");
            inventoryScreen.LoadContent(Game1.contentManager);
        }
        public void OpenInventory()
        {
            if(CanOpenMenu("inventoryScreen"))
                inventoryScreen.SetActive(!inventoryScreen.isActive);
        }

        public void OpenCuttingBoard()
        {
            if (CanOpenMenu("cuttingBoardScreen"))
            {
                cuttingBoardScreen.SetActive(!cuttingBoardScreen.isActive);
            }
        }
            
        public override void Draw(SpriteBatch spriteBatch)
        {
                components.ForEach(p => p.Draw(spriteBatch));
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
                openableMenus.ForEach(p => p.Draw(spriteBatch));
                uiCanvas.Draw(spriteBatch);
                uiCanvas.Draw(spriteBatch);
        }



        public override void Update(GameTime gameTime)
        {
                openableMenus.ForEach(p => p.Update(gameTime));

                uiCanvas.Update(gameTime);
                components.ForEach(p => p.Update(gameTime));
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
                uiCanvas.DrawText(spriteBatch);
                uiCanvas.DrawText(spriteBatch);
        }
    }
}

