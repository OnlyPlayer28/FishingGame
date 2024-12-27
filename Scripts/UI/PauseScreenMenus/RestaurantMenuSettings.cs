using Core;
using Core.Components;
using Core.Debug;
using Core.UI;
using Core.UI.Elements;
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
        public RestaurantMenuSettings(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
           /* Game1.player.restaurantManager.currentRestaurantMenu.AddMenuItem(5);
            Game1.player.restaurantManager.currentRestaurantMenu.AddMenuItem(9);
            Game1.player.restaurantManager.currentRestaurantMenu.AddMenuItem(10, 500);*/
            menuItemContainers = new MenuItemContainer[6];
            for (int i = 0; i < menuItemContainers.Length; i++)
            {
                menuItemContainers[i] = new MenuItemContainer(position + new Vector2(0, 1) + new Vector2(1, i * 16), new Vector2(size.X - 2, 16), layer, canvas, $"menu_{i}");
            }
            for (int i = 0; i < Game1.player.restaurantManager.currentRestaurantMenu.menuItems.Count; i++)
            {
                menuItemContainers[i].SetMenuItem(Game1.player.restaurantManager.currentRestaurantMenu.menuItems[i]);
            }
            addNewItemButton = new Button(new Sprite(menuItemContainers[firstEmptyContainerID].position + new Vector2((size.X - 2) / 2, 2), new Rectangle(16,37,8,8), "Art/UI/UI", layer: layer-.00001f),false,"click").SetButtonName("newItemButton");
            addNewItemButton.OnButtonClickEvent += OnButtonCLick;
            canvas.AddClickableElement(addNewItemButton);

            menuItemContainers[firstEmptyContainerID].SetDisplayMode(ContainerDisplayMode.NextInLineToSet);
        }
        private void OnButtonCLick(Object o,ButtonEventArgs e)
        {
            if (!isActive) {  return; }
            if(e.buttonRef.name == "newItemButton"&&e.buttonRef.isActive)
            {
                menuItemContainers[firstEmptyContainerID].SetDisplayMode(ContainerDisplayMode.ItemSet);
                menuItemContainers[firstEmptyContainerID].SetActive(true);
                if(firstEmptyContainerID == menuItemContainers.Length-1)
                {
                    firstEmptyContainerID = -1;
                    addNewItemButton.isActive = false;
                }else
                {
                    menuItemContainers[firstEmptyContainerID+1].SetDisplayMode(ContainerDisplayMode.NextInLineToSet);
                    menuItemContainers[firstEmptyContainerID+1].SetActive(true);
                    addNewItemButton.SetPosition(menuItemContainers[firstEmptyContainerID+1].position + new Vector2((size.X - 2) / 2, 2));

                    firstEmptyContainerID++;
                }


                //if(firstEmptyContainerID == menuItemContainers.Length) { addNewItemButton.isActive = false; }
            }
        }
        public override IMenu SetActive(bool active)
        {
            for (int i = 0;i <menuItemContainers.Length;i++)
            {
                menuItemContainers[i].SetActive(active);
            }
            addNewItemButton.isActive = false;
            if (firstEmptyContainerID >=0 && active) { addNewItemButton.isActive = true; } 
            return base.SetActive(active);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuItemContainers.Length; i++)
            {
                menuItemContainers[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
