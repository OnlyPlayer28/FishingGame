using Core.Components;
using Core.SceneManagement;
using Core.UI;
using Core.UI.Elements;
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
        public RestaurantScene(string name, bool isActive = false, bool isDrawing = false) 
            : base(name, isActive, isDrawing)
        {
            openableMenus = new List<IMenu>();
            uiCanvas = new Canvas("uiCanvas", isActive);
            inventoryScreen = new InventoryScreen(new Vector2(10, 20), new Vector2(90, 80), .1f, uiCanvas,"inventoryScreen");
            cuttingBoardScreen = new CuttingBoardScreen(new Vector2(10, 20), new Vector2(90, 80), .1f, uiCanvas,"cuttingBoardScreen");
            backdrop = new Sprite(Vector2.Zero, new Vector2(128, 128), Vector2.Zero, "Art/Backdrops/restaurant",layer:.0002f);
            windowView = new Sprite(new Vector2(18, 13), new Vector2(92, 39), new Vector2(2, 2), "Art/Backdrops/Windows",layer:.0001f);
          //  window = new Sprite(new Vector2(18, 13), new Vector2(92, 39), new Vector2(2, 2), "Art/Backdrops/window_overlay")/*.SetColor(new Color(172, 185, 204, 10))*/;

            goToOceanButton = new Button(new Vector2(1, 2), new Vector2(10), 0)
                .SetButtonSprite(new Sprite(new Vector2(1, 2), new Vector2(10, 10), new Vector2(28, 19), "art/UI/UI"),Game1.contentManager).SetOnButtonCLickAction(GoToOcean);

            inventoryButton = new Button(new Vector2(91, 89), new Vector2(29, 34), 0)
                .SetButtonSprite(new Sprite(new Vector2(91, 89), new Vector2(30, 34), new Vector2(70, 0), "art/UI/UI", layer: .25f), Game1.contentManager)
                .SetOnButtonCLickAction(OpenInventory);
            cuttingBoardButton = new Button(new Vector2(87, 75), new Vector2(27, 10), 0)
                .SetButtonSprite(new Sprite(new Vector2(87, 75), new Vector2(27, 10), new Vector2(100, 0), "art/UI/UI", layer: .25f), Game1.contentManager)
                .SetOnButtonCLickAction(OpenCuttingBoard);
            uiCanvas.isActive = true;
            uiCanvas.AddClickableElement(goToOceanButton);
            uiCanvas.AddClickableElement(inventoryButton);
            uiCanvas.AddClickableElement(cuttingBoardButton);
            
            components.Add(backdrop);
           // components.Add(window);
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
            if (isActive || isDrawing)
            {
                components.ForEach(p => p.Draw(spriteBatch));
            }
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            if (isActive || isDrawing)
            {
                inventoryScreen.Draw(spriteBatch);
                uiCanvas.Draw(spriteBatch);
                uiCanvas.Draw(spriteBatch);
            }
        }



        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                inventoryScreen.Update(gameTime);
                uiCanvas.Update(gameTime);
                components.ForEach(p => p.Update(gameTime));
            }
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            if (isActive || isDrawing)
            {
                uiCanvas.DrawText(spriteBatch);
                uiCanvas.DrawText(spriteBatch);
            }
        }
    }
}

