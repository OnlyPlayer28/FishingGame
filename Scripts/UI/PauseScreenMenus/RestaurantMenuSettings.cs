using Core;
using Core.Audio;
using Core.Components;
using Core.Debug;
using Core.UI;
using Core.UI.Elements;
using Fishing.Scripts.Restaurant;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI.PauseScreenMenus
{
    internal class RestaurantMenuSettings : IMenu
    {
        private MenuItemContainer[] menuItemContainers { get; set; }
        private Button addNewItemButton;
        private int firstEmptyContainerID = 0;

        private MenuItemSelectionScreen itemSelectionScreen { get; set; }
        private int selectionScreenOpenedByID { get; set; } 
        public RestaurantMenuSettings(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            itemSelectionScreen = new MenuItemSelectionScreen(position, size, layer-.000009f, canvas, "selectionScreen");
            menuItemContainers = new MenuItemContainer[6];
            for (int i = 0; i < menuItemContainers.Length; i++)
            {
                menuItemContainers[i] = new MenuItemContainer(position + new Vector2(0, 1) + new Vector2(1, i * 16), new Vector2(size.X - 2, 16), layer - .0000000001f, canvas, $"{i}");
                menuItemContainers[i].openItemSelectionAction = ()=>itemSelectionScreen.SetActive(true);
                menuItemContainers[i].SetActive(false);
                menuItemContainers[i].OnSelectionScreenOpened += OnSelectionScreenSelected;
            }
            for (int i = 0; i < Game1.player.restaurantManager.currentRestaurantMenu.menuItems.Count; i++)
            {
                menuItemContainers[i].SetMenuItem(Game1.player.restaurantManager.currentRestaurantMenu.menuItems[i]);
            }
            addNewItemButton = new Button(menuItemContainers[firstEmptyContainerID].position + new Vector2((size.X - 2) / 2, 2), new Vector2(8), layer: layer - .0000001f, "", false, "click").SetButtonName("newItemButton").SetSimpleSprite(Color.Red, Color.Black);
            addNewItemButton.OnButtonClickEvent += OnButtonCLick;
            canvas.AddClickableElement(addNewItemButton);

            itemSelectionScreen.OnItemChooseEvent += OnMenuItemSelectionMade;
            itemSelectionScreen.OnSetActiveEvent += OnSelectionScreenTurnedOn;
        }
        private void SetCorrectNewButtonPosition(int containerID)
        {
            addNewItemButton.SetPosition(menuItemContainers[containerID].position + new Vector2((size.X - 2) / 2, 2));
        }
        private void OnButtonCLick(Object o,ButtonEventArgs e)
        {
            if (!isActive) {  return; }
            if(e.buttonRef.name == "newItemButton"&&e.buttonRef.isActive)
            {
                menuItemContainers[firstEmptyContainerID].SetActive(true);
                if(firstEmptyContainerID == menuItemContainers.Length-1)
                {
                    firstEmptyContainerID = -1;
                    addNewItemButton.isActive = false;
                }else
                {

                    firstEmptyContainerID = FindFirstInactiveContainer();
                    SetCorrectNewButtonPosition(firstEmptyContainerID);

                }


                //if(firstEmptyContainerID == menuItemContainers.Length) { addNewItemButton.isActive = false; }
            }
        }
        private void OnSelectionScreenTurnedOn(Object o,SelectScreenActiveEventArgs e)
        {
            //addNewItemButton.isActive=!e.active;
        }

        private void OnSelectionScreenSelected(Object o,EventArgs e)
        {
            selectionScreenOpenedByID =Convert.ToInt16( ((MenuItemContainer)o).name);
        }
        private int FindFirstEmptyContainer()
        {
            if (menuItemContainers.All(p => p.menuItem.HasValue)) { return -1; }
            return Convert.ToInt16(menuItemContainers.First(p => !p.menuItem.HasValue).name);
        }
        private int FindFirstInactiveContainer()
        {
            return Convert.ToInt16(menuItemContainers.First(p => !p.isActive).name);
        }
        public void OnMenuItemSelectionMade(Object o, MenuItemSelectionEventArgs e)
        {
            if(menuItemContainers.Any(p=>p.menuItem.HasValue ? p.menuItem.Value.ID==e.ID:false ))
            {
                AudioManager.PlayCue("error");
            }else
            {
                if (menuItemContainers[selectionScreenOpenedByID].menuItem != null)
                {
                    Game1.player.restaurantManager.currentRestaurantMenu.RemoveMenuItem(menuItemContainers[selectionScreenOpenedByID].menuItem.Value.ID);
                }
                Game1.player.restaurantManager.currentRestaurantMenu.AddMenuItem(e.ID);
                menuItemContainers[selectionScreenOpenedByID].SetMenuItem(new MenuItem(e.ID));
                AudioManager.PlayCue("success");
            }
        }
        public override IActive SetActive(bool active)
        {
            if (!isActive)
            {
                firstEmptyContainerID = FindFirstEmptyContainer();
            }

            for (int i = 0;i <menuItemContainers.Length;i++)
            {
                if (active)
                {
                    if (menuItemContainers[i].menuItem.HasValue) { menuItemContainers[i].SetActive(true); }
                    else { menuItemContainers[i].SetActive(false); }

                }
                else
                {
                    menuItemContainers[i].SetActive(active);
                }
            }
            if(active)
            {
                if (firstEmptyContainerID != -1)
                {
                    SetCorrectNewButtonPosition(firstEmptyContainerID);
                    addNewItemButton.isActive = true;
                }
            }
            else
            {
                addNewItemButton.isActive = false;
            }

            if (!active) { itemSelectionScreen.SetActive(false); }
            return base.SetActive(active);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            /*for (int i = 0; i < menuItemContainers.Length; i++)
            {
                menuItemContainers[i].Draw(spriteBatch);
            }*/
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            itemSelectionScreen.Update(gameTime);
            foreach (var item in menuItemContainers)
            {
                item.Update(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
